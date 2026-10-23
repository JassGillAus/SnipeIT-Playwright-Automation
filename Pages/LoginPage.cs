using Microsoft.Playwright;


namespace SnipeIT.Playwright.Automation.Pages;


/// <summary>
/// This is Login Page Object Class to deal with login page locators and Actions
/// </summary>
public class LoginPage
{
    // Declare Variables
    private readonly IPage _page;


    // Contructor
    public LoginPage(IPage page)
    {
        _page = page;
    }

    // Declare Page Locators
    private ILocator eUsername => _page.GetByRole(AriaRole.Textbox, new() { Name = "username" });
    private ILocator ePassword => _page.GetByRole(AriaRole.Textbox, new() { Name = "password" });
    private ILocator eLoginButton => _page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public string myLoginPageTitle = string.Empty;


    // Methods
    /// <summary>
    /// This Method Loads the Main Login Page for Website
    /// </summary>
    /// <returns></returns>
    public async Task NavigateToLoginPage()
    {
        await _page.GotoAsync("/login");
        myLoginPageTitle = await _page.TitleAsync();
    }

    /// <summary>
    /// This Method Performs the Actual Login
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task PerformLogin(string username, string password)
    {
        await eUsername.FillAsync(username);
        await ePassword.FillAsync(password);
        await eLoginButton.ClickAsync();
    }
}