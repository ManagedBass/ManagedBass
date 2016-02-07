using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiDeviceInfo
    {
        IntPtr name;
        int id;
        DeviceInfoFlags flags;

        public int ID { get { return id; } }

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public bool IsEnabled { get { return flags.HasFlag(DeviceInfoFlags.Enabled); } }

        public bool IsInitialized { get { return flags.HasFlag(DeviceInfoFlags.Initialized); } }
    }
}