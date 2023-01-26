using Ch1FlyoutPageModel.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastMessage))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using Android.App;
    using Android.Content;
    using Android.Widget;
    using Ch1FlyoutPageModel.DependencyServices;
    public class ToastMessage : IToastMessage
    {
        private static Context Context => Application.Context;

        public void Show(string? msg)
        {
            if (msg is { })
            {
                Toast.MakeText(Context, msg, ToastLength.Long)?.Show();
            }
        }
    }
}
