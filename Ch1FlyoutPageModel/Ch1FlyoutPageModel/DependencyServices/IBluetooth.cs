using System.Collections.Generic;
using Ch1FlyoutPageModel.Interfaces;

namespace Ch1FlyoutPageModel.DependencyServices
{
    public interface IBluetooth
    {
        IEnumerable<IBluetoothDevice> Devices { get; }
        bool CheckPermission();
    }
}
