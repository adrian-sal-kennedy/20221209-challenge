using Ch1FlyoutPageModel.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsPage
    {
        // private static bool hasRunOnce;
        public PermissionsPage()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is PermissionsViewModel { MissingPermissions: { Count: > 0 } } vm)
            {
                Task.Run(vm.AskPermissions);
                // hasRunOnce = true;
            }
        }
        // protected override void OnAppearing()
        // {
        //     base.OnAppearing();
        //     if (!hasRunOnce && BindingContext is PermissionsViewModel { MissingPermissions: { Count: > 0 } } vm)
        //     {
        //         // Task.Run(vm.AskPermissions);
        //         hasRunOnce = true;
        //     }
        // }
    }
}