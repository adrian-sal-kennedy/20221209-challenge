using Android;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Droid.DependencyServices;
using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ar = Ch1FlyoutPageModel.AppResources;
using Application = Android.App.Application;

[assembly: Dependency(typeof(PermissionAsker))]

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class ChPermissionDroid : IChPermission
    {
        public Permission PermissionType { get; set; }
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string PermissionDescription { get; set; }
        public string PermissionRationale { get; set; }
        public ChPermissionDroid(IChPermission perm)
        {
            PermissionType = perm.PermissionType;
            PermissionDescription = perm.PermissionDescription;
            PermissionRationale = perm.PermissionRationale;
        }
        public override string ToString()
        {
            // this could go on forever, but Xamarin.Forms is cross-platform and
            // it's nice to have room for these values to change around
            switch (PermissionType)
            {
                case Permission.Camera:
                    return Manifest.Permission.Camera;
                case Permission.ActivityRecognition:
                    return Manifest.Permission.ActivityRecognition;
                case Permission.LocationAlwaysAllow:
                    return Manifest.Permission.AccessBackgroundLocation;
                case Permission.LocationOnlyForeground:
                    return Manifest.Permission.AccessFineLocation;
                default:
                    return string.Empty;
            }
        }
    }

    public class PermissionAsker : IPermissionAsker
    {
        private static TaskCompletionSource<bool> permissionReceivedThankYou = new TaskCompletionSource<bool>();

        [BroadcastReceiver(Enabled = true, Exported = false)]
        public class PermissionReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                permissionReceivedThankYou.TrySetResult(intent.DataString != null);
                permissionReceivedThankYou = new TaskCompletionSource<bool>();
            }
        }

        public async Task<bool> AskPermission(IChPermission permission)
        {
            var perm = new ChPermissionDroid(permission);
            var activity = Platform.CurrentActivity;
            Device.InvokeOnMainThreadAsync(() =>
            {
                ActivityCompat.RequestPermissions(
                    activity,
                    new string[] { perm.ToString() },
                    (int)RequestCodes.Permission
                );
            });
            var res = await permissionReceivedThankYou.Task;
            return res;
        }

        public IEnumerable<IChPermission> CheckAllPermissions()
        {
            var context = Application.Context;
            var count = new List<ChPermission>() { };
            // note that we add to the list when we *don't* have permission.
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.AccessWifiState) < 0)
            {
                count.Add(new ChPermission(Permission.LocationAlwaysAllow)
                {
                    PermissionDescription = ar.LocationAlwaysAllowDescription,
                    PermissionRationale = ar.LocationAlwaysAllowRationale,
                });
            }
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.ActivityRecognition) < 0)
            {
                count.Add(new ChPermission(Permission.ActivityRecognition)
                {
                    PermissionDescription = ar.ActivityRecognitionDescription,
                    PermissionRationale = ar.ActivityRecognitionRationale,
                });
            }
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.Camera) < 0)
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