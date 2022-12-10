using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Xamarin.Essentials;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class PermissionAsker : IPermissionAsker
    {
        public IEnumerable<ChPermission> CheckAllPermissions()
        {
            var activity = Platform.CurrentActivity;
            var context = Application.Context;
            var count = new List<ChPermission>() { };
            // note that we add to the list when we *don't* have permission.
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.AccessWifiState) < 0)
            {
                count.Add(new ChPermission()
                {
                    PermissionName = Manifest.Permission.AccessFineLocation,
                    PermissionDescription = "Fine Location access",
                    PermissionRationale = "We need Wi-Fi permission, which can be used to determine location",
                });
            }
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.ActivityRecognition) < 0)
            {
                count.Add(new ChPermission()
                {
                    PermissionName = Manifest.Permission.ActivityRecognition,
                    PermissionDescription = "Detect physical activity",
                    PermissionRationale = "We need to pump iron",
                });
            }
            if ((int)ContextCompat.CheckSelfPermission(context, Manifest.Permission.ActivityRecognition) < 0)
            {
                count.Add(new ChPermission()
                {
                    PermissionName = Manifest.Permission.ActivityRecognition,
                    PermissionDescription = "Detect physical activity",
                    PermissionRationale = "We need to pump iron",
                });
            }
            return count;
        }
    }
}