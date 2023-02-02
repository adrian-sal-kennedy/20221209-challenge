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

    public class ChPermissionDroid : IChPermission
    {
        private MainActivity Activity => (Platform.CurrentActivity as MainActivity)!;
        public Permission PermissionType { get; set; }
        public List<object> NativePermissions { get; } = new();
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string? PermissionDescription { get; set; }
        public string? PermissionRationale { get; set; }

        private bool isGranted;

        public bool IsGranted
        {
            get
            {
                var res = CheckDroidPermissions();
                isGranted = res == 0;
                return isGranted;
            }
        }

        private int CheckDroidPermissions()
        {
            int agg = 0;
            foreach (object obj in NativePermissions)
            {
                if (obj is string p)
                {
                    agg += (int)ContextCompat.CheckSelfPermission(PermissionAsker.Context, p);
                }
            }
            return agg;
        }

        public bool IsEssentialForAppToRunProperly { get; set; }

        public ChPermissionDroid(IChPermission perm)
        {
            PermissionType = perm.PermissionType;
            PermissionDescription = perm.PermissionDescription;
            PermissionRationale = perm.PermissionRationale;
            if (perm is not { NativePermissions: { } np })
            {
                var res = ChPermissionToNativeList(perm.PermissionType);
                IsEssentialForAppToRunProperly = perm.IsEssentialForAppToRunProperly || res.Item2;
                NativePermissions = res.Item1;
            }
            else
            {
                NativePermissions = perm.NativePermissions;
            }
        }

        public ChPermissionDroid(Permission perm, bool isEssential = false)
        {
            PermissionType = perm;
            var res = ChPermissionToNativeList(perm);
            IsEssentialForAppToRunProperly = isEssential || res.Item2;
            NativePermissions = res.Item1;
        }

        private (List<object>, bool) ChPermissionToNativeList(Permission perm)
        {
            var nativePermissions = new List<object>();
            bool isEssentialForAppToRunProperly = false;
            switch (perm)
            {
                case Permission.Camera:
                    nativePermissions.Add(Manifest.Permission.Camera);
                    break;
                case Permission.ActivityRecognition:
                    if (MainActivity.ApiLevel >= 29)
                    {
                        nativePermissions.Add(Manifest.Permission.ActivityRecognition);
                    }

                    break;
                case Permission.LocationAlwaysAllow:
                    nativePermissions.AddRange(new[]
                        {
                            Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation,
                        }
                    );
                    if (MainActivity.ApiLevel >= 29)
                    {
                        nativePermissions.Add(Manifest.Permission.AccessBackgroundLocation);
                        isEssentialForAppToRunProperly = true;
                    }

                    break;
                case Permission.LocationOnlyForeground:
                    nativePermissions.AddRange(new[]
                    {
                        Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation,
                    });
                    break;
                case Permission.Bluetooth:
                    nativePermissions.AddRange(new[]
                    {
                        Manifest.Permission.Bluetooth, Manifest.Permission.BluetoothAdmin,
                    });
                    break;
            }

            return (nativePermissions, isEssentialForAppToRunProperly);
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
