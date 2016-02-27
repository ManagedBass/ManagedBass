using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public class MidiDeviceInfo
    {
        IntPtr name;
        int id;
        DeviceInfoFlags flags;

        public int ID => id;

        public string Name => Marshal.PtrToStringAnsi(name);

        public bool IsEnabled => flags.HasFlag(DeviceInfoFlags.Enabled);

        public bool IsInitialized => flags.HasFlag(DeviceInfoFlags.Initialized);
    }
}