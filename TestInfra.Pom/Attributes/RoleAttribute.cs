using Microsoft.Playwright;

namespace TestInfra.Pom.Attributes;

/// <summary>
/// Locate by ARIA role and optional accessible name (substring match).
/// </summary>
/// <param name="role">ARIA role to match.</param>
public sealed class RoleAttribute(AriaRole role) : LocatorAttribute
{
    /// <summary>
    /// ARIA role to match.
    /// </summary>
    public AriaRole Role { get; } = role;

    /// <summary>
    /// Accessible name filter (substring match); null matches any name.
    /// </summary>
    public string? Name { get; set; }

    /// <inheritdoc />
    public override ILocator Resolve(ILocator root) =>
        root.GetByRole(Role, new LocatorGetByRoleOptions { Name = Name });
}
