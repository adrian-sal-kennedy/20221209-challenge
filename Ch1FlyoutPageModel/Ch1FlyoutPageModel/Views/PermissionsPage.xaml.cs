using Ch1FlyoutPageModel.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsPage
    {
        public PermissionsPage()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is PermissionsViewModel { Permissions: { Count: > 0 } } vm)
            {
                Task.Run(vm.AskPermissions);
            }
        }
    }
}
