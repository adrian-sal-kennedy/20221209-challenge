using System;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel
{
    using System.Threading.Tasks;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            // Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        public static async Task GoToOnMainThreadAsync(string? route)
        {
            if (route is not { }) { return; }

            await Device.InvokeOnMainThreadAsync(() => Current.GoToAsync(route));
        }

        /// <summary>
        /// the methods OnEditButtonClicked and OnSaveButtonClicked are just... antipatterns.
        /// But this is such a simple feature that we're doing it this way on this occasion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Element { Parent: { Parent: ContentView { } gp } } &&
                gp.Resources.TryGetValue("EditView", out object t))
            {
                gp.ControlTemplate = (ControlTemplate)t;
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (sender is Element { Parent: { Parent: ContentView { } gp } } &&
                gp.Resources.TryGetValue("DefaultUserProfileView", out object t))
            {
                gp.ControlTemplate = (ControlTemplate)t;
            }
        }
    }
}
