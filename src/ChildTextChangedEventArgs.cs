
using Android.Text;

namespace MiniUtility;

public class ChildTextChangedEventArgs(IEnumerable<char>? text, int start, int before, 
    int after, int position0, int position1) : TextChangedEventArgs(text, start, before, after)
{
    public int Position1 { get; } = position0;
    public int Position2 { get; } = position1;
}
