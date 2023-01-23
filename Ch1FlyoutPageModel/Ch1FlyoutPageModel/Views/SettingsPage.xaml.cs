using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using System.Threading.Tasks;
    using Models;
    using ViewModels;
    using ar = AppResources;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is SettingsViewModel vm)
            {
                Task.Run(() =>
                {
                    var perm = new ChPermission(Permission.Bluetooth)
                    {
                        PermissionDescription = ar.BluetoothPermissionDescription,
                        PermissionRationale = ar.BluetoothPermissionRationale,
                    };
                    vm.AskPermission(perm);
                });
                // hasRunOnce = true;
            }
        }
    }
}
