using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiDeviceInfo
    {
        IntPtr name;
        public int id;
        DeviceInfoFlags flags;

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public bool IsEnabled { get { return flags.HasFlag(DeviceInfoFlags.Enabled); } }

        public bool IsInitialized { get { return flags.HasFlag(DeviceInfoFlags.Initialized); } }
    }
}