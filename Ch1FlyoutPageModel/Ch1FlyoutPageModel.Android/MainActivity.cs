using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Java.Util.Jar;
using System.Linq;
using AndroidX.Core.App;
using System.Reflection;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.Droid
{
    [Activity(Label = "Ch1FlyoutPageModel", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Bundle _savedInstanceState;
        private PermissionAsker.PermissionReceiver permissionReceiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _savedInstanceState = savedInstanceState;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            permissionReceiver = new PermissionAsker.PermissionReceiver();
            // RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            _savedInstanceState = outState;
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (_savedInstanceState != null)
            {
                Xamarin.Essentials.Platform.Init(this, _savedInstanceState);
                Xamarin.Forms.Forms.Init(this, _savedInstanceState);
                Xamarin.Forms.FormsMaterial.Init(this, _savedInstanceState);
            }
            RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
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
        }
    }
}