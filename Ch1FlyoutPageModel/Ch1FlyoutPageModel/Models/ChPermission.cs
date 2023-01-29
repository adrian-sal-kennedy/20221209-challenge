using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using System;
using System.ComponentModel;
using System.Resources;
using Xamarin.Essentials;

namespace Ch1FlyoutPageModel.Models
{
    using System.Collections.Generic;

    public enum Permission : int
    {
        [EnumDescriptionResource("Undefined", typeof(AppResources))]
        Undefined = 0,

        [EnumDescriptionResource("LocationAlwaysAllow", typeof(AppResources))]
        LocationAlwaysAllow,

        [EnumDescriptionResource("LocationOnlyForeground", typeof(AppResources))]
        LocationOnlyForeground,

        [EnumDescriptionResource("ActivityRecognition", typeof(AppResources))]
        ActivityRecognition,

        [EnumDescriptionResource("Camera", typeof(AppResources))]
        Camera,

        [EnumDescriptionResource("Bluetooth", typeof(AppResources))]
        Bluetooth,
    }

    public class ChPermission : IChPermission
    {
        public Permission PermissionType { get; set; }
        public List<object> NativePermissions { get; } = new();
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string? PermissionDescription { get; set; } = "";
        public string? PermissionRationale { get; set; } = "";
        public bool IsGranted { get; }
        public bool IsEssentialForAppToRunProperly { get; }

        public ChPermission(Permission perm, bool isEssential = false)
        {
            PermissionType = perm;
            IsEssentialForAppToRunProperly = isEssential;
        }
    }
}
