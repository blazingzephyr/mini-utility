
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class NestedRecyclerAdapter(int resource, IList<IList<string>> items) : RecyclerView.Adapter
{
    public event EventHandler<ChildTextChangedEventArgs>? TextChanged;
    public override int ItemCount => Items.Count;
    public int Res { get; set; } = resource;
    public IList<IList<string>> Items { get; set; } = [..items];

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        LayoutInflater inflater = LayoutInflater.From(parent.Context)!;
        View view = inflater.Inflate(Resource.Layout.parent_row, parent, false)!;
        return new RecyclerViewHolder(view);
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        if (holder is not RecyclerViewHolder recyclerHolder) return;
        recyclerHolder.Bind(TextChanged, position, Res, items[position]);
    }
}
