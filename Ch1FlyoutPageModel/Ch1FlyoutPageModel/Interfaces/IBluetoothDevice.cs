// true

namespace Ch1FlyoutPageModel.Interfaces
{
    public interface IBluetoothDevice
    {
        object? NativeObject { get; }
        string? Name { get; }
        string? Alias { get; }
        string? Address { get; }
    }
}
