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

[assembly: Dependency(typeof(Devices))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using Android.Locations;

    public class Devices : IDevices
    {
        private static Context Context => Application.Context;

        private static LocationManager? locationManager =
            Context.GetSystemService(Context.LocationService) as LocationManager;

        private static BluetoothManager? bluetoothManager =
            Context.GetSystemService(Context.BluetoothService) as BluetoothManager;

        // and once again, Android changed this in API 31 so it's not going to be relevant beyond this exercise...
        // private static string? bestProvider => locationManager?.GetBestProvider(new Criteria(null), true);
        // private LocationProvider? LocationProvider => locationManager?.GetProvider(bestProvider ?? "");
        private BluetoothAdapter? BtAdapter => bluetoothManager?.Adapter;

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

        public bool BtIsOn => BtAdapter is { State: { } state } && (int)state is not 10 or 11;

        public bool GpsIsOn => locationManager is { IsLocationEnabled: true };

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
