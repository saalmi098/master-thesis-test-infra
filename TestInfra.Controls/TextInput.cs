using Microsoft.Playwright;
using TestInfra.Controls.Base;

namespace TestInfra.Controls;

/// <summary>
/// A single-line text input.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class TextInput(ILocator locator) : Control(locator)
{
    /// <summary>
    /// Replaces the input's content with <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Text to enter.</param>
    public Task FillAsync(string value) => Locator.FillAsync(value);

    /// <summary>
    /// Returns the input's current value.
    /// </summary>
    public Task<string> ValueAsync() => Locator.InputValueAsync();
}
