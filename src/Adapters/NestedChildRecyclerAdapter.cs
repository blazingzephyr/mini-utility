
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class NestedChildRecyclerAdapter(
    EventHandler<ChildTextChangedEventArgs>? textChanged,
    int topPosition,
    int resource,
    IList<string> items) : RecyclerView.Adapter
{
    public override int ItemCount => items.Count;

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        LayoutInflater inflater = LayoutInflater.From(parent.Context)!;
        View view = inflater.Inflate(resource, parent, false)!;
        return new NestedChildViewHolder(view);
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        if (holder is not NestedChildViewHolder child) return;
        child.TextView.Text = items[position];
        child.TextView.TextChanged += (s, o) =>
        {
            if (textChanged is null) return;

            ChildTextChangedEventArgs args = new(o.Text, o.Start, o.BeforeCount, o.AfterCount, topPosition, position);
            textChanged(s, args);
        };
    }
}
