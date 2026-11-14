using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Questions;

public sealed record InventoryIsVisible : IQuestion<bool>
{
    public Task<bool> AnsweredByAsync(Actor actor) =>
        actor.Ability<BrowseTheWeb>().Page.GetByTestId("inventory-page").IsVisibleAsync();
}
