using Android.Bluetooth;
using Ch1FlyoutPageModel.Interfaces;

namespace Ch1FlyoutPageModel.Droid.DependencyServices
{
    public class BluetoothDeviceDroid : IBluetoothDevice
    {
        public object? NativeObject { get; }
        public string? Name { get; }
        public string? Alias { get; }
        public string? Address { get; }

        public BluetoothDeviceDroid(BluetoothDevice bluetoothDevice)
        {
            NativeObject = bluetoothDevice;
            Name = bluetoothDevice.Name;
            Alias = bluetoothDevice.Alias;
            Address = bluetoothDevice.Address;
        }
    }
}
