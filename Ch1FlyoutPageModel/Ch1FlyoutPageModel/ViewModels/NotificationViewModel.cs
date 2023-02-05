namespace Ch1FlyoutPageModel.ViewModels
{
    using System.Windows.Input;
    using DependencyServices;
    using Xamarin.Forms;
    using ar = AppResources;

    public class NotificationViewModel : BaseViewModel
    {
        public bool IsAlarmSet => DependencyService.Get<IDevices>().AlarmIsSet;
        public ICommand SetAlarmCommand { get; set; }

        public NotificationViewModel()
        {
            SetAlarmCommand = new Command((sender) =>
            {
                if (sender is Button btn)
                {
                    if (IsAlarmSet)
                    {
                        btn.Text = ar.SetAlarm;
                        DependencyService.Get<IDevices>().CancelAlarm();
                    }
                    else
                    {
                        btn.Text = ar.CancelAlarm;
                        DependencyService.Get<IDevices>().SetAlarm(5 * 1000);
                    }
                }
            });
        }
    }
}
