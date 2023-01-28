using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    public class PermissionsViewModel : BaseViewModel
    {
        private IChPermission? selectedPermission;
        public IChPermission? SelectedPermission
        {
            get => selectedPermission;
            set => SetProperty(ref selectedPermission, value ?? new ChPermission(new()));
        }

        public ICommand AskPermissionCommand { get; private set; }

        public PermissionsViewModel()
        {
            new Command(AskPermissions);
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
            missingPermissions = new(perms);
        }

        public async void AskPermissions()
        {
            await Task.Run(async () =>
            {
                // reqs say "sequentially", otherwise I'd make a list of tasks and a WhenAll.
                // Given we need to wait for user interaction it's not performance constrained.
                int cnt = missingPermissions.Count;
                for (int i = cnt; i > 0;)
                {
                    var perm = missingPermissions[i - 1];
                    bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                    if (res)
                    {
                        missingPermissions.Remove(perm);
                        OnPropertyChanged(nameof(MissingPermissions));
                    }
                    i--;
                }
            });
            await AppShell.GoToOnMainThreadAsync("//List");
        }
    }
}
