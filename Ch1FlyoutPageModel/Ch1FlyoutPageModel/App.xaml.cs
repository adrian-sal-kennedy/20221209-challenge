using Ch1FlyoutPageModel.DependencyServices;
using Ch1FlyoutPageModel.Interfaces;
using Ch1FlyoutPageModel.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Ch1FlyoutPageModel
{
    public partial class App : Application
    {
        public static ResourceDictionary Colours => Current.Resources.MergedDictionaries.FirstOrDefault() ?? new();
        public static bool ThemeIsDark => Current.RequestedTheme == OSAppTheme.Dark;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            // DependencyService.Register<MockDataStore>();
            List<IChPermission> permissions = DependencyService.Get<IPermissionAsker>().CheckAllPermissions().ToList() ?? new();
            PermissionsViewModel.SetPermissions(permissions);
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