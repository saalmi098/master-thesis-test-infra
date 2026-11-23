using Microsoft.Playwright;
using TestInfra.Controls.Base;

namespace TestInfra.Controls;

/// <summary>
/// Table pager showing e.g. "Rows per page: 10  1-10 of 25".
/// </summary>
/// <param name="locator">Locator of the element this control wraps.</param>
public sealed class Pager(ILocator locator) : Control(locator);
