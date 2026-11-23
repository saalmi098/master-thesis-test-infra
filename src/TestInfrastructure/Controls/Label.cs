using Microsoft.Playwright;
using TestInfrastructure.Controls.Base;

namespace TestInfrastructure.Controls;

/// <summary>
/// Read-only text element.
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Label(ILocator locator) : Control(locator);