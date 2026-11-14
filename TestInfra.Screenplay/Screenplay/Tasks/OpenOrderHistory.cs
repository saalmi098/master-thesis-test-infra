using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Tasks;

public sealed record OpenOrderHistory : ITask
{
    public Task PerformAsAsync(Actor actor) =>
        actor.Ability<BrowseTheWeb>().Page.GetByTestId("nav-orders-desktop").ClickAsync();
}
