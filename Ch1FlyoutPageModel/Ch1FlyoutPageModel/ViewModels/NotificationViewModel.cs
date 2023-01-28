namespace Ch1FlyoutPageModel.ViewModels
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class NotificationViewModel : BaseViewModel
    {
        public ICommand SetAlarmCommand { get; set; }
        public NotificationViewModel()
        {
            SetAlarmCommand = new Command(async (sender) =>
            {
                SetAlarm();
                if (sender is RefreshView rv) { rv.IsRefreshing = false; }
            });
        }

        private void SetAlarm()
        {
            throw new NotImplementedException();
        }
    }
}
