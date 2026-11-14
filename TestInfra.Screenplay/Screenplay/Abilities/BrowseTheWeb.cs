using Microsoft.Playwright;

namespace TestInfra.Screenplay.Abilities;

public sealed class BrowseTheWeb(IPage page) : IAbility
{
    public IPage Page { get; } = page;
}
