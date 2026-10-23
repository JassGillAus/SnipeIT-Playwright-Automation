using Microsoft.Playwright;
using Newtonsoft.Json;

namespace SnipeIT.Playwright.Automation.Pages;


/// <summary>
/// This is a ASset List Page Object Class to deal with Asset list Page locators and Actions
/// </summary>
public class AssetListpage
{
    // Declare Variables
    private readonly IPage _page;

    public AssetListpage(IPage page)
    {
        _page = page;
    }

    // Declare Locators
    private ILocator SearchBox => _page.GetByRole(AriaRole.Searchbox, new() { Name = "Search" });

    
    
    /// <summary>
    /// This Method Searches the Newly Created Asset, find from the table
    /// </summary>
    /// <param name="assetTag"></param>
    /// <returns></returns>
    public async Task<ILocator> SearchAsset_ByTag(string assetTag)
    {

        await SearchBox.PressSequentiallyAsync(assetTag);
        await SearchBox.PressAsync("Enter");

        ILocator AssetSearch = _page.GetByText(assetTag, new() { Exact = true });
        await AssetSearch.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        return AssetSearch;
        // var CheckIfAssetFound = await AssetSearch.CountAsync();
        // if(CheckIfAssetFound >0)
        // {
        //     Console.WriteLine("Asset List: Asset(s) Found in Search : "+ CheckIfAssetFound);
        // }
        // else
        // {
        //     Console.WriteLine("Asset List: Did NOT Find Newly Created Asset");
        // }
        
    }

    /// <summary>
    /// This Method Navigates to the Asset Detail Page
    /// </summary>
    /// <param name="objAssetTag"></param>
    /// <returns></returns>
    public async Task Navigateto_AssetDetails(string objAssetTag)
    {
      ILocator AssetSearch = _page.GetByText(objAssetTag, new() { Exact = true });
      await AssetSearch.ClickAsync();
    }

}