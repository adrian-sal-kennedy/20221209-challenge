namespace Ch1FlyoutPageModel.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public bool HasAllPermissions { get; set; } = true;
        public bool IsBluetoothOn => Xamarin.Essentials.Connectivity.ConnectionProfiles
        public bool IsGpsOn { get; set; } = true;
        public bool IsInternetAvailable { get; set; }
        public SettingsViewModel()
        {
        }
    }
}