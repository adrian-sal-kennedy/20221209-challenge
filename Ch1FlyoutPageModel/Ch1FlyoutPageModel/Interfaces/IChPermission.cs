using Ch1FlyoutPageModel.Models;

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
        bool IsGranted { get; set; }
        bool IsEssentialForAppToRunProperly { get; }
    }
}
