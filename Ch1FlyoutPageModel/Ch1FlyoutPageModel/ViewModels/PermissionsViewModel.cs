using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Linq;

    public class PermissionsViewModel : BaseViewModel
    {
        private IChPermission? selectedPermission;

        public IChPermission? SelectedPermission
        {
            get => selectedPermission;
            set => SetProperty(ref selectedPermission, value);
        }

        public ICommand TogglePermissionCommand { get; private set; }

        public PermissionsViewModel()
        {
            TogglePermissionCommand = new Command((perm) =>
            {
                if (perm is IChPermission p)
                {
                    TogglePermission(p);
                }
            });
        }

        public static void SetPermissions(IEnumerable<IChPermission> perms)
        {
            permissions = new(perms);
        }

        public async Task AskPermissions()
        {
            // reqs say "sequentially", otherwise I'd make a list of tasks and a WhenAll.
            // Given we need to wait for user interaction it's not performance constrained.
            foreach (var perm in permissions)
            {
                bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                if (res)
                {
                    // DependencyService.Get<IToastMessage>().Show(AppResources.ToastPermissionGranted);
                }

                OnPropertyChanged(nameof(Permissions));
            }
        }

        protected void TogglePermission(IChPermission? perm)
        {
            Task.Run(async () =>
            {
                bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                if (res)
                {
                    var grantedPerm = permissions.FirstOrDefault(p => p.PermissionType == perm?.PermissionType);
                    if (grantedPerm is { })
                    {
                        grantedPerm.IsGranted = res;
                    }
                }

                OnPropertyChanged(nameof(Permissions));
            }).Wait();
        }
    }
}
