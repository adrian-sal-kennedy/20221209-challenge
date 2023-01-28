using System;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel
{
    using System.Threading.Tasks;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Xamarin.Forms.Shell
    {
        // public ContentView UserProfile;
        public AppShell()
        {
            InitializeComponent();
            // Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            // Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        public static async Task GoToOnMainThreadAsync(string? path)
        {
            if (path is not { }) { return; }

            await Device.InvokeOnMainThreadAsync(() => Current.GoToAsync(path));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await GoToAsync("//LoginPage");
        }

        /// <summary>
        /// the below 2 methods are just... antipatterns. But this is such
        /// a simple feature that we're doing it this way on this occasion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Element { Parent: { Parent: ContentView { } gp } })
            {
                if (gp.Resources.TryGetValue("EditView", out object t))
                {
                    gp.ControlTemplate = (ControlTemplate)t;
                }
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (sender is Element { Parent: { Parent: ContentView { } gp } })
            {
                if (gp.Resources.TryGetValue("DefaultUserProfileView", out object t))
                {
                    gp.ControlTemplate = (ControlTemplate)t;
                }
            }
        }
    }
}
