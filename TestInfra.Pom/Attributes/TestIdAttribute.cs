using Microsoft.Playwright;

namespace TestInfra.Pom.Attributes;

/// <summary>
/// Locate by test ID (data-testid).
/// </summary>
/// <param name="id">Value of the data-testid attribute.</param>
public sealed class TestIdAttribute(string id) : LocatorAttribute
{
    /// <summary>
    /// Value of the data-testid attribute.
    /// </summary>
    public string Id { get; } = id;

    /// <inheritdoc />
    public override ILocator Resolve(ILocator root) => root.GetByTestId(Id);
}
