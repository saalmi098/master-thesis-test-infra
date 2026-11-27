using Microsoft.Playwright;

namespace TestInfra.Core;

/// <summary>
/// Extension methods for raw Playwright locators.
/// </summary>
public static class LocatorExtensions
{
    /// <summary>
    /// Starts an assertion chain on the locator.
    /// Usage: <c>locator.Expect().ToHaveTextAsync(...)</c>.
    /// </summary>
    /// <param name="locator">Locator to assert on.</param>
    public static ILocatorAssertions Expect(this ILocator locator)
        => Assertions.Expect(locator);
}
