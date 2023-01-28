using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using ar = AppResources;
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        // public string ButtonText => ar.
        public NotificationPage()
        {
            InitializeComponent();
        }
    }
}

