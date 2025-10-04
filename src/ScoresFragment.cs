
using System.Net;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ClosedXML.Excel;
using Google;

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
            var sheetRange = scoreSheets[i][1];

            string title = string.Empty;
            IList<string> values = [];

            try
            {
                var valuesRequest = activity.Service.Spreadsheets.Values.Get(sheetId, sheetRange);
                var nameRequest = activity.Service.Spreadsheets.Get(sheetId);
                nameRequest.Fields = "properties.title";

                var titleResponse = await nameRequest.ExecuteAsync();
                title = titleResponse.Properties.Title;

                var valuesResponse = await valuesRequest.ExecuteAsync();
                var valuesRow = valuesResponse.Values[0];
                values = new string[valuesRow.Count];

                for (int j = 0; j < valuesRow.Count; j++)
                {
                    values[j] = valuesRow[j].ToString()!;
                }
            }
            catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.BadRequest)
            {
                if (activity.DriveService is null) return;

                using MemoryStream memoryStream = new MemoryStream();
                var fileRequest = activity.DriveService.Files.Get(sheetId);

                var file = await fileRequest.ExecuteAsync();
                title = file.Name.Split(".xls")[0];

                var downloadResponse = await fileRequest.DownloadAsync(memoryStream);
                if (downloadResponse.Status == Google.Apis.Download.DownloadStatus.Failed)
                {
                    activity.CreateToast($"{sheetId}: {downloadResponse.Exception.Message}");
                    return;
                }

                XLWorkbook workbook = new(memoryStream);
                IXLCells cells = workbook.Cells(sheetRange);

                values = [];
                foreach (var cell in cells)
                {
                    if (!cell.IsEmpty())
                    {
                        values.Add(cell.GetString());
                    }
                }
            }
            catch (Exception ex)
            {
                activity.CreateToast($"{sheetId}: {ex.Message}");
                return;
            }

            dataset.Add(new string[values.Count + 1]);
            dataset[i][0] = title;

            for (int j = 0; j < values.Count; j++)
            {
                dataset[i][j + 1] = values[j];
            }

            scoresTable.SetAdapter(new NestedRecyclerAdapter(Resource.Layout.nested_item_readonly, dataset));
            activity.CreateToast(title, ToastLength.Short);
        }
    }
}
