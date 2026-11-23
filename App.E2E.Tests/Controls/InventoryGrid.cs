using Microsoft.Playwright;
using TestInfra.Controls.Base;

namespace App.E2E.Tests.Controls;

/// <summary>Inventory table; rows are identified by their "product-inventory-row-N" test IDs.</summary>
public sealed class InventoryGrid(ILocator locator) : Control(locator)
{
    public ILocator Rows => Locator.Locator("[data-testid^='product-inventory-row-']");

    public Task<int> RowCountAsync() => Rows.CountAsync();
}
