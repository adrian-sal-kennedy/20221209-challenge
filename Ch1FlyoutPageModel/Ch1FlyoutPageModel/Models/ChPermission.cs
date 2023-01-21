using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Interfaces;
using System;
using System.ComponentModel;
using System.Resources;
using Xamarin.Essentials;

namespace Ch1FlyoutPageModel.Models
{
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
    }
    public class ChPermission : IChPermission
    {
        public Permission PermissionType { get; set; } = 0;
        public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
        public string PermissionDescription { get; set; } = "";
        public string PermissionRationale { get; set; } = "";
        public bool IsPermitted { get; }
        public ChPermission(Permission perm)
        {
            PermissionType = perm;
        } 
    }
}