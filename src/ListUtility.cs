
namespace MiniUtility;

internal static class ListUtility
{
    public static bool Compare(IList<IList<string>> a, IList<IList<string>> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (!a[i].SequenceEqual(b[i]))
            {
                return false;
            }
        }

        return true;
    }
}
