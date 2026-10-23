using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace SnipeIT.Playwright.Automation.Pages;


/// <summary>
/// This is Create Asset Page Object Class to deal with locators and Actions for creating assets
/// </summary>
public class CreateAssetPage
{

    // Variables
    private readonly IPage _page;
    private AssetTestData _TestDataObject;

    // Locators
    private ILocator Textbox_Model => _page.Locator("#select2-model_select_id-container");
    private ILocator Textbox_Status => _page.Locator("#select2-status_select_id-container");
    private ILocator Textbox_User => _page.Locator("#select2-assigned_user_select-container");
    private ILocator Btn_SubmitAsset => _page.Locator("#submit_button");

    // Constructor
    public CreateAssetPage(IPage page, AssetTestData obj)
    {
        _page = page;
        _TestDataObject = obj;
    }
    // Methods



    /// <summary>
    /// This Method Fills all the fields on the Asset Creation form. It calls other indiviual methods
    /// It then Returns the filled Asset Test Data Object which hold the data we are using to create ASset
    /// we will Use this for comparision later
    /// </summary>
    /// <returns> AssetTestData Object</returns>
    public async Task<AssetTestData> Fill_AssetCreationForm()
    {
        await this.SelectCompanyName();
        await this.SelectModelType();
        await this.SelectAssetStatus();
        await this.SelectAndAssignUser();
        await this.SelectAssetDetails();
        return _TestDataObject;
    }



    /// <summary>
    /// Fills Company Name from the List by Selecting First item
    /// </summary>
    /// <returns></returns>
    public async Task SelectCompanyName()
    {
        // 1. Find and Click Combo Box 
        ILocator eSelectCompany = _page.GetByRole(AriaRole.Combobox, new() { Name = "Select company" });
        await eSelectCompany.ClickAsync();

        // 2. Select2 Options List & Select 1st Value
        await _page.Locator(".select2-results__option").WaitForAsync();
        IReadOnlyList<ILocator> FinalList = await _page.Locator(".select2-results__option").AllAsync();
        if (FinalList.Count > 0)
        {
            await FinalList[0].ClickAsync();
        }

        // 3. Read and Store that Value into Test Data Object
        ILocator textboxAgain = _page.Locator("#select2-company_id_select-container");
        await textboxAgain.WaitForAsync();

        var companyName = await textboxAgain.TextContentAsync();
        if (string.IsNullOrWhiteSpace(companyName))
        { _TestDataObject.CompanyName = string.Empty; }
        else
        {
            // Value comes with an x, so we are using substring to remove it
            string xcompanyName = companyName.Substring(1);
            _TestDataObject.CompanyName = xcompanyName;
        }
        Console.WriteLine("CreateAsset: Test Company Name used for asset is - " + _TestDataObject.CompanyName);
    }


    /// <summary>
    /// Fills the Model Type Macbook Pro 13 from the drop down list
    /// </summary>
    /// <returns></returns>
    public async Task SelectModelType()
    {

        await Textbox_Model.ClickAsync();

        var Model_we_need_toSelect = new Regex(@"Macbook Pro 13", RegexOptions.IgnoreCase);
        
        ILocator Model = _page.Locator(".select2-results__option", new() { HasTextRegex = Model_we_need_toSelect});
        await Model.WaitForAsync();
        await Model.ClickAsync();

        var ModelTypeReturned = await Textbox_Model.TextContentAsync();

        if (string.IsNullOrWhiteSpace(ModelTypeReturned))
        { _TestDataObject.Model = string.Empty; }
        else
        {
            // remove the x from the string
            string xModelTypeReturned = ModelTypeReturned.Substring(1);
            _TestDataObject.Model = xModelTypeReturned;
        }

        Console.WriteLine("CreateAsset: Test Model Type used is - " + _TestDataObject.Model);
    }

    /// <summary>
    /// This Method fills the Asset Status = "Ready to Deploy" from the dropdown
    /// </summary>
    /// <returns></returns>
    public async Task SelectAssetStatus()
    {
        await Textbox_Status.ClickAsync();

        string Status_we_need_toSelect = "Ready to Deploy";
        ILocator StatusList = _page.Locator(".select2-results__option", new() { HasText = Status_we_need_toSelect });
        await StatusList.WaitForAsync();
        await StatusList.ClickAsync();


        var StatusReturned = await Textbox_Status.TextContentAsync();

        if (string.IsNullOrWhiteSpace(StatusReturned))
        { _TestDataObject.AssetStatus = string.Empty; }
        else { _TestDataObject.AssetStatus = StatusReturned; }

        Console.WriteLine("CreateAsset: Test Asset Status assigned is: " + StatusReturned);
    }

    /// <summary>
    /// This Method is used to assign a random user for Checkout (1st User from the List)
    /// </summary>
    /// <returns></returns>
    public async Task SelectAndAssignUser()
    {
        await Textbox_User.ClickAsync();

        // 2. Select2 Options List & Select 1st Value
        await _page.Locator(".select2-results__option").WaitForAsync();
        IReadOnlyList<ILocator> UserList = await _page.Locator(".select2-results__option").AllAsync();
        if (UserList.Count > 0)
        {
            await UserList[0].ClickAsync();
        }

        await Textbox_User.WaitForAsync();
        var UserReturned = await Textbox_User.TextContentAsync();

        if (string.IsNullOrWhiteSpace(UserReturned))
        { _TestDataObject.AssignedUser = string.Empty; }
        else
        {
            // 1. Find the index position of the '(' character
            int openParenIndex = UserReturned.IndexOf('(');

            // 2. Extract from index 1 up to the position of '('
            // Length formula: (index of '(') - (start index) -> openParenIndex - 1
            string xUserReturned = UserReturned.Substring(1, openParenIndex - 1);

            // assign it to test data Object
            _TestDataObject.AssignedUser = xUserReturned;
        }
        Console.WriteLine("CreateAsset: Test User Name owing the asset is - " + _TestDataObject.AssignedUser);
    }

    /// <summary>
    /// This Method Grabs the Asset Tag (auto-generated) by the application and Stores in Test Data Object
    /// </summary>
    /// <returns></returns>
    public async Task SelectAssetDetails()
    {
        // Grab the Asset Tag
        ILocator AssetTagLocator = _page.GetByRole(AriaRole.Textbox, new() { Name = "Asset Tag", Exact = true });

        var AssetTagValue = await AssetTagLocator.InputValueAsync();

        if (string.IsNullOrWhiteSpace(AssetTagValue))
        { _TestDataObject.AssetTag = string.Empty; }
        else { _TestDataObject.AssetTag = AssetTagValue; }

        Console.WriteLine("CreateAsset: Asset Tag generated is :" + AssetTagValue);
    }

    /// <summary>
    /// This Method create the Actual Asset by Submitting the form
    /// You MUST fill Data in the form before calling this method.
    /// </summary>
    /// <returns></returns>
    public async Task CreateAsset()
    {
        await Btn_SubmitAsset.ClickAsync();
        Console.WriteLine("CreateAsset: Asset Creation Form has been Submittted");
    }
}
