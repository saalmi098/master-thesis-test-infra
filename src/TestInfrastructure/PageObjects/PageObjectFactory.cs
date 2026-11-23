using System.Diagnostics;
using System.Reflection;
using Microsoft.Playwright;
using TestInfrastructure.Attributes;

namespace TestInfrastructure.PageObjects;

/// <summary>
/// Creates page objects and populates their locator-attributed properties. Required elements are waited
/// for until visible (throws on timeout); optional elements are only checked once and left null if absent.
/// </summary>
public static class PageObjectFactory
{
    /// <summary>
    /// Creates the page object via reflection and initializes it.
    /// </summary>
    /// <typeparam name="T">Page object type; needs a constructor taking <see cref="IPage"/>.</typeparam>
    /// <param name="page">Page to wrap.</param>
    public static async Task<T> CreateAsync<T>(IPage page) where T : PageObject
    {
        var pageObject = (T)Activator.CreateInstance(typeof(T), page)!;
        await InitializeAsync(pageObject, 0);
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
        await InitializeAsync(pageObject, 0);
        return pageObject;
    }

    /// <summary>
    /// Initializes the page object's locator properties (recursing into nested page objects).
    /// </summary>
    /// <param name="pageObject">Page object to populate.</param>
    /// <param name="depth">Nesting level, used for log indentation.</param>
    private static async Task InitializeAsync(PageObject pageObject, int depth)
    {
        var name = pageObject.GetType().Name;
        var total = Stopwatch.StartNew();
        PageObjectLog.Write(depth, $"creating page object {name}");

        foreach (var (prop, attr) in GetLocatorProperties(pageObject))
        {
            await InitializePropertyAsync(pageObject, prop, attr, depth + 1);
        }

        PageObjectLog.Write(depth, $"page object {name} ready after {total.ElapsedMilliseconds} ms");
    }

    /// <summary>
    /// Returns attributed properties, required ones first, optional elements last
    /// </summary>
    /// <param name="pageObject">Page object to inspect.</param>
    private static IEnumerable<(PropertyInfo Prop, LocatorAttribute Attr)> GetLocatorProperties(PageObject pageObject)
        => pageObject.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(p => (Prop: p, Attr: p.GetCustomAttribute<LocatorAttribute>()))
            .Where(x => x.Attr != null)
            .OrderBy(x => x.Attr!.Optional)
            .Select(x => (x.Prop, x.Attr!));

    /// <summary>
    /// Creates the control for one property and assigns it. Required: waits until visible. Optional: leaves
    /// the property null if not visible. Nested page objects are initialized afterwards.
    /// </summary>
    /// <param name="pageObject">Page object owning the property.</param>
    /// <param name="prop">Property to populate.</param>
    /// <param name="attr">Locator attribute declared on <paramref name="prop"/>.</param>
    /// <param name="depth">Nesting level, used for log indentation.</param>
    private static async Task InitializePropertyAsync(PageObject pageObject,
                                                      PropertyInfo prop,
                                                      LocatorAttribute attr,
                                                      int depth)
    {
        var label = $"{pageObject.GetType().Name}.{prop.Name}";
        var locator = attr.Resolve(pageObject.Root);

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
        prop.SetValue(pageObject, value);

        if (!attr.Optional)
        {
            await WaitUntilVisibleAsync(locator, label, depth);
        }

        if (value is PageObject nested)
        {
            await InitializeAsync(nested, depth);
        }
    }

    /// <summary>
    /// Waits until <paramref name="locator"/> is visible (Playwright default timeout).
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
