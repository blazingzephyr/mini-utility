
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace MiniUtility;

[Activity(Label = "Mini-Utility", MainLauncher = true)]
internal class MainActivity : FragmentActivity
{
    public PreferencesManager? Prefs { get; set; }
    public SheetsService? Service { get; set; }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        var tabLayout = FindViewById<TabLayout>(Resource.Id.tabLayout);
        var viewPager = FindViewById<ViewPager2>(Resource.Id.viewPager);

        if (tabLayout is null || viewPager is null) return;

        Prefs = new PreferencesManager(this);
        SetSheetsService(true);

        viewPager.Adapter = new UtilityPagerAdapter(this);

        var config = new TabConfigurationStrategy(this);
        new TabLayoutMediator(tabLayout, viewPager, config).Attach();
    }

    public int SetSheetsService(bool initialLaunch = false)
    {
        if (Prefs is null) return 0;
        if (string.IsNullOrEmpty(Prefs.AccountSettings)) return 0;

        bool accountChanged = Prefs.CurrentAccountSettings != Prefs.AccountSettings;
        bool nameChanged = Prefs.CurrentAppName != Prefs.AppName;
        if (!initialLaunch && !accountChanged && !nameChanged) return 0;

        try
        {
            var credential = GoogleCredential
                    .FromJson(Prefs.AccountSettings)
                    .CreateScoped(SheetsService.Scope.Spreadsheets);

            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Prefs.AppName
            });

            this.CreateToast(Resource.String.launched_api);
            return 1;
        }
        catch (Exception ex)
        {
            Service = null;
            this.CreateToast(ex.Message);
            return -1;
        }
    }
}
