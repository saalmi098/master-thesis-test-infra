using TestInfra.Shared;
using Microsoft.Playwright;
using TestInfra.Pom.V2.Infrastructure.Locators;
using TestInfra.Pom.V2.Infrastructure.POM;

namespace TestInfra.Pom.V2.POM;

public sealed class OrdersPage(IPage page, ILocator? baseLocator = null) : PageObject(page, baseLocator)
{
    [DataTestId("orders-page")]
    public ILocator Root { get; private set; } = null!;

    public async Task<bool> ContainsOrder(string orderNumber) =>
        await Root.GetByTestId($"order-card-{orderNumber}").IsEventuallyVisibleAsync();
}
