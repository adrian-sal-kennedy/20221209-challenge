using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using System.Linq;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.Droid
{
    using Android.Provider;
    using AndroidX.Core.App;
    using Ch1FlyoutPageModel.DependencyServices;
    using Xamarin.Forms;

    [Activity(Label = "Ch1FlyoutPageModel", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Bundle? savedInstanceState;
        private PermissionAsker.PermissionReceiver? permissionReceiver;
        private Devices.BluetoothReceiver? bluetoothReceiver;
        private static Devices.AlarmReceiver? alarmReceiver;
        public static int ApiLevel => (int)Build.VERSION.SdkInt;

        protected override void OnCreate(Bundle savedInstanceStateArg)
        {
            base.OnCreate(savedInstanceStateArg);
            savedInstanceState = savedInstanceStateArg;
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            Xamarin.Essentials.Platform.Init(this, savedInstanceStateArg);
            Forms.Init(this, savedInstanceStateArg);
            FormsMaterial.Init(this, savedInstanceStateArg);
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageViewHandler();
            LoadApplication(new App());
            permissionReceiver = new PermissionAsker.PermissionReceiver();
            bluetoothReceiver = new Devices.BluetoothReceiver();
            alarmReceiver = new Devices.AlarmReceiver();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            savedInstanceState = outState;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (savedInstanceState != null)
            {
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                Forms.Init(this, savedInstanceState);
                FormsMaterial.Init(this, savedInstanceState);
            }

            var intent = RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
            intent = RegisterReceiver(bluetoothReceiver, new IntentFilter(BluetoothAdapter.ActionStateChanged));
            intent = RegisterReceiver(alarmReceiver, new IntentFilter(AlarmClock.ActionSetAlarm));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionAsker.PermissionReceivedThankYou.TrySetResult(grantResults.Any(gr => (int)gr != -1));
            PermissionAsker.PermissionReceivedThankYou = new TaskCompletionSource<bool>();
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnPause()
        {
            DependencyService.Get<IToastMessage>().Show(AppResources.ToastLifecycleOnPause);
            base.OnPause();
            UnregisterReceiver(permissionReceiver);
            UnregisterReceiver(bluetoothReceiver);
            UnregisterReceiver(alarmReceiver);
        }
    }
}
