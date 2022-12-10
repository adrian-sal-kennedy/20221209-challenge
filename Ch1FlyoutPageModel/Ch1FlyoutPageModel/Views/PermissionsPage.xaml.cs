using Ch1FlyoutPageModel.Models;
using Ch1FlyoutPageModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsPage : ContentPage
    {
        public PermissionsPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PermissionsViewModel vm)
            {
                vm.MissingPermissions = new()
                {
                    new ChPermission()
                    {
                        PermissionName = "bklah",
                        PermissionDescription = "a bit of text",
                        PermissionRationale = "to have something to look at",
                    }
                };
            }
        }
    }
}