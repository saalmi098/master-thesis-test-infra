using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Tasks;

public sealed record AddProductToCart(string ProductId) : ITask
{
    public async Task PerformAsAsync(Actor actor)
    {
        var page = actor.Ability<BrowseTheWeb>().Page;
        await page.GetByTestId("nav-catalog-desktop").ClickAsync();
        await page.GetByTestId($"product-card-{ProductId}")
            .GetByTestId($"product-add-{ProductId}")
            .ClickAsync();
    }
}
