using Microsoft.Playwright;
using TestInfra.Pom.V2.Infrastructure.Locators;
using TestInfra.Pom.V2.Infrastructure.POM;

namespace TestInfra.Pom.V2.POM;

public sealed class InventoryPage(IPage page, ILocator? baseLocator = null) : PageObject(page, baseLocator)
{
    [DataTestId("inventory-page")]
    public ILocator Root { get; private set; } = null!;

    public async Task UpdateStock(string productId, int stock)
    {
        var item = Root.GetByTestId($"inventory-item-{productId}");
        await item.GetByTestId($"inventory-stock-{productId}").Locator("input").FillAsync(stock.ToString());
        await item.GetByTestId($"inventory-save-{productId}").ClickAsync();
        await item.GetByTestId($"inventory-current-stock-{productId}")
            .WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }
}
