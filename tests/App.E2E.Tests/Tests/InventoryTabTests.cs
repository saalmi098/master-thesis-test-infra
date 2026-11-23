using App.E2E.Tests.Pages;
using Microsoft.Playwright;
using TestInfrastructure.PageObjects;
using Xunit;

namespace App.E2E.Tests.Tests;

public class InventoryTabTests : PlaywrightTestBase
{
    [Fact]
    public async Task Inventory_tab_shows_grid_after_it_loaded()
    {
        var page = Session.Page;
        await page.GotoAsync("http://localhost:5178/products/field-notebook");

        var details = await PageObjectFactory.CreateAsync<ProductDetailsPage>(page);
        await Assertions.Expect(details.Name.Locator).ToHaveTextAsync("Field Notes Notebook");

        var inventory = await details.OpenInventoryTabAsync();

        Assert.True(await details.InventoryTab.IsSelectedAsync());
        Assert.False(await details.OverviewTab.IsSelectedAsync());
        await Assertions.Expect(inventory.Grid.Rows).ToHaveCountAsync(10);
        Assert.Equal(10, await inventory.Grid.RowCountAsync());
        await Assertions.Expect(inventory.Pager.Locator).ToContainTextAsync("1-10 of 25");
    }
}
