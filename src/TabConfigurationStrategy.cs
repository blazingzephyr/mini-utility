
using Android.Content;
using Google.Android.Material.Tabs;
using Object = Java.Lang.Object;

namespace MiniUtility;

internal class TabConfigurationStrategy(Context context) : Object, TabLayoutMediator.ITabConfigurationStrategy
{
    private readonly Context _context = context;

    public void OnConfigureTab(TabLayout.Tab tab, int position)
    {
        int id = position switch
        {
            0 => Resource.String.finances,
            // 1 => Resource.String.tasks,
            1 => Resource.String.scores,
            _ => Resource.String.settings
        };

        string text = _context.GetString(id);
        tab.SetText(text);
    }
}
