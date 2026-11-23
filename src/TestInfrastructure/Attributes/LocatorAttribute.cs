using Microsoft.Playwright;

namespace TestInfrastructure.Attributes;

/// <summary>Base type: stores locator instructions, resolves them against a scope.</summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class LocatorAttribute : Attribute
{
    /// <summary>
    /// Element may be absent (e.g. only visible when logged in). It is not waited for: the property
    /// is set if the element is visible right after the required elements are ready, otherwise null.
    /// Declare the property nullable and without 'required'.
    /// </summary>
    public bool Optional { get; init; }

    public abstract ILocator Resolve(ILocator root);
}
