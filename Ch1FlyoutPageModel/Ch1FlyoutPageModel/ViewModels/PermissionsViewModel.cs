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

        private IChPermission chPermission;

        public IChPermission ChPermission
        {
            get => chPermission;
            set => SetProperty(ref chPermission, value ?? new ChPermission(new()));
        }

        public ICommand AskPermissionsCommand { get; }

        public PermissionsViewModel()
        {
            AskPermissionsCommand = new Command(AskPermissions);
        }

        public static void SetPermissions(IEnumerable<IChPermission> perms)
        {
            missingPermissions = new(perms);
        }

        public async void AskPermissions()
        {
            // reqs say "sequentially", otherwise I'd make a list of tasks and a WhenAll.
            // Given we need to wait for user interaction it's not performance constrained.
            var permissions = MissingPermissions;
            foreach (var perm in permissions)
            {
                bool res = await DependencyService.Get<IPermissionAsker>().AskPermission(perm); 
                if (res)
                {
                    MissingPermissions.Remove(perm);
                }
            }

            OnPropertyChanged(nameof(MissingPermissions));
            PermissionsPage.IsWaiting = false;
        }
    }
}