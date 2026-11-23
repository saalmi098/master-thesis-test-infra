using Microsoft.Playwright;

namespace TestInfrastructure.PageObjects;

/// <summary>
/// Owns Playwright, browser and page for one test.
/// </summary>
public sealed class BrowserSession : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    /// <summary>
    /// The single page opened for this session.
    /// </summary>
    public IPage Page { get; private set; } = null!;

    /// <summary>
    /// Launches Chromium and opens a new page. Dispose the session to close the browser.
    /// </summary>
    /// <param name="headless">If true, run the browser without a visible window.</param>
    public static async Task<BrowserSession> StartAsync(bool headless = true)
    {
        var session = new BrowserSession
        {
            _playwright = await Playwright.CreateAsync(),
        };

        session._browser = await session._playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });

        session.Page = await session._browser.NewPageAsync();
        return session;
    }

    /// <summary>
    /// Closes the browser and releases Playwright.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
