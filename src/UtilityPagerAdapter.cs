
using AndroidX.ViewPager2.Adapter;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace MiniUtility;

internal class UtilityPagerAdapter(MainActivity activity) : FragmentStateAdapter(activity)
{
    public override int ItemCount => 3;

    private readonly FinancesFragment _finances = new FinancesFragment();
    // private readonly TasksFragment _tasks = new TasksFragment();
    private readonly ScoresFragment _scoresFragment = new ScoresFragment();
    private readonly SettingsFragment _settings = new SettingsFragment();

    public override Fragment CreateFragment(int position)
    {
        return position switch
        {
            0 => _finances,
            // 1 => _tasks,
            1 => _scoresFragment,
            2 => _settings,
            _ => throw new InvalidOperationException()
        };
    }
}
