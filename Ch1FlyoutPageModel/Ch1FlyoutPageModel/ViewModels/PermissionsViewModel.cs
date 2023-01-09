using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.Models;
using System;
using System.Collections;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ch1FlyoutPageModel.Views;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    public class PermissionsViewModel : BaseViewModel
    {
        private static ObservableCollection<IChPermission> missingPermissions = new();

        public ObservableCollection<IChPermission> MissingPermissions
        {
            get => missingPermissions;
            set => SetProperty(ref missingPermissions, value);
        }

        private IChPermission selectedPermission;

        public IChPermission SelectedPermission
        {
            get => selectedPermission;
            set => SetProperty(ref selectedPermission, value ?? new ChPermission(new()));
        }

        public ICommand AskPermissionsCommand => new Command(AskPermissions);

        public PermissionsViewModel()
        {
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
                while (missingPermissions.Count > 0)
                {
                    var perm = missingPermissions.FirstOrDefault();
                    bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm);
                    // if (res)
                    // {
                    // }
                    OnPropertyChanged(nameof(MissingPermissions));
                    missingPermissions.Remove(perm);
                }
            });

            PermissionsPage.IsWaiting = false;
        }
    }
}