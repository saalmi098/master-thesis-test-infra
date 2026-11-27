using App.E2E.Tests.Pages;
using Microsoft.Playwright;
using System.Text.RegularExpressions;
using TestInfra.Pom.PageObjects;
using Xunit;

namespace App.E2E.Tests.Tests;

public class AddToCartTests : TestBase
{
    [Fact]
    public async Task Searching_and_adding_a_product_updates_the_cart_count()
    {
        var page = Page;
        await page.GotoAsync("/");

        // Product cards load asynchronously; wait for them before creating the page object.
        await page.GetByTestId("catalog-result-count").WaitForAsync();
        var catalog = await PageObjectFactory.CreateAsync<CatalogPage>(page);

        await catalog.Content.Search.FillAsync("notebook");
        await catalog.Content.ResultCount.Expect().ToHaveTextAsync("1 product(s) found");

        await catalog.Content.FieldNotebook.AddToCart.ClickAsync();

        await catalog.Cart.Expect().ToHaveTextAsync(new Regex(@"Cart \(1\)"));
    }
}
