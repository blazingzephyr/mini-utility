
namespace MiniUtility;

internal static class ToastHelper
{
    public static void CreateToast(
        this Activity activity,
        int id,
        ToastLength length = ToastLength.Long) =>
            activity.RunOnUiThread(() => Toast.MakeText(activity, id, length)?.Show());

    public static void CreateToast(
        this Activity activity,
        string message,
        ToastLength length = ToastLength.Long) =>
            activity.RunOnUiThread(() => Toast.MakeText(activity, message, length)?.Show());
}
