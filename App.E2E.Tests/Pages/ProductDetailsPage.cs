using App.E2E.Tests.Controls;
using Microsoft.Playwright;
using TestInfra.Pom.Attributes;
using TestInfra.Controls;
using TestInfra.Pom.PageObjects;

namespace App.E2E.Tests.Pages;

public sealed class ProductDetailsPage(IPage page) : PageObject(page)
{
    [TestId("product-overview-tab")]
    public required Tab OverviewTab { get; init; }

    [TestId("product-inventory-tab")]
    public required Tab InventoryTab { get; init; }

    [TestId("product-overview-name")]
    public required Label Name { get; init; }

    /// <summary>
    /// The inventory panel is only rendered after its tab is activated, so it is not a
    /// property of this page; it is created on demand (and waits for the slow grid).
    /// </summary>
    public async Task<InventoryTab> OpenInventoryTabAsync()
    {
        await InventoryTab.SelectAsync();
        return await PageObjectFactory.CreateAsync<InventoryTab>(Root.GetByTestId("product-inventory-panel"));
    }
}

public sealed class InventoryTab(ILocator root) : PageObject(root)
{
    [TestId("product-inventory-grid")]
    public required InventoryGrid Grid { get; init; }

    [TestId("product-inventory-pager")]
    public required Pager Pager { get; init; }
}
