using System.Collections.Generic;
using Ch1FlyoutPageModel.Interfaces;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IDevices
    {
        bool BtIsOn { get; }
        bool GpsIsOn { get; }
        bool CheckPermission();
    }
}
