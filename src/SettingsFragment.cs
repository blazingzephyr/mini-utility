
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class SettingsFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.settings_tab, container, false);
        if (view is null) return new View(Context);
        if (RequireActivity() is not MainActivity activity) return view;

        var account = view.FindViewById<EditText>(Resource.Id.account_settings);
        var appName = view.FindViewById<EditText>(Resource.Id.google_app_name);
        var finances_sheet = view.FindViewById<LinearLayout>(Resource.Id.finances_sheet);
        var autoIncrement = view.FindViewById<CheckBox>(Resource.Id.auto_increment);
        var acceptedSheets = view.FindViewById<RecyclerView>(Resource.Id.accepted_sheets)!;
        var addSheet = view.FindViewById<Button>(Resource.Id.add_sheet);
        var update = view.FindViewById<Button>(Resource.Id.update_button);

        if (account is null || appName is null || finances_sheet is null || acceptedSheets is null ||
            update is null || autoIncrement is null || addSheet is null || activity.Prefs is null) return view;

        var sheet_id = finances_sheet.FindViewById<EditText>(Resource.Id.sheets_id);
        var range = finances_sheet.FindViewById<EditText>(Resource.Id.range);
        if (sheet_id is null || range is null) return view;

        account.Text = activity.Prefs.AccountSettings;
        account.TextChanged += (o, s) => activity.Prefs.AccountSettings = s.Text?.ToString();

        appName.Text = activity.Prefs.AppName;
        appName.TextChanged += (o, s) => activity.Prefs.AppName = s.Text?.ToString();

        sheet_id.Text = activity.Prefs.SheetId;
        sheet_id.TextChanged += (o, s) => activity.Prefs.SheetId = s.Text?.ToString();

        range.Text = activity.Prefs.Range;
        range.TextChanged += (o, s) => activity.Prefs.Range = s.Text?.ToString();

        autoIncrement.Checked = activity.Prefs.AutoIncrement;
        autoIncrement.CheckedChange += (o, s) => activity.Prefs.AutoIncrement = s.IsChecked;

        NestedRecyclerAdapter RefreshAdapter()
        {
            var adapter = new NestedRecyclerAdapter(Resource.Layout.nested_item, activity.Prefs!.ScoreSheets);
            adapter.TextChanged += (o, s) =>
            {
                activity.Prefs.ScoreSheets[s.Position1][s.Position2] = s.Text!.ToString()!;
            };

            return adapter;
        }

        acceptedSheets.SetLayoutManager(new LinearLayoutManager(Context));
        acceptedSheets.SetAdapter(RefreshAdapter());

        addSheet.Click += (sender, e) =>
        {
            activity.Prefs.ScoreSheets.Add([string.Empty, string.Empty]);
            acceptedSheets.SetAdapter(RefreshAdapter());
        };

        update.Click += (sender, e) =>
        {
            int result = activity.SetSheetsService();
            int update_scores = activity.UpdateScores();
            int changesMade = activity.Prefs.ApplyChanges();

            if (result == 0)
            {
                int id = changesMade > 0 ? Resource.String.saved : Resource.String.no_changes;
                activity.CreateToast(id);
            }
        };

        return view;
    }
}
