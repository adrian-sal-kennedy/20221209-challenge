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
        private ChPermission? selectedPermission;

        public ChPermission? SelectedPermission
        {
            get => selectedPermission;
            set => SetProperty(ref selectedPermission, value);
        }

        public ICommand AskPermissionCommand { get; private set; }

        public PermissionsViewModel()
        {
            AskPermissionCommand = new Command((perm) =>
            {
                if (perm is IChPermission p)
                {
                    AskPermission(p);
                }
            });
        }

        public static void SetPermissions(IEnumerable<IChPermission> perms)
        {
            permissions = new(perms.Select(p => new ChPermission(p)));
        }

        public async void AskPermissions()
        {
            Task.Run(async () =>
            {
                // reqs say "sequentially", otherwise I'd make a list of tasks and a WhenAll.
                // Given we need to wait for user interaction it's not performance constrained.
                int cnt = permissions.Count;
                foreach (var perm in permissions)
                {
                    bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                    if (res)
                    {
                        // DependencyService.Get<IToastMessage>().Show(AppResources.ToastPermissionGranted);
                    }

                    OnPropertyChanged(nameof(Permissions));
                }
            }).Wait();
            await AppShell.GoToOnMainThreadAsync("//List");
        }
    }
}
