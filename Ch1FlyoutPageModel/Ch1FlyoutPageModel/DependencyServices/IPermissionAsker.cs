using Ch1FlyoutPageModel.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IPermissionAsker
    {
        IEnumerable<ChPermission> AskAllPermissions();
        IEnumerable<ChPermission> CheckAllPermissions();
    }
}