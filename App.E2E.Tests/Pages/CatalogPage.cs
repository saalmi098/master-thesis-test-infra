using Microsoft.Playwright;
using TestInfra.Pom.Attributes;
using TestInfra.Controls;
using TestInfra.Pom.PageObjects;

namespace App.E2E.Tests.Pages;

public sealed class CatalogPage(IPage page) : PageObject(page)
{
    [TestId("nav-cart-desktop")]
    public required Link Cart { get; init; }

    // Only visible when logged in.
    [TestId("nav-orders-desktop", Optional = true)]
    public Link? Orders { get; init; }

    [TestId("catalog-page")]
    public required CatalogContent Content { get; init; }
}

public sealed class CatalogContent(ILocator root) : PageObject(root)
{
    [TestId("catalog-search")]
    public required TextInput Search { get; init; }

    [TestId("catalog-result-count")]
    public required Label ResultCount { get; init; }

    [TestId("product-card-field-notebook")]
    public required ProductCard FieldNotebook { get; init; }
}

public sealed class ProductCard(ILocator root) : PageObject(root)
{
    [TestId("product-price-field-notebook")]
    public required Label Price { get; init; }

    [TestId("product-add-field-notebook")]
    public required Button AddToCart { get; init; }
}
