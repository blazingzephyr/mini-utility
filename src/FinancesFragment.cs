
using System.Text.RegularExpressions;
using Android.Views;
using Android.Widget;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace MiniUtility;

public class FinancesFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater,
        ViewGroup? container, 
        Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.finances_tab, container, false);
        if (view is null) return new View(Context);

        var store = view.FindViewById<EditText>(Resource.Id.store);
        var product = view.FindViewById<EditText>(Resource.Id.product);
        var category = view.FindViewById<Spinner>(Resource.Id.category);
        var price = view.FindViewById<EditText>(Resource.Id.price);
        var days = view.FindViewById<Spinner>(Resource.Id.days);
        var button = view.FindViewById<Button>(Resource.Id.add_button);

        if (store is null || product is null || category is null ||
            price is null || days is null || button is null) return view;

        category.Adapter = ArrayAdapter.CreateFromResource(
            RequireContext(),
            Resource.Array.categories,
            Android.Resource.Layout.SimpleSpinnerItem);

        days.Adapter = ArrayAdapter.CreateFromResource(
            RequireContext(),
            Resource.Array.days,
            Android.Resource.Layout.SimpleSpinnerItem);

        string[] daysArray = Resources.GetStringArray(Resource.Array.days);
        string[] categoryAlt = Resources.GetStringArray(Resource.Array.categories_alt);
        string[] categoryMain = Resources.GetStringArray(Resource.Array.categories);

        button.Click += async (sender, e) =>
        {
            if (RequireActivity() is not MainActivity activity) return;
            if (activity.Prefs is not PreferencesManager prefs) return;

            if (activity.Service is not SheetsService service)
            {
                Activity?.RunOnUiThread(() =>
                    Toast.MakeText(Context, Resource.String.not_connected, ToastLength.Long)?.Show());

                return;
            }

            string today = $"{DateTime.Now:dd.MM.yyyy}";
            string? dur = daysArray[days.SelectedItemPosition];
            string category1 = categoryAlt[category.SelectedItemPosition];
            string category2 = categoryMain[category.SelectedItemPosition];
            string? cost = price.Text?.Replace('.', ',');

            var valueRange = new ValueRange();
            valueRange.Values = [[today, store.Text, product.Text, category1, category2, cost, dur]];

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, prefs.SheetId, prefs.Range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            try
            {
                var updateResponse = await Task.Run(updateRequest.Execute);
                int toast = updateResponse.UpdatedCells > 0 ? Resource.String.success : Resource.String.fail;
                activity.CreateToast(toast);

                if (updateResponse.UpdatedCells > 0 && prefs.AutoIncrement && prefs.Range is not null)
                {
                    prefs.Range = Regex.Replace(prefs.Range, @"(?<=\D)(\d+)", m => (int.Parse(m.Value) + 1).ToString());
                    prefs.ApplyChanges();
                }
            }
            catch (Exception ex)
            {
                activity.CreateToast(ex.Message);
            }
        };

        return view;
    }
}
