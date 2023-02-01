using Ch1FlyoutPageModel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IPermissionAsker
    {
        Task<bool> AskPermission(IChPermission? perm);
        IEnumerable<IChPermission> CheckAllPermissions();
    }
}
