using TestInfra.Shared;
using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Questions;

public sealed record OrderIsVisible(string OrderNumber) : IQuestion<bool>
{
    public async Task<bool> AnsweredByAsync(Actor actor) =>
        await actor.Ability<BrowseTheWeb>()
            .Page.GetByTestId($"order-card-{OrderNumber}")
            .IsEventuallyVisibleAsync();
}
