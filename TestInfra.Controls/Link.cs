using Microsoft.Playwright;
using TestInfra.Controls.Base;

namespace TestInfra.Controls;

/// <summary>
/// A hyperlink or link-styled navigation element.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Link(ILocator locator) : Control(locator);
