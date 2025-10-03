
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class RecyclerViewHolder : RecyclerView.ViewHolder
{
    private readonly RecyclerView _innerRecycler;

    public RecyclerViewHolder(View itemView) : base(itemView)
    {
        _innerRecycler = itemView.FindViewById<RecyclerView>(Resource.Id.childRecyclerView)!;
        _innerRecycler.SetLayoutManager(new LinearLayoutManager(itemView.Context, LinearLayoutManager.Horizontal, false));
    }

    public void Bind(EventHandler<ChildTextChangedEventArgs>? textChanged, int topPosition, int resource, IList<string> items)
    {
        var adapter = new NestedChildRecyclerAdapter(textChanged, topPosition, resource, items);
        _innerRecycler.SetAdapter(adapter);
    }
}
