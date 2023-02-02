using Android.Bluetooth;
using Android.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.ViewModels;
using System.Linq;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Xamarin.Forms;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.Models;
using ar = Ch1FlyoutPageModel.AppResources;

[assembly: Dependency(typeof(Devices))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using System;
    using Android.App;
    using Android.Locations;
    using Android.OS;
    using Android.Provider;

    public class Devices : IDevices
    {
        private static Context Context => Application.Context;

        private static AlarmManager? alarmManager =
            Context.GetSystemService(Context.AlarmService) as AlarmManager;

        public bool BtIsOn => BtAdapter is { State: { } state } && (int)state is not 10 or 11;

        public bool GpsIsOn => locationManager is { IsLocationEnabled: true };

        public bool AlarmIsSet
        {
            get
            {
                Intent checker = new Intent(Context, typeof(AlarmReceiver));
                PendingIntent? pe = PendingIntent.GetBroadcast(Context, (int)RequestCodes.Alarm, checker,
                    PendingIntentFlags.NoCreate);
                return pe is { };
            }
        }

        [BroadcastReceiver(Enabled = true, Exported = false)]
        [IntentFilter(new[] { AlarmClock.ActionSetAlarm })]
        public class AlarmReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context? context, Intent? intent)
            {
                if (intent is not { }) { return; }

                var location = locationManager?.GetLastKnownLocation(LocationManager.GpsProvider)
                               ?? locationManager?.GetLastKnownLocation(LocationManager.NetworkProvider);

                if (location is { Latitude: { } lat, Longitude: { } lon })
                {
                    DependencyService.Get<IToastMessage>().Show(
                        $"Lat: {lat} Long: {lon}");
                }
            }
        }

        [Activity(Label = "SetAlarmActivity")]
        public class SetAlarmActivity : Activity
        {
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);

                var intent = new Intent(this, typeof(AlarmReceiver));
                var pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);

                var triggerTime = SystemClock.ElapsedRealtime() + TimeSpan.FromSeconds(10).Ticks;
                alarmManager?.Set(AlarmType.ElapsedRealtimeWakeup, triggerTime, pendingIntent);
            }
        }

        private static LocationManager? locationManager =
            Context.GetSystemService(Context.LocationService) as LocationManager;

        private static BluetoothManager? bluetoothManager =
            Context.GetSystemService(Context.BluetoothService) as BluetoothManager;

        // and once again, Android changed this in API 31 so it's not going to be relevant beyond this exercise...
        // private static string? bestProvider => locationManager?.GetBestProvider(new Criteria(null), true);
        // private LocationProvider? LocationProvider => locationManager?.GetProvider(bestProvider ?? "");
        private BluetoothAdapter? BtAdapter => bluetoothManager?.Adapter;

        // [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged }, Categories = new[] { Intent.CategoryDefault })]
        [BroadcastReceiver(Enabled = true, Exported = false)]
        [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged })]
        public class BluetoothReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context? context, Intent? intent)
            {
                if (intent is not { Extras: { } }) { return; }

                State state = (State)intent.GetIntExtra(BluetoothAdapter.ExtraState, -1);
                // BaseViewModel.SetIsBluetoothOn((int)state is not 10 or 11); // "Off" or "Turning On"
                SettingsViewModel.CheckBluetoothStatusStatic();
            }
        }

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

        public void SetAlarm()
        {
            AlarmManager? alarmContext = Context.GetSystemService(Context.AlarmService) as AlarmManager;
            if (alarmContext is { })
            {
                var intent = new Intent(Context, typeof(SetAlarmActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                Context.StartActivity(intent);
                DependencyService.Get<IToastMessage>().Show(ar.SetAlarm);
            }
        }

        public void CancelAlarm()
        {
            var intent = new Intent(Context, typeof(SetAlarmActivity));
            PendingIntent? pe = PendingIntent.GetService(Context, (int)RequestCodes.Alarm, intent,
                PendingIntentFlags.NoCreate);

            alarmManager.Cancel(pe);
        }
    }
}
