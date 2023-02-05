using Ch1FlyoutPageModel.Droid.DependencyServices;
using Xamarin.Forms;
using ar = Ch1FlyoutPageModel.AppResources;

[assembly: Dependency(typeof(Devices))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using System;
    using System.Linq;
    using Android.App;
    using Android.Bluetooth;
    using Android.Content;
    using Android.Locations;
    using AndroidX.Core.Content;
    using Ch1FlyoutPageModel.DependencyServices;
    using Models;
    using ViewModels;
    using Xamarin.Essentials;

    public class Devices : IDevices
    {
        private static Activity activity => Platform.CurrentActivity;
        private static Context Context => activity.Application!.ApplicationContext!;

        private static AlarmManager? alarmManager;
        public bool BtIsOn => BtAdapter is { State: { } state } && (int)state is not 10 or 11;

        public bool GpsIsOn => locationManager is { IsLocationEnabled: true };

        public bool AlarmIsSet
        {
            get => alarmManager is { };
        }

        [IntentFilter(new[] { "SetAlarmActivity" })]
        [BroadcastReceiver(Enabled = true, Exported = true)]
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

        private static LocationManager? locationManager =
            Context.GetSystemService(Context.LocationService) as LocationManager;

        private static BluetoothManager? bluetoothManager =
            Context.GetSystemService(Context.BluetoothService) as BluetoothManager;

        // and once again, Android changed this in API 31 so it's not going to be relevant beyond this exercise...
        // private static string? bestProvider => locationManager?.GetBestProvider(new Criteria(null), true);
        // private LocationProvider? LocationProvider => locationManager?.GetProvider(bestProvider ?? "");
        private BluetoothAdapter? BtAdapter => bluetoothManager?.Adapter;

        [BroadcastReceiver(Enabled = true, Exported = false)]
        [IntentFilter(new[] { BluetoothAdapter.ActionStateChanged })]
        public class BluetoothReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context? context, Intent? intent)
            {
                if (intent is not { Extras: { } }) { return; }

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

        public void SetAlarm(long repeatEveryMillis)
        {
            var intent = new Intent(Context, typeof(AlarmReceiver))
                .SetFlags(ActivityFlags.NewTask);

            PendingIntent? pe = PendingIntent.GetBroadcast(Context, (int)RequestCodes.Alarm, intent,
                PendingIntentFlags.Immutable);
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            alarmManager = (AlarmManager?)Context.GetSystemService(Context.AlarmService);
            if (pe is { } && alarmManager is { })
            {
                alarmManager.SetRepeating(AlarmType.Rtc, now + repeatEveryMillis, repeatEveryMillis, pe);
                DependencyService.Get<IToastMessage>().Show(ar.SetAlarm);
            }
        }

        public void CancelAlarm()
        {
            var intent = new Intent(Context, typeof(AlarmReceiver))
                .SetFlags(ActivityFlags.NewTask);
            PendingIntent? pe = PendingIntent.GetBroadcast(Context, (int)RequestCodes.Alarm, intent,
                PendingIntentFlags.Immutable);
            // PendingIntent? pe = PendingIntent.GetService(Context, (int)RequestCodes.Alarm, intent, PendingIntentFlags.NoCreate);
            if (alarmManager is { } && pe is { }) { alarmManager.Cancel(pe); }
            // sketchy I know, but added as the simplest dumbest way to ensure
            // the alarm is cancelled and my checks also work.
            alarmManager = null;
        }
    }
}
