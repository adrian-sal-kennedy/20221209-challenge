using Ch1FlyoutPageModel.Helpers;
using Ch1FlyoutPageModel.Models;
using System.ComponentModel;

namespace Ch1FlyoutPageModel.Interfaces
{
    public interface IChPermission
    {
        Permission PermissionType { get; set; }
        string PermissionName { get; }
        string PermissionDescription { get; set; }
        string PermissionRationale { get; set; }
    }
}