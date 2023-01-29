using Ch1FlyoutPageModel.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Interfaces
{
    using System.Collections.Generic;

    public interface IChPermission
    {
        Permission PermissionType { get; set; }
        List<object> NativePermissions { get; }
        string PermissionName { get; }
        string? PermissionDescription { get; set; }
        string? PermissionRationale { get; set; }
        bool IsGranted { get; }
        bool IsEssentialForAppToRunProperly { get; }
    }
}
