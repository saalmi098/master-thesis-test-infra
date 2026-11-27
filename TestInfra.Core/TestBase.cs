using Microsoft.Playwright;
using Xunit;

namespace TestInfra.Core;

/// <summary>
/// Base class for generated tests. xUnit creates one instance per test, so every test gets its own
/// <see cref="BrowserSession"/> (Playwright, browser, context and page) and tests cannot share state.
/// </summary>
public abstract class TestBase : IAsyncLifetime
{
    private BrowserSession? _session;

    /// <summary>
    /// Page of the current test. Relative URLs resolve against the configured <c>BaseUrl</c>.
    /// </summary>
    protected IPage Page => _session?.Page
        ?? throw new InvalidOperationException("Page is only available after InitializeAsync.");

    /// <summary>
    /// Routes page object logging to the xUnit test output. Must happen in the (synchronous) constructor:
    /// an AsyncLocal set inside an async InitializeAsync would not flow back into the test method.
    /// </summary>
    protected TestBase() =>
        PageObjectLog.SetSink(TestContext.Current.TestOutputHelper!.WriteLine);

    /// <summary>
    /// Starts a fresh browser session before each test. Headed mode is controlled by <c>Headed</c> in appsettings.json.
    /// </summary>
    public async ValueTask InitializeAsync()
        => _session = await BrowserSession.StartAsync(TestSettings.BaseUrl, headless: !TestSettings.Headed);

    /// <summary>
    /// Disposes the browser session after each test.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_session != null) await _session.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
