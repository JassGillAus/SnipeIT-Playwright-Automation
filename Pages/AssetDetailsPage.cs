
using Microsoft.Playwright;

namespace SnipeIT.Playwright.Automation.Pages;


/// <summary>
/// This is Asset Details Page object Class that is used to handle all locators and actions on this page
/// </summary>
public class AssetDetailsPage
{
    private readonly IPage _page;
    private ILocator HistoryTab => _page.GetByRole(AriaRole.Link, new() { Name = "History", IncludeHidden = true });

    public AssetDetailsPage(IPage page)
    {
        _page = page;
    }

    /// <summary>
    /// Method that search for Asset Tag on the Details Page and returns found locator
    /// </summary>
    /// <param name="assetTag"></param>
    /// <returns></returns>
    public async Task<ILocator> Verify_AssetTagMatch(string assetTag)
    {
        ILocator searchAssetTag = _page.GetByText(assetTag, new() { Exact = true });
        return searchAssetTag;
    }

    /// <summary>
    /// Method that search for Assigned User on the Details Page and returns found locator
    /// </summary>
    /// <param name="assignedUser"></param>
    /// <returns></returns>
    public async Task<ILocator> Verify_AssignedUserMatch(string assignedUser)
    {
        ILocator searchAssetUser = _page.GetByRole(AriaRole.Link, new() { Name = assignedUser, IncludeHidden = true });
        return searchAssetUser;
    }

    /// <summary>
    /// Method that search for Asset Status on the Details Page and returns found locator
    /// </summary>
    /// <param name="assetStatus"></param>
    /// <returns></returns>
    public async Task<ILocator> Verify_AssetStatusMatch(string assetStatus)
    {
        ILocator searchAssetStatus = _page.GetByRole(AriaRole.Link, new() { Name = assetStatus, IncludeHidden = true });
        return searchAssetStatus;
    }

    /// <summary>
    /// Method that search for Asset Model on the Details Page and returns found locator
    /// </summary>
    /// <param name="assetModel"></param>
    /// <returns></returns>
    public async Task<ILocator> Verify_AssetModelMatch(string assetModel)
    {
        ILocator searchAssetModel = _page.GetByRole(AriaRole.Link, new() { Name = "Macbook Pro 13\"", Exact = true });
        return searchAssetModel;
    }


    /// <summary>
    /// This Method Navigates to the History Tab 
    /// </summary>
    public async Task NAvigateto_HistoryTab()
    {
        await HistoryTab.ClickAsync();
    }

    /// <summary>
    /// This Method Verifies that History tab has recorded Asset creation and Asset Checkout Activiteis
    /// </summary>
    /// <param name="typeOfActivityCheckRequest"></param>
    /// <returns></returns>
    public async Task<ILocator> VerifyAssetDetails_onHistoryTab(string typeOfActivityCheckRequest)
    {
        // we should have 2 records - 1 "create new" and 1 "checkout"
        ILocator createRecord = _page.GetByText(typeOfActivityCheckRequest, new() { Exact = true });
        await createRecord.WaitForAsync();
        return createRecord;
    }

}