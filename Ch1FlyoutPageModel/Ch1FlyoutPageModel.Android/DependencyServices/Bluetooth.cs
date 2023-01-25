using System.Collections.Generic;
using Android.Bluetooth;
using Android.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.ViewModels;
using System.Linq;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Xamarin.Forms;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.Models;
using Application = Android.App.Application;

[assembly: Dependency(typeof(Bluetooth))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class Bluetooth : IBluetooth
    {
        public static Context Context => Application.Context;

        private BluetoothManager? bluetoothManager =
            Context.GetSystemService(Context.BluetoothService) as BluetoothManager;

        public BluetoothAdapter? BtAdapter => bluetoothManager?.Adapter;

        // [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged }, Categories = new[] { Intent.CategoryDefault })]
        [BroadcastReceiver(Enabled = true, Exported = true)]
        public class BluetoothReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent is not { Extras: { } }) { return; }

                State state = (State)intent.GetIntExtra(BluetoothAdapter.ExtraState, -1);
                // BaseViewModel.SetIsBluetoothOn((int)state is not 10 or 11); // "Off" or "Turning On"
                SettingsViewModel.CheckBluetoothStatusStatic();
            }
        }
        public bool IsOn => BtAdapter is { State: { } state } && (int)state is not 10 or 11;

        public bool CheckPermission()
        {
            if (Context is { } context)
            {
                var permString = new ChPermissionDroid(Permission.Bluetooth).ToStringArray();
                bool res = ContextCompat.CheckSelfPermission(context, permString.FirstOrDefault()) == 0;
                return res;
            }

            return false;
        }
    }
}
