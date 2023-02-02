using Android.Content;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ar = Ch1FlyoutPageModel.AppResources;

[assembly: Dependency(typeof(PermissionAsker))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using Android.App;

    public class PermissionAsker : IPermissionAsker
    {
        public static Context Context => Application.Context;
        private MainActivity Activity => (Platform.CurrentActivity as MainActivity)!;
        public static TaskCompletionSource<bool> PermissionReceivedThankYou = new();

        [BroadcastReceiver(Enabled = true, Exported = false)]
        // [IntentFilter(Android.Content.ContextWrapper.PackageName)]
        public class PermissionReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context? context, Intent? intent)
            {
                // permissionReceivedThankYou.TrySetResult(ContextCompat.CheckSelfPermission(context, perm.ToString()) == 0);
                PermissionReceivedThankYou.TrySetResult(intent?.DataString != null);
                PermissionReceivedThankYou = new TaskCompletionSource<bool>();
            }
        }

        public async Task<bool> AskPermission(IChPermission? permission)
        {
            if (permission is not { }) { return false; }

            var perm = new ChPermissionDroid(permission);
            var permArr = perm.ToStringArray();
            // var prec = new PermissionReceiver();
            // Activity.RegisterReceiver(prec, new IntentFilter(Activity.PackageName));
            var permcheck = permArr.Select(prmStr => ContextCompat.CheckSelfPermission(Context, prmStr)).ToList();

            if (permcheck.Any(p => p != Android.Content.PM.Permission.Granted))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ActivityCompat.RequestPermissions(
                        Activity,
                        permArr,
                        (int)RequestCodes.Permission
                    );
                });
                bool res = await PermissionReceivedThankYou.Task;
                if (!res && perm.IsEssentialForAppToRunProperly)
                {
                    if (perm.ShouldShowRationale())
                    {
                        // Show an explanation to the user *asynchronously* -- don't block
                        // this thread waiting for the user's response! After the user
                        // sees the explanation, try again to request the permission.
                        // var dialog = new AlertDialog.Builder(this)
                        //     .SetTitle("Permission needed")
                        //     .SetMessage(
                        //         "This permission is needed because we need access to your camera to take a photo.")
                        //     .SetPositiveButton("OK", (sender, args) =>
                        //     {
                        //         ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.CAMERA },
                        //             requestCode);
                        //     })
                        //     .SetNegativeButton("Cancel", (sender, args) =>
                        //     {
                        //         // User denied permission
                        //     })
                        //     .Create();
                        // dialog.Show();
                    }
                    else
                    {
                        // No explanation needed; request the permission
                        // ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.CAMERA }, requestCode);
                    }
                }
            }
            else
            {
                return true;
            }

            permcheck.Clear();
            return permArr
                .Select(prmStr => ContextCompat.CheckSelfPermission(Context, prmStr))
                .All(p => p == Android.Content.PM.Permission.Granted);
        }

        public IEnumerable<IChPermission> CheckAllPermissions()
        {
            var perms = new List<IChPermission>()
            {
                new ChPermissionDroid(Permission.LocationAlwaysAllow, true)
                {
                    PermissionDescription = ar.LocationAlwaysAllowDescription,
                    PermissionRationale = ar.LocationAlwaysAllowRationale,
                },
                new ChPermissionDroid(Permission.ActivityRecognition, true)
                {
                    PermissionDescription = ar.ActivityRecognitionDescription,
                    PermissionRationale = ar.ActivityRecognitionRationale,
                },
                new ChPermissionDroid(Permission.Camera, true)
                {
                    PermissionDescription = ar.CameraDescription, PermissionRationale = ar.CameraRationale,
                },
            };

            return perms;
        }
    }
}
