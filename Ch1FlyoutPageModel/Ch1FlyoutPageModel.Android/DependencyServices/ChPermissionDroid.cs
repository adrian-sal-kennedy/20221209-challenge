using Android;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.ComponentModel;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    using System.Collections.Generic;
    using Xamarin.Forms;

    public class ChPermissionDroid : IChPermission
    {
        public Permission PermissionType { get; set; }
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
        }

        public string[] ToStringArray()
        {
            // this switch case could go on forever, but Xamarin.Forms is cross-platform and
            // it's nice to have room for these values to change around
            switch (PermissionType)
            {
                case Permission.Camera:
                    return new[] { Manifest.Permission.Camera };
                case Permission.ActivityRecognition:
                    if (MainActivity.ApiLevel >= 29)
                    {
                        return new[] { Manifest.Permission.ActivityRecognition };
                    }

                    break;
                case Permission.LocationAlwaysAllow:
                    var arr = new List<string>()
                    {
                        Manifest.Permission.AccessFineLocation, 
                        Manifest.Permission.AccessCoarseLocation,
                    };
                    if (MainActivity.ApiLevel >= 29)
                    {
                        arr.Add(Manifest.Permission.AccessBackgroundLocation);
                    }

                    return arr.ToArray();
                case Permission.LocationOnlyForeground:
                    return new[] { Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation, };
                case Permission.Bluetooth:
                    return new[] { Manifest.Permission.Bluetooth,
                        Manifest.Permission.BluetoothAdmin, };
                default:
                    return new[] { string.Empty };
            }
            return new string[] { };
        }
    }
}
