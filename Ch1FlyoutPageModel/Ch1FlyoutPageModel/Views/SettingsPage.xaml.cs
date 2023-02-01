using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using ViewModels;
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
            vm?.CheckDevicesStatuses();
        }
    }
}
