using System.Linq;

namespace Ch1FlyoutPageModel.ViewModels
{
    using DependencyServices;
    using Xamarin.Forms;

    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
        }

        public bool CheckBluetoothStatus()
        {
            bool res = DependencyService.Get<IBluetooth>().CheckPermission(); 
            if (res)
            {
                // wifi and cellular
                var list = Xamarin.Essentials.Connectivity.ConnectionProfiles.ToList();
            }

            return false;
        }
    }
}
