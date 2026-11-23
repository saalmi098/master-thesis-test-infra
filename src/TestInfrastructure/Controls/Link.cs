using Microsoft.Playwright;
using TestInfrastructure.Controls.Base;

namespace TestInfrastructure.Controls;

/// <summary>
/// A hyperlink or link-styled navigation element.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Link(ILocator locator) : Control(locator);
