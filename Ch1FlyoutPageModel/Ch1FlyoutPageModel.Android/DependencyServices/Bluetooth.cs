using System.Collections.Generic;
using Android.Bluetooth;
using Android.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.ViewModels;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using System;
    using System.Linq;

    public class Bluetooth : IBluetooth
    {
        // [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged }, Categories = new[] { Intent.CategoryDefault })]
        [BroadcastReceiver(Enabled = true, Exported = true)]
        public class BluetoothReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent is not { Extras: { } }) { return; }

                State state = (State)intent.GetIntExtra(BluetoothAdapter.ExtraState, -1);
                BaseViewModel.SetIsBluetoothOn((int)state is not 10 or 11); // "Off" or "Turning On"
            }
        }

        private List<BluetoothDeviceDroid> GetBluetoothDevices()
        {
            List<BluetoothDeviceDroid> list = new();
            BluetoothAdapter? adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter is { IsEnabled: true, BondedDevices: { } devices })
            {
                list = devices.Select(d => new BluetoothDeviceDroid(d)).ToList();
            }

            return list;
        }

        public IEnumerable<IBluetoothDevice> Devices
        {
            get => GetBluetoothDevices();
        }

        public bool CheckPermission() => throw new NotImplementedException();
    }
}
