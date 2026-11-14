using Microsoft.Playwright;

namespace TestInfra.Pom.V1.Pages;

public sealed class CartPage(IPage page)
{
    public async Task ProceedToCheckoutAsync()
    {
        await page.GetByTestId("nav-cart-desktop").ClickAsync();
        await page.GetByTestId("cart-checkout").ClickAsync();
    }
}
