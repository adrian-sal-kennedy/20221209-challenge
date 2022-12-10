using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.ViewModels;
using Ch1FlyoutPageModel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // DependencyService.Register<MockDataStore>();
            List<IChPermission> permissions = DependencyService.Get<IPermissionAsker>().CheckAllPermissions().ToList() ?? new();
            PermissionsViewModel.SetPermissions(permissions);
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}