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
        public ICommand AskPermissionsCommand { get; }
        public PermissionsViewModel()
        {
            AskPermissionsCommand = new Command(AskPermissions);
            Task.Run(AskPermissions);
        }
        public static void SetPermissions(IEnumerable<IChPermission> perms)
        {
            missingPermissions = new(perms);
        }
        public async void AskPermissions()
        {
            // reqs say "sequentially", otherwise I'd make a list of tasks and a WhenAll.
            // Given we're waiting for user interaction it's not performance constrained.
            var permissions = MissingPermissions;
            await Task.Run(async () =>
            {
                foreach (var perm in permissions)
                {
                    if (DependencyService.Get<IPermissionAsker>().AskPermission(perm))
                    {
                        MissingPermissions.Remove(perm);
                    }
                }
            });
            OnPropertyChanged(nameof(MissingPermissions));
            return;
        }
    }
}