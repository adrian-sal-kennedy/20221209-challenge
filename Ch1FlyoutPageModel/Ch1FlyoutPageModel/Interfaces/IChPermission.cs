using Ch1FlyoutPageModel.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.Interfaces
{
    public interface IChPermission
    {
        Permission PermissionType { get; set; }
        string PermissionName { get; }
        string PermissionDescription { get; set; }
        string PermissionRationale { get; set; }
        bool IsPermitted { get; }
    }
}