using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IPermissionAsker
    {
        bool AskPermission(IChPermission perm);
        IEnumerable<IChPermission> CheckAllPermissions();
    }
}