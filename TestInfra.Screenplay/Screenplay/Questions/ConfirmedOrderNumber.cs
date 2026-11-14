using TestInfra.Screenplay.Abilities;

namespace TestInfra.Screenplay.Questions;

public sealed record ConfirmedOrderNumber : IQuestion<string>
{
    public Task<string> AnsweredByAsync(Actor actor) =>
        actor.Ability<BrowseTheWeb>().Page.GetByTestId("checkout-order-number").InnerTextAsync();
}
