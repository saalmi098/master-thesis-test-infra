using Microsoft.Playwright;
using TestInfra.Pom.V4.Infrastructure.Locators;
using TestInfra.Pom.V4.Infrastructure.POM;

namespace TestInfra.Pom.V4.POM;

public sealed class ProductDetailsOverviewTab(IPage page, ILocator? baseLocator = null) : PageObject(page, baseLocator)
{
    [Locator(LocatorKind.DataTestId, "product-overview-name")]
    public required ILocator Name { get; init; }

    [Locator(LocatorKind.DataTestId, "product-overview-description")]
    public required ILocator Description { get; init; }

    [Locator(LocatorKind.DataTestId, "product-overview-category")]
    public required ILocator Category { get; init; }

    [Locator(LocatorKind.DataTestId, "product-overview-price")]
    public required ILocator Price { get; init; }
}
