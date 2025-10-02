
using Android.Views;

namespace MiniUtility;

public class TasksFragment : AndroidX.Fragment.App.Fragment
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = inflater.Inflate(Resource.Layout.tasks_tab, container, false);
        if (view == null) return new View(Context);

        // W.I.P.
        return view;
    }
}
