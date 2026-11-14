using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Tasks;

public sealed record OpenInventory : ITask
{
    public Task PerformAsAsync(Actor actor) =>
        actor.Ability<BrowseTheWeb>().Page.GotoAsync("/manage/inventory");
}
