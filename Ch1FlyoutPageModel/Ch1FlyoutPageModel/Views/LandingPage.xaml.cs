using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);
            Device.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"//PermissionsPage"));
        }
    }
}