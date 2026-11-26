using Microsoft.Playwright;
using TestInfra.Extensions;

namespace TestInfra.Controls.Base;

/// <summary>
/// Base class for UI controls; wraps the Playwright locator of one element.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public abstract class Control(ILocator locator)
{
    /// <summary>
    /// The wrapped element's locator, for direct Playwright access and assertions.
    /// </summary>
    public ILocator Locator { get; } = locator;

    /// <summary>
    /// Returns the element's rendered text (innerText).
    /// </summary>
    public Task<string> TextAsync() => Locator.InnerTextAsync();

    /// <summary>
    /// Starts an assertion chain on the wrapped element.
    /// Usage: <c>control.Expect().ToHave...(...)</c>.
    /// </summary>
    public ILocatorAssertions Expect() => Locator.Expect();
}
