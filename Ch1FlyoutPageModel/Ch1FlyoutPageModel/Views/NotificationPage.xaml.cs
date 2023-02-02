using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ch1FlyoutPageModel.Views
{
    using ViewModels;
    using ar = AppResources;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        // public string ButtonText => 
        public NotificationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is NotificationViewModel { IsAlarmSet: { } alarmSet } vm)
            {
                Button.Text = alarmSet ? ar.CancelAlarm : ar.SetAlarm;
            }
        }
    }
}
