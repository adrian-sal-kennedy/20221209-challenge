namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Diagnostics;
    using DependencyServices;
    using Xamarin.Forms;

    public class BluetoothViewModel : BaseViewModel
    {
        public bool BluetoothToggle
        {
            get => IsBluetoothOn;
            set
            {
                DependencyService.Get<IToastMessage>().Show("Bluetooth toggling not yet implemented...");
            }
        }
    }
}
