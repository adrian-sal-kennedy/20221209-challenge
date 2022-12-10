using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel.ViewModels
{
    public class PermissionsViewModel : BaseViewModel
    {
        public ICommand CheckPermissions { get; set; }
        private ObservableCollection<ChPermission> missingPermissions = new();
        public ObservableCollection<ChPermission> MissingPermissions
        {
            get => missingPermissions;
            set => SetProperty(ref missingPermissions, value);
        }
        public PermissionsViewModel()
        {
            CheckPermissions = new Command(() =>
            {
                MissingPermissions = new(DependencyService.Get<IPermissionAsker>().CheckAllPermissions());
            });
        }
    }
}
