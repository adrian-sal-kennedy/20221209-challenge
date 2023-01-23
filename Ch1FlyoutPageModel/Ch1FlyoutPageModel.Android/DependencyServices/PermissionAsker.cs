using Android;
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
using Application = Android.App.Application;

[assembly: Dependency(typeof(PermissionAsker))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class PermissionAsker : IPermissionAsker
    {
        public static Context Context => Application.Context;
        private MainActivity Activity => (Platform.CurrentActivity as MainActivity)!;
        public static TaskCompletionSource<bool> PermissionReceivedThankYou = new();

        [BroadcastReceiver(Enabled = true, Exported = false)]
        public class PermissionReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                // permissionReceivedThankYou.TrySetResult(ContextCompat.CheckSelfPermission(context, perm.ToString()) == 0);
                PermissionReceivedThankYou.TrySetResult(intent.DataString != null);
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
            var permcheck = ContextCompat.CheckSelfPermission(Context, perm.ToStringArray().First());
            if (permcheck != 0)
            {
                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    ActivityCompat.RequestPermissions(
                        Activity,
                        permArr,
                        (int)RequestCodes.Permission
                    );
                });
                bool res = await PermissionReceivedThankYou.Task;
                if (res)
                {
                    // anything here?
                }
            }
            permcheck = ContextCompat.CheckSelfPermission(Context, perm.ToStringArray().First());
            return permcheck == 0;
        }

        public IEnumerable<IChPermission> CheckAllPermissions()
        {
            var count = new List<ChPermission>() { };
            // note that we add to the list when we *don't* have permission.
            if ((int)ContextCompat.CheckSelfPermission(Context, Manifest.Permission.AccessWifiState) < 0
                || (int)ContextCompat.CheckSelfPermission(Context, Manifest.Permission.AccessBackgroundLocation) < 0)
            {
                count.Add(new ChPermission(Permission.LocationAlwaysAllow)
                {
                    PermissionDescription = ar.LocationAlwaysAllowDescription,
                    PermissionRationale = ar.LocationAlwaysAllowRationale,
                });
            }

            if ((int)ContextCompat.CheckSelfPermission(Context, Manifest.Permission.ActivityRecognition) < 0)
            {
                count.Add(new ChPermission(Permission.ActivityRecognition)
                {
                    PermissionDescription = ar.ActivityRecognitionDescription,
                    PermissionRationale = ar.ActivityRecognitionRationale,
                });
            }

            if ((int)ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) < 0)
            {
                count.Add(new ChPermission(Permission.Camera)
                {
                    PermissionDescription = ar.CameraDescription,
                    PermissionRationale = ar.CameraRationale,
                });
            }

            return count;
        }
    }
}
