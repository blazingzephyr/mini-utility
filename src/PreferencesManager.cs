
using Android.Content;

namespace MiniUtility;

internal class PreferencesManager
{
    public string? AccountSettings { get; set; }
    public string? AppName { get; set; }
    public string? SheetId { get; set; }
    public string? Range { get; set; }
    public bool AutoIncrement { get; set; }

    public string? CurrentAccountSettings { get; private set; }
    public string? CurrentAppName { get; private set; }
    public string? CurrentSheetId { get; private set; }
    public string? CurrentRange { get; private set; }
    public bool CurrentAutoIncrement { get; set; }

    private readonly ISharedPreferences? _prefs;
    private readonly ISharedPreferencesEditor? _editor;

    public PreferencesManager(Context context)
    {
        _prefs = context.GetSharedPreferences("google_settings", FileCreationMode.Private);
        if (_prefs is null) return;

        _editor = _prefs.Edit();
        AccountSettings = _prefs.GetString("account_settings", string.Empty);
        AppName = _prefs.GetString("app_name", string.Empty);
        SheetId = _prefs.GetString("sheet_id", string.Empty);
        Range = _prefs.GetString("range", string.Empty);
        AutoIncrement = _prefs.GetBoolean("auto_increment", true);

        CurrentAccountSettings = AccountSettings;
        CurrentAppName = AppName;
        CurrentSheetId = SheetId;
        CurrentRange = Range;
        CurrentAutoIncrement = AutoIncrement;
    }

    public int ApplyChanges()
    {
        int changesMade = 0;
        if (_editor is null) return changesMade;

        if (CurrentAccountSettings != AccountSettings)
        {
            CurrentAccountSettings = AccountSettings;
            _editor.PutString("account_settings", AccountSettings);
            changesMade += 1;
        }

        if (CurrentAppName != AppName)
        {
            CurrentAppName = AppName;
            _editor.PutString("app_name", AppName);
            changesMade += 1;
        }

        if (CurrentSheetId != SheetId)
        {
            CurrentSheetId = SheetId;
            _editor.PutString("sheet_id", SheetId);
            changesMade += 1;
        }

        if (CurrentRange != Range)
        {
            CurrentRange = Range;
            _editor.PutString("range", Range);
            changesMade += 1;
        }

        if (CurrentAutoIncrement != AutoIncrement)
        {
            CurrentAutoIncrement = AutoIncrement;
            _editor.PutBoolean("auto_increment", AutoIncrement);
            changesMade += 1;
        }

        _editor.Apply();

        return changesMade;
    }
}
