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
        public static bool IsWaiting;
        public PermissionsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsWaiting)
            {
                IsWaiting = true;
                if (BindingContext is PermissionsViewModel vm
                    && vm.MissingPermissions.Count > 0)
                {
                    // Task.Run(vm.AskPermissions());
                    vm.AskPermissions();
                }
            }
        }
    }
}