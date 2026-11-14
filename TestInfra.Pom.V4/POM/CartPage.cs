using Microsoft.Playwright;
using TestInfra.Pom.V4.Infrastructure.Controls;
using TestInfra.Pom.V4.Infrastructure.Locators;
using TestInfra.Pom.V4.Infrastructure.POM;

namespace TestInfra.Pom.V4.POM;

public sealed class CartPage(IPage page, ILocator? baseLocator = null) : PageObject(page, baseLocator)
{
    [Locator(LocatorKind.DataTestId, "cart-page")]
    public required ILocator Root { get; init; }

    // Not shown to a store manager (who cannot check out), so it must not block readiness.
    [Locator(LocatorKind.DataTestId, "cart-checkout", Optional = true)]
    public required Button Checkout { get; init; }

    public async Task<CheckoutPage> ProceedToCheckout()
    {
        await Checkout.Click();
        return await CreatePageObject<CheckoutPage>();
    }
}
