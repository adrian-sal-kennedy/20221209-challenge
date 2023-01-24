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
    [Activity(Label = "Ch1FlyoutPageModel", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Bundle? savedInstanceState;
        private PermissionAsker.PermissionReceiver? permissionReceiver;
        private Bluetooth.BluetoothReceiver? bluetoothReceiver;

        protected override void OnCreate(Bundle savedInstanceStateArg)
        {
            base.OnCreate(savedInstanceStateArg);
            savedInstanceState = savedInstanceStateArg;
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true); 
            Xamarin.Essentials.Platform.Init(this, savedInstanceStateArg);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceStateArg);
            // FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageViewHandler();
            LoadApplication(new App());
            permissionReceiver = new PermissionAsker.PermissionReceiver();
            bluetoothReceiver = new Bluetooth.BluetoothReceiver();
            // RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
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
                Xamarin.Forms.Forms.Init(this, savedInstanceState);
                Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            }

            RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
            RegisterReceiver(bluetoothReceiver, new IntentFilter(PackageName));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionAsker.PermissionReceivedThankYou.TrySetResult(grantResults.Any(gr => (int)gr != -1));
            PermissionAsker.PermissionReceivedThankYou = new TaskCompletionSource<bool>();
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(permissionReceiver);
            UnregisterReceiver(bluetoothReceiver);
        }
    }
}
