using Android.Bluetooth;
using Android.Content;
using Ch1FlyoutPageModel.ViewModels;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IBluetooth
    {
    }
}

namespace Ch1FlyoutPageModel.Droid.Classes
{
    using Ch1FlyoutPageModel.DependencyServices;

    public class Bluetooth : IBluetooth
    {
        // [BroadcastReceiver(Enabled = true, Exported = true)]
        // [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged }, Categories = new[] { Intent.CategoryDefault })]
        [BroadcastReceiver]
        public class BluetoothReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent is not { Extras: { } }) { return; }

                State state = (State)intent.GetIntExtra(BluetoothAdapter.ExtraState, -1);
                BaseViewModel.SetIsBluetoothOn((int)state is not 10 or 11); // "Off" or "Turning On"
            }
        }
    }
}
