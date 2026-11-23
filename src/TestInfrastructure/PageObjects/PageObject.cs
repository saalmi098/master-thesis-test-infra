using System.Diagnostics;
using System.Reflection;
using Microsoft.Playwright;
using TestInfrastructure.Attributes;

namespace TestInfrastructure.PageObjects;

/// <summary>
/// Base class for page objects (whole pages or components). Properties marked with a locator attribute
/// are resolved against <see cref="Root"/> and populated by <see cref="InitializeAsync()"/>.
/// </summary>
public abstract class PageObject
{
    /// <summary>
    /// Page-level page object; <see cref="Root"/> is the whole document.
    /// </summary>
    /// <param name="page">Page to wrap.</param>
    protected PageObject(IPage page)
    {
        Page = page;
        Root = page.Locator("html");
    }

    /// <summary>
    /// Component page object; all locators resolve inside <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Container that scopes all locators.</param>
    protected PageObject(ILocator root)
    {
        Root = root;
        Page = root.Page;
    }

    /// <summary>
    /// The Playwright page this object belongs to.
    /// </summary>
    public IPage Page { get; }

    /// <summary>
    /// Scope for all attribute locators of this object.
    /// </summary>
    public ILocator Root { get; }

    /// <summary>
    /// Creates the page object via reflection (bypasses C# 'required' checks, since properties are
    /// populated by <see cref="InitializeAsync"/>), then initializes it.
    /// </summary>
    /// <typeparam name="T">Page object type; needs a constructor taking <see cref="IPage"/>.</typeparam>
    /// <param name="page">Page to wrap.</param>
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
    /// <typeparam name="T">Page object type; needs a constructor taking <see cref="ILocator"/>.</typeparam>
    /// <param name="root">Container that scopes all locators.</param>
    public static async Task<T> CreateAsync<T>(ILocator root) where T : PageObject
    {
        var pageObject = (T)Activator.CreateInstance(typeof(T), root)!;
        await pageObject.InitializeAsync();
        return pageObject;
    }

    /// <summary>
    /// Populates all attributed properties, recursing into nested page objects. Required elements are waited
    /// for until visible (throws on timeout); optional elements are only checked once and left null if absent.
    /// </summary>
    public Task InitializeAsync() => InitializeAsync(0);

    /// <summary>
    /// Initializes this object's locator properties and logs timing, indented by nesting <paramref name="depth"/>.
    /// </summary>
    /// <param name="depth">Nesting level, used for log indentation.</param>
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

    /// <summary>
    /// Creates the control for one property and assigns it. Required: waits until visible. Optional: leaves
    /// the property null if not visible. Nested page objects are initialized afterwards.
    /// </summary>
    /// <param name="prop">Property to populate.</param>
    /// <param name="attr">Locator attribute declared on <paramref name="prop"/>.</param>
    /// <param name="depth">Nesting level, used for log indentation.</param>
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

    /// <summary>
    /// Waits until <paramref name="locator"/> is visible (Playwright default timeout) and logs the wait time.
    /// </summary>
    /// <param name="locator">Locator to wait for.</param>
    /// <param name="label">Name used in log output.</param>
    /// <param name="depth">Nesting level, used for log indentation.</param>
    private static async Task WaitUntilVisibleAsync(ILocator locator, string label, int depth)
    {
        PageObjectLog.Write(depth, $"waiting for {label} to be visible");
        var wait = Stopwatch.StartNew();

        await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        PageObjectLog.Write(depth, $"{label} visible after {wait.ElapsedMilliseconds} ms");
    }
}
