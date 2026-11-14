using Microsoft.Playwright;
using TestInfra.Pom.V2.Infrastructure.Locators;
using TestInfra.Pom.V2.Infrastructure.POM;
using TestInfra.Shared;

namespace TestInfra.Pom.V2.POM;

public sealed class LoginPage(IPage page, ILocator? baseLocator = null) : PageObject(page, baseLocator)
{
    [DataTestId("login-form")]
    public ILocator Form { get; private set; } = null!;

    [DataTestId("login-email")]
    public ILocator Email { get; private set; } = null!;

    [DataTestId("login-password")]
    public ILocator Password { get; private set; } = null!;

    [DataTestId("login-submit")]
    public ILocator Submit { get; private set; } = null!;

    public async Task<ShopNavigation> SignIn(DemoAccount account)
    {
        await Email.FillAsync(account.Email);
        await Password.FillAsync(account.Password);
        await Submit.ClickAsync();

        var logoutLink = Page.GetByTestId("nav-logout-desktop");
        await logoutLink.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        return await Create<ShopNavigation>();
    }
}
