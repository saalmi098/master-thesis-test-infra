using App.E2E.Tests.Pages;
using TestInfra.Extensions;
using TestInfra.Pom.PageObjects;
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
        await details.Name.Expect().ToHaveTextAsync("Field Notes Notebook");

        var inventory = await details.OpenInventoryTabAsync();

        Assert.True(await details.InventoryTab.IsSelectedAsync());
        Assert.False(await details.OverviewTab.IsSelectedAsync());
        await inventory.Grid.Rows.Expect().ToHaveCountAsync(2);
        Assert.Equal(2, await inventory.Grid.RowCountAsync());
        await inventory.Pager.Expect().ToContainTextAsync("1-2 of 2");
    }
}
