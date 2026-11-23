using Microsoft.Playwright;

namespace TestInfra.Pom.PageObjects;

/// <summary>
/// Base class for page objects (whole pages or components). Properties marked with a locator attribute
/// are resolved against <see cref="Root"/> and populated by <see cref="PageObjectFactory"/>.
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
}
