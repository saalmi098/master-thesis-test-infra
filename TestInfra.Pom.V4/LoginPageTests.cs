using TestInfra.Pom.V4.POM;
using TestInfra.Shared;
using Xunit;

namespace TestInfra.Pom.V4;

public sealed class LoginPageTests : ShopDemoPageTestV4<LoginPage>
{
    protected override string PageUrl => "/login";

    [Fact]
    public async Task Login_page_controls_are_bound_and_ready()
    {
        Assert.True(await Model.IsReady()); // TODO: why need to call IsReady() here? Shouldn't the page object be ready after CreatePageObject()?
        await Model.Email.Fill(E2ETestSettings.Shopper.Email);
        Assert.Equal(E2ETestSettings.Shopper.Email, await Model.Email.GetValue());
        await Model.Password.Expect().ToBeVisibleAsync();
        await Model.Submit.Expect().ToContainTextAsync("Log in", new() { IgnoreCase = true });
    }
}
