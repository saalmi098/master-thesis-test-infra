using Microsoft.Extensions.Configuration;

namespace TestInfra.Core;

/// <summary>
/// Test configuration read from <c>appsettings.json</c> next to the test assembly.
/// </summary>
public static class TestSettings
{
    private const string BaseUrlKey = "BaseUrl";
    private const string HeadedKey = "Headed";

    private const string FileName = "appsettings.json";

    private static readonly IConfiguration Configuration = LoadConfiguration();

    /// <summary>
    /// Builds the configuration from <c>appsettings.json</c> in the output directory.
    /// </summary>
    /// <exception cref="FileNotFoundException">Thrown if the test project does not provide <c>appsettings.json</c>.</exception>
    private static IConfiguration LoadConfiguration()
    {
        var path = Path.Combine(AppContext.BaseDirectory, FileName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"'{FileName}' not found at '{path}'. Each test project must provide its own {FileName} " +
                $"(with '{BaseUrlKey}' and '{HeadedKey}') and copy it to the output directory",
                path);
        }

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(FileName, optional: false)
            .Build();
    }

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
