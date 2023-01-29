using Android;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.ComponentModel;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AndroidX.Core.App;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class ChPermissionDroid : IChPermission
    {
        private MainActivity Activity => (Platform.CurrentActivity as MainActivity)!;
        public Permission PermissionType { get; set; }
        public List<object> NativePermissions { get; } = new();
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string? PermissionDescription { get; set; }
        public string? PermissionRationale { get; set; }
        public bool IsGranted => ContextCompat.CheckSelfPermission(PermissionAsker.Context, PermissionName) > 0;
        public bool IsEssentialForAppToRunProperly { get; }

        public ChPermissionDroid(IChPermission perm)
        {
            PermissionType = perm.PermissionType;
            PermissionDescription = perm.PermissionDescription;
            PermissionRationale = perm.PermissionRationale;
            IsEssentialForAppToRunProperly = perm.IsEssentialForAppToRunProperly;
        }

        public ChPermissionDroid(Permission perm, bool isEssential = false)
        {
            PermissionType = perm;
            IsEssentialForAppToRunProperly = isEssential;

            switch (perm)
            {
                case Permission.Camera:
                    NativePermissions.Add(Manifest.Permission.Camera);
                    break;
                case Permission.ActivityRecognition:
                    if (MainActivity.ApiLevel >= 29)
                    {
                        NativePermissions.Add(Manifest.Permission.ActivityRecognition);
                    }

                    break;
                case Permission.LocationAlwaysAllow:
                    NativePermissions.AddRange(new[]
                        {
                            Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation,
                        }
                    );
                    if (MainActivity.ApiLevel >= 29)
                    {
                        NativePermissions.Add(Manifest.Permission.AccessBackgroundLocation);
                        IsEssentialForAppToRunProperly = true;
                    }

                    break;
                case Permission.LocationOnlyForeground:
                    NativePermissions.AddRange(new[]
                    {
                        Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation,
                    });
                    break;
                case Permission.Bluetooth:
                    NativePermissions.AddRange(new[]
                    {
                        Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin,
                    });
                    break;
            }
        }

        public string[] ToStringArray()
        {
            return NativePermissions.Select(p => p.ToString()).ToArray();
        }

        public bool ShouldShowRationale()
        {
            return NativePermissions.Aggregate(false,
                (agg, p) => agg | ActivityCompat.ShouldShowRequestPermissionRationale(Activity, p.ToString()) ||
                            IsEssentialForAppToRunProperly);
        }
    }
}
