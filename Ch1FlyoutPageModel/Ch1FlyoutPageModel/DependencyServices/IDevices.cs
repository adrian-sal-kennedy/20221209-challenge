namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IDevices
    {
        bool BtIsOn { get; }
        bool GpsIsOn { get; }
        bool AlarmIsSet { get; }
        bool CheckPermission();
        void SetAlarm();
        void CancelAlarm();
    }
}
