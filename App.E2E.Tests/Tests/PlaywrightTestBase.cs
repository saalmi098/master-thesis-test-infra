using TestInfra.Pom.PageObjects;
using Xunit;

namespace App.E2E.Tests.Tests;

public abstract class PlaywrightTestBase : IAsyncLifetime
{
    protected BrowserSession Session { get; private set; } = null!;

    // Must happen in the (synchronous) constructor: an AsyncLocal set inside an async
    // InitializeAsync would not flow back into the test method.
    protected PlaywrightTestBase() =>
        PageObjectLog.SetSink(TestContext.Current.TestOutputHelper!.WriteLine);

    public async ValueTask InitializeAsync() => Session = await BrowserSession.StartAsync(false);

    public async ValueTask DisposeAsync() => await Session.DisposeAsync();
}
