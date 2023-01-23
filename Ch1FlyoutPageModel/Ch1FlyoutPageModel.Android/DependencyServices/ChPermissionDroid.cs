using Android;
using AndroidX.Core.Content;
using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.ComponentModel;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class ChPermissionDroid : IChPermission
    {
        public Permission PermissionType { get; set; }
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string PermissionDescription { get; set; }
        public string PermissionRationale { get; set; }
        public bool IsPermitted => ContextCompat.CheckSelfPermission(PermissionAsker.Context, PermissionName) > 0;

        public ChPermissionDroid(IChPermission perm)
        {
            PermissionType = perm.PermissionType;
            PermissionDescription = perm.PermissionDescription;
            PermissionRationale = perm.PermissionRationale;
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
                    return new[] { Manifest.Permission.ActivityRecognition };
                case Permission.LocationAlwaysAllow:
                    return new[]
                    {
                        Manifest.Permission.AccessBackgroundLocation,
                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation,
                    };
                case Permission.LocationOnlyForeground:
                    return new[]
                    {
                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation,
                    };
                case Permission.Bluetooth:
                    return new[]
                    {
                        Manifest.Permission.Bluetooth,
                        Manifest.Permission.BluetoothAdmin,
                    };
                default:
                    return new[] { string.Empty };
            }
        }
    }
}
