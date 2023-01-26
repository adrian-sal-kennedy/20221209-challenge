namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Windows.Input;
    using DependencyServices;
    using Xamarin.Forms;

    public class SettingsViewModel : BaseViewModel
    {
        private static SettingsViewModel? _instance;
        public ICommand RefreshCommand { get; set; }

        public SettingsViewModel()
        {
            _instance = this;
            MessagingCenter.Subscribe<SettingsViewModel>(this, "OnPropertyChanged()", (sender) =>
            {
                _ = CheckDevicesStatuses();
            });
            RefreshCommand = new Command((sender) =>
            {
                CheckDevicesStatuses();
                if (sender is RefreshView rv) { rv.IsRefreshing = false; }
            });
        }

        public bool CheckDevicesStatuses()
        {
            if (!DependencyService.Get<IDevices>().CheckPermission()) { return false; }

            bool getterHitter = IsBluetoothOn & IsGpsOn;
            OnPropertyChanged(nameof(IsBluetoothOn));
            OnPropertyChanged(nameof(IsGpsOn));
            OnPropertyChanged(nameof(IsInternetAvailable));
            return getterHitter;
        }

        public static void CheckBluetoothStatusStatic()
        {
            MessagingCenter.Send(_instance!, "OnPropertyChanged()");
        }
    }
}
