using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Java.Util.Jar;
using System.Reflection;

namespace Ch1FlyoutPageModel.Droid
{
    [Activity(Label = "Ch1FlyoutPageModel", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private PermissionAsker.PermissionReceiver permissionReceiver;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            permissionReceiver = new PermissionAsker.PermissionReceiver();
            RegisterReceiver(permissionReceiver, new IntentFilter(PackageName));
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnResume()
        {
            base.OnResume();
        }
        protected override void OnPause()
        {
            UnregisterReceiver(permissionReceiver);
            base.OnPause();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}