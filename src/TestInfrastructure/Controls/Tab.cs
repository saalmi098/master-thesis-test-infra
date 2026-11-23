using Microsoft.Playwright;
using TestInfrastructure.Controls.Base;

namespace TestInfrastructure.Controls;

/// <summary>
/// A tab header. The located element may be the tab itself or a child of it.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Tab(ILocator locator) : Control(locator)
{
    private ILocator TabElement => Locator.Locator("xpath=ancestor-or-self::*[@role='tab'][1]");

    /// <summary>
    /// Activates the tab by clicking it.
    /// </summary>
    public Task SelectAsync() => Locator.ClickAsync();

    /// <summary>
    /// Returns true if the tab is selected.
    /// </summary>
    public async Task<bool> IsSelectedAsync() =>
        await TabElement.GetAttributeAsync("aria-selected") == "true";
}
