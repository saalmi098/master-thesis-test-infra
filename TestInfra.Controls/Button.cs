using Microsoft.Playwright;
using TestInfra.Controls.Base;

namespace TestInfra.Controls;

/// <summary>
/// A clickable button.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Button(ILocator locator) : Control(locator)
{
    /// <summary>
    /// Clicks the button.
    /// </summary>
    public Task ClickAsync() => Locator.ClickAsync();
}
