
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class ScoresFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater,
        ViewGroup? container, 
        Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.scores_tab, container, false);
        if (view is null) return new View(Context);
        if (RequireActivity() is not MainActivity activity) return view;

        var scoresTable = view.FindViewById<RecyclerView>(Resource.Id.scoresTable)!;
        scoresTable.SetLayoutManager(new LinearLayoutManager(Context));

        activity.ScoresUpdated += async () => await RefreshScores(activity, scoresTable);
        activity.UpdateScores(true);

        return view;
    }

    public async Task RefreshScores(MainActivity activity, RecyclerView scoresTable)
    {
        if (activity.Prefs is null) return;
        if (activity.Service is null) return;

        var scoreSheets = activity.Prefs.ScoreSheets;
        List<IList<string>> dataset = new List<IList<string>>(scoreSheets.Count);

        for (int i = 0; i < scoreSheets.Count; i++)
        {
            var sheetId = scoreSheets[i][0];
            var valuesRequest = activity.Service.Spreadsheets.Values.Get(sheetId, scoreSheets[i][1]);
            var nameRequest = activity.Service.Spreadsheets.Get(sheetId);
            nameRequest.Fields = "properties.title";

            try
            {
                var titleResponse = await nameRequest.ExecuteAsync();
                var title = titleResponse.Properties.Title;

                var valuesResponse = await valuesRequest.ExecuteAsync();
                var values = valuesResponse.Values[0];

                dataset.Add(new string[values.Count + 1]);
                dataset[i][0] = title;

                for (int j = 0; j < values.Count; j++)
                {
                    dataset[i][j + 1] = values[j].ToString()!;
                }

                scoresTable.SetAdapter(new NestedRecyclerAdapter(Resource.Layout.nested_item_readonly, dataset));
                activity.CreateToast(title, ToastLength.Short);
            }
            catch (Exception ex)
            {
                activity.CreateToast($"{sheetId}: {ex.Message}");
            }
        }
    }
}
