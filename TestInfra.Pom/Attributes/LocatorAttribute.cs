using Microsoft.Playwright;

namespace TestInfra.Pom.Attributes;

/// <summary>
/// Base type for locator attributes. 
/// Stores locator instructions, resolves them against a scope.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class LocatorAttribute : Attribute
{
    /// <summary>
    /// Element may be absent. It is not waited for: the property
    /// is set if the element is visible, otherwise null.
    /// Declare such properties nullable and without 'required'.
    /// </summary>
    public bool Optional { get; init; }

    /// <summary>
    /// Builds the Playwright locator for this attribute, scoped to <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Scope to search in.</param>
    public abstract ILocator Resolve(ILocator root);
}
