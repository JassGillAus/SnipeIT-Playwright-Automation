using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using SnipeIT.Playwright.Automation.Pages;

namespace SnipeIT.Playwright.Automation;

/// <summary>
/// This is Main Class that includes all the E2E workflows like AssetCreationWorkflow, License creation workflow etc.
/// </summary>
[TestFixture]
public partial class Tests : PageTest
{

    /// <summary>
    /// Setting up Base URL for Context and reading it from runsettings file
    /// </summary>
    /// <returns></returns>
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {

            BaseURL = TestContext.Parameters.Get("BaseURL"),
        };

    }
    /// <summary>
    /// This is Asset Creation workflow involving Login, asset creation, Asset Search & Detail verification workflow
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task AssetCreationWorkflow()
    {

        #region Declarations
        // Instantiate our TestData Object (which will hold asset test data)
        AssetTestData objTestData = new();
        
        // Instantiate All the Page Objects we need
        LoginPage objLoginPage = new(Page);
        DashboardPage objDashboardPage = new(Page);
        CreateAssetPage objCreateAssetPage = new(Page, objTestData);
        AssetListpage objAssetListPage = new(Page);
        AssetDetailsPage objAssetDetailsPage = new(Page);
        #endregion Declarations

        #region LoginPage
        // Step 1. Lets Load Website and Validate if correct website loads
        await objLoginPage.NavigateToLoginPage();
        if (objLoginPage.myLoginPageTitle.Contains("Asset Management Demo").Equals(false))
        {
            //Assert.Inconclusive("Incorrect Website Loaded. Please Rerun Test after few minutes");
            Assert.Fail("Login Page: Incorrect Website Loaded. Please Rerun Test after few minutes");
        }
        else
        {
            Console.WriteLine("Login Page: Correct Website Loaded with Title: " + objLoginPage.myLoginPageTitle);
        }

        // Step 2. Now Lets Login to application  : Business Requirement 1
        // Not storing password in .env file as they are publically available on login page
        await objLoginPage.PerformLogin("admin", "password");

        // Validate that Login was successfull
        await Expect(Page).ToHaveTitleAsync(new Regex("Dashboard"));
        Console.WriteLine("Login Page: Login Successfull and Dashboard Page Loaded Successfully");

        #endregion LoginPage

        #region Asset Creation
        // Step 3: Navigate to Asset Creation Page : Business Requirement 2
        await objDashboardPage.NavigateTo_CreateAssetPage();

        // Validate that Asset Creation Page loaded
        await Expect(Page).ToHaveTitleAsync(new Regex("Create Asset"));
        Console.WriteLine("CreateAsset: Create Asset Page Loaded successfully");

        // Step 4: Fill Asset Creation Form and Create Asset : Business Requirement 3

        objTestData = await objCreateAssetPage.Fill_AssetCreationForm();
        await objCreateAssetPage.CreateAsset();

        // Validate that Asset creation was success and we moved to List of All Assets
        await Expect(Page).ToHaveTitleAsync(new Regex("All Assets"));
        await Expect(Page.GetByText("Success")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Asset with tag")).ToBeVisibleAsync();

        // Step 5. Navigate to Asset List Page and Search for our Newly created Asset to confirm Asset created
        ILocator AssetSearch = await objAssetListPage.SearchAsset_ByTag(objTestData.AssetTag);
        await Expect(AssetSearch).ToBeVisibleAsync();
        var NoOfMatchingAssets = await AssetSearch.CountAsync();
        if (NoOfMatchingAssets > 0)
        {
            Console.WriteLine("Asset List: Asset(s) Found in Search : " + NoOfMatchingAssets);
        }
        else
        {
            Console.WriteLine("Asset List: Did NOT Find Newly Created Asset");
        }
        #endregion Asset creation

        #region Asset Details Verification
        // Step 6: Navigate to Asset Details page and verify Asset details are correct
        await objAssetListPage.Navigateto_AssetDetails(objTestData.AssetTag);

        // Validate that Asset Details Page loaded
        await Expect(Page).ToHaveTitleAsync(new Regex("View Asset"));
        Console.WriteLine("Detail Page: View Asset Details Page Loaded successfully");


        // Step 7: Verify that Asset Details Match Correctly
        // Check Asset Tag
        ILocator AssetTagLocator = await objAssetDetailsPage.Verify_AssetTagMatch(objTestData.AssetTag);
        await Expect(AssetTagLocator).ToBeVisibleAsync();
        Console.WriteLine("Detail Page: Asset Tag Found and Matched");

        // Check Assigned User
        ILocator AssignedUserLocator = await objAssetDetailsPage.Verify_AssignedUserMatch(objTestData.AssignedUser);
        await Expect(AssignedUserLocator).ToBeVisibleAsync();
        Console.WriteLine("Detail Page: Assigned User Found and Matched");

        // Check Asset Status
        ILocator AssetStatusLocator = await objAssetDetailsPage.Verify_AssetStatusMatch(objTestData.AssetStatus);
        await Expect(AssetStatusLocator).ToBeAttachedAsync();
        Console.WriteLine("Detail Page: Asset Status Found & Matched");

        // Check Asset Model 
        ILocator AssetModelLocator = await objAssetDetailsPage.Verify_AssetModelMatch(objTestData.Model);
        await Expect(AssetModelLocator).ToBeAttachedAsync();
        Console.WriteLine("Detail Page: Asset Model Found & Matched");

        
        // Step 8:Navigate to History Tab, Validate that History records asset creation and Checkout
        await objAssetDetailsPage.NAvigateto_HistoryTab();
        ILocator ActivityRecordLocator = await objAssetDetailsPage.VerifyAssetDetails_onHistoryTab("create new");
        await Expect(ActivityRecordLocator).ToBeVisibleAsync();
        Console.WriteLine("Detail History Page : Create Record Entry Found");

        ActivityRecordLocator = await objAssetDetailsPage.VerifyAssetDetails_onHistoryTab("checkout");
        await Expect(ActivityRecordLocator).ToBeVisibleAsync();
        Console.WriteLine("Detail History Page : Checkout Record Entry Found");



        #endregion Asset Details Verification
        Console.WriteLine("End 2 End : Asset Creation WorkFlow Completed");

    }

}
