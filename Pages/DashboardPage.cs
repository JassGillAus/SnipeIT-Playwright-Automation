
using Microsoft.Playwright;

namespace SnipeIT.Playwright.Automation.Pages;


/// <summary>
/// This is Dashboard Page Object Class to deal with dashboard page locators and actions
/// </summary>
public class DashboardPage
{
    // Declare variables
    private readonly IPage _page;

    // Declare Locators
    private ILocator eMenuList => _page.GetByRole(AriaRole.Link, new() { Name = "Create New", IncludeHidden = true });
    private ILocator eChildListItem => _page.GetByText("Asset", new() { Exact = true });


    // COnstructor
    public DashboardPage(IPage page)
    {
        _page = page;
    }


    // Methods
    /// <summary>
    /// This method is used to Navigate to Create Asset Page
    /// </summary>
    /// <returns></returns>
    public async Task NavigateTo_CreateAssetPage()
    {
        // find and click on the Menu controls to navigate to Create asset Page 
        await eMenuList.ClickAsync();
        await eChildListItem.ClickAsync();
        Console.WriteLine("Dashboard: Navigating to Create Asset Page");
    }


}