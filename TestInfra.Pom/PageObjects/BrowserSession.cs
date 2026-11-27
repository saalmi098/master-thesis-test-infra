using Microsoft.Playwright;

namespace TestInfra.Pom.PageObjects;

/// <summary>
/// Owns Playwright, browser, browser context and page for one test.
/// </summary>
public sealed class BrowserSession : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;

    /// <summary>
    /// The single page opened for this session. Relative URLs resolve against the base URL.
    /// </summary>
    public IPage Page { get; private set; } = null!;

    /// <summary>
    /// Launches Chromium, creates a fresh browser context bound to <paramref name="baseUrl"/> and opens a new
    /// page in it. Dispose the session to close context and browser.
    /// </summary>
    /// <param name="baseUrl">Base URL that relative navigation (e.g. <c>GotoAsync("/login")</c>) resolves against.</param>
    /// <param name="headless">If true, run the browser without a visible window.</param>
    public static async Task<BrowserSession> StartAsync(string baseUrl, bool headless = true)
    {
        var session = new BrowserSession
        {
            _playwright = await Playwright.CreateAsync(),
        };

        session._browser = await session._playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });

        session._context = await session._browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = baseUrl
        });

        session.Page = await session._context.NewPageAsync();
        return session;
    }

    /// <summary>
    /// Closes the context and browser and releases Playwright.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_context != null) await _context.CloseAsync();
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
