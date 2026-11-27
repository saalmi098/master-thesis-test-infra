using App.E2E.Tests.Pages;
using TestInfra.Pom.PageObjects;
using Xunit;

namespace App.E2E.Tests.Tests;

public class OrdersLinkTests : TestBase
{
    private const string BaseUrl = "http://localhost:5178/";

    [Fact]
    public async Task Orders_link_is_not_present_when_logged_out()
    {
        var page = Page;
        await page.GotoAsync(BaseUrl);
        await page.GetByTestId("catalog-result-count").WaitForAsync();

        var catalog = await PageObjectFactory.CreateAsync<CatalogPage>(page);

        Assert.Null(catalog.Orders);
    }

    [Fact]
    public async Task Orders_link_is_present_when_logged_in()
    {
        var page = Page;
        await page.GotoAsync(BaseUrl + "login");
        await page.GetByTestId("login-email").FillAsync("demo@test.com");
        await page.GetByTestId("login-password").FillAsync("1234");
        await page.GetByTestId("login-submit").ClickAsync();

        // Login state is checked once at creation, so wait for the final (logged-in) nav first.
        await page.GetByTestId("nav-orders-desktop").WaitForAsync();
        await page.GotoAsync(BaseUrl);
        await page.GetByTestId("catalog-result-count").WaitForAsync();

        var catalog = await PageObjectFactory.CreateAsync<CatalogPage>(page);

        Assert.NotNull(catalog.Orders);
        Assert.Equal("ORDERS", (await catalog.Orders.TextAsync()).ToUpperInvariant());
    }
}
