using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiDeviceInfo
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