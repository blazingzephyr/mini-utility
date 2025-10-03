
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace MiniUtility;

internal class NestedChildViewHolder(View itemView) : RecyclerView.ViewHolder(itemView)
{
    public TextView TextView { get; } = itemView.FindViewById<TextView>(Resource.Id.childText)!;
}
