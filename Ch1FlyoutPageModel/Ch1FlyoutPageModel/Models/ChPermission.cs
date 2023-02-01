using Ch1FlyoutPageModel.Helpers;
// using Ch1FlyoutPageModel.Interfaces;
// using System.ComponentModel;

namespace Ch1FlyoutPageModel.Models
{
    // using System.Collections.Generic;
    // using System.Runtime.CompilerServices;

    public enum Permission
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
//
//     public class ChPermission : IChPermission, INotifyPropertyChanged
//     {
//         public Permission PermissionType { get; set; }
//         public List<object> NativePermissions { get; set; } = new();
//         public string PermissionName => PermissionType.GetAttributeOfType<DescriptionAttribute>().Description ?? "";
//         public string? PermissionDescription { get; set; } = "";
//
//         public string? PermissionRationale { get; set; } = "";
//         private bool isGranted;
//
//         public bool IsGranted
//         {
//             get => isGranted;
//             set => SetProperty(ref isGranted, value);
//         }
//
//         public bool IsEssentialForAppToRunProperly { get; }
//
//         public ChPermission(Permission perm, bool isEssential = false)
//         {
//             PermissionType = perm;
//             IsEssentialForAppToRunProperly = isEssential;
//         }
//         public ChPermission(IChPermission perm)
//         {
//             PermissionType = perm.PermissionType;
//             PermissionDescription = perm.PermissionDescription;
//             PermissionRationale = perm.PermissionRationale;
//             NativePermissions = perm.NativePermissions;
//             IsEssentialForAppToRunProperly = perm.IsEssentialForAppToRunProperly;
//         }
//
//         public event PropertyChangedEventHandler? PropertyChanged;
//
//         protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
//         {
//             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//         }
//
//         protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
//         {
//             if (EqualityComparer<T>.Default.Equals(field, value))
//                 return false;
//             field = value;
//             OnPropertyChanged(propertyName);
//             return true;
//         }
//     }
}
