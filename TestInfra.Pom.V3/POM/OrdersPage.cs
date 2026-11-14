using TestInfra.Shared;
using Microsoft.Playwright;
using TestInfra.Pom.V3.Infrastructure;

namespace TestInfra.Pom.V3.POM;

public sealed class OrdersPage : PageComponent, IPageComponent<OrdersPage>
{
    public Required<ILocator> Root { get; }

    private OrdersPage(IPage page)
    {
        Root = Require(page.GetByTestId("orders-page"), $"{nameof(OrdersPage)}.{nameof(Root)}");
    }

    static OrdersPage IPageComponent<OrdersPage>.CreateInstance(IPage page, ILocator? baseLocator) => new(page);

    public async Task<bool> ContainsOrder(string orderNumber) =>
        await Root.Value.GetByTestId($"order-card-{orderNumber}").IsEventuallyVisibleAsync();
}
