using System.Linq;

namespace Ch1FlyoutPageModel.ViewModels
{
    using DependencyServices;
    using Xamarin.Forms;

    public class SettingsViewModel : BaseViewModel
    {
        private static SettingsViewModel? _instance;

        public SettingsViewModel()
        {
            _instance = this;
            MessagingCenter.Subscribe<SettingsViewModel>(this, "OnPropertyChanged(nameof(IsBluetoothOn))", (sender) =>
            {
                _ = CheckBluetoothStatus();
            });
        }

        public bool CheckBluetoothStatus()
        {
            if (!DependencyService.Get<IBluetooth>().CheckPermission()) { return false; }
            bool ibo = IsBluetoothOn;
            OnPropertyChanged(nameof(IsBluetoothOn));
            return ibo;
        }

        public static void CheckBluetoothStatusStatic()
        {
            MessagingCenter.Send(_instance!, "OnPropertyChanged(nameof(IsBluetoothOn))");
        }
    }
}
