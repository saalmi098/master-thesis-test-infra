using Microsoft.Extensions.Configuration;

namespace TestInfra.Pom.PageObjects;

/// <summary>
/// Test configuration read from <c>appsettings.json</c> next to the test assembly.
/// </summary>
public static class TestSettings
{
    private const string BaseUrlKey = "BaseUrl";
    private const string HeadedKey = "Headed";

    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    /// <summary>
    /// Base URL of the application under test (key <c>BaseUrl</c>).
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if <c>BaseUrl</c> is missing or empty.</exception>
    public static string BaseUrl { get; } = LoadBaseUrl();

    /// <summary>
    /// If true, the browser runs with a visible window (key <c>Headed</c>, default false).
    /// </summary>
    public static bool Headed { get; } = bool.TryParse(Configuration[HeadedKey], out var headed) && headed;

    /// <summary>
    /// Reads <c>BaseUrl</c> from the configuration.
    /// </summary>
    private static string LoadBaseUrl()
    {
        var baseUrl = Configuration[BaseUrlKey];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                $"Missing '{BaseUrlKey}' in appsettings.json (see {AppContext.BaseDirectory}).");
        }

        return baseUrl;
    }
}
