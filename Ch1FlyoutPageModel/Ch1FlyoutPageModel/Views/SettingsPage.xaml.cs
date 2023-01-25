using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using System.Threading.Tasks;
    using DependencyServices;
    using Models;
    using ViewModels;
    using Xamarin.Forms;
    using ar = AppResources;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        private SettingsViewModel? vm => BindingContext as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm?.CheckBluetoothStatus();
        }
    }
}
