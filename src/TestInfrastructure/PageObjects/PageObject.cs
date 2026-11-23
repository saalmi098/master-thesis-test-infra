using System.Diagnostics;
using System.Reflection;
using Microsoft.Playwright;
using TestInfrastructure.Attributes;

namespace TestInfrastructure.PageObjects;

public abstract class PageObject
{
    protected PageObject(IPage page)
    {
        Page = page;
        Root = page.Locator("html");
    }

    protected PageObject(ILocator root)
    {
        Root = root;
        Page = root.Page;
    }

    public IPage Page { get; }

    public ILocator Root { get; }

    /// <summary>
    /// Creates the page object via reflection (bypasses C# 'required' checks, since properties are
    /// populated by <see cref="InitializeAsync"/>), then initializes it.
    /// </summary>
    public static async Task<T> CreateAsync<T>(IPage page) where T : PageObject
    {
        var pageObject = (T)Activator.CreateInstance(typeof(T), page)!;
        await pageObject.InitializeAsync();
        return pageObject;
    }

    /// <summary>
    /// Same as <see cref="CreateAsync{T}(IPage)"/>, scoped to a container locator
    /// (e.g. a tab panel that only exists after the tab was activated).
    /// </summary>
    public static async Task<T> CreateAsync<T>(ILocator root) where T : PageObject
    {
        var pageObject = (T)Activator.CreateInstance(typeof(T), root)!;
        await pageObject.InitializeAsync();
        return pageObject;
    }

    /// <summary>Populates attributed properties and waits for each (recursively for nested page objects).</summary>
    public Task InitializeAsync() => InitializeAsync(0);

    private async Task InitializeAsync(int depth)
    {
        var name = GetType().Name;
        var total = Stopwatch.StartNew();
        PageObjectLog.Write(depth, $"creating page object {name}");

        foreach (var (prop, attr) in GetLocatorProperties())
        {
            await InitializePropertyAsync(prop, attr, depth + 1);
        }

        PageObjectLog.Write(depth, $"page object {name} ready after {total.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// Returns attributed properties, required ones first so optional elements are checked once the page has settled.
    /// </summary>
    private IEnumerable<(PropertyInfo Prop, LocatorAttribute Attr)> GetLocatorProperties()
        => GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(p => (Prop: p, Attr: p.GetCustomAttribute<LocatorAttribute>()))
            .Where(x => x.Attr != null)
            .OrderBy(x => x.Attr!.Optional)
            .Select(x => (x.Prop, x.Attr!));

    private async Task InitializePropertyAsync(PropertyInfo prop, LocatorAttribute attr, int depth)
    {
        var label = $"{GetType().Name}.{prop.Name}";
        var locator = attr.Resolve(Root);

        if (attr.Optional)
        {
            var present = await locator.IsVisibleAsync();
            PageObjectLog.Write(depth, $"{label} optional, {(present ? "present" : "not present")}");
            if (!present)
            {
                return; // property stays null
            }
        }

        var value = Activator.CreateInstance(prop.PropertyType, locator)
            ?? throw new InvalidOperationException($"Cannot create {prop.PropertyType} for {label}.");
        prop.SetValue(this, value);

        if (!attr.Optional)
        {
            await WaitUntilVisibleAsync(locator, label, depth);
        }

        if (value is PageObject nested)
        {
            await nested.InitializeAsync(depth);
        }
    }

    private static async Task WaitUntilVisibleAsync(ILocator locator, string label, int depth)
    {
        PageObjectLog.Write(depth, $"waiting for {label} to be visible");
        var wait = Stopwatch.StartNew();

        await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        PageObjectLog.Write(depth, $"{label} visible after {wait.ElapsedMilliseconds} ms");
    }
}
