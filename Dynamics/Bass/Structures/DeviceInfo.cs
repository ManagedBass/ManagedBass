using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceInfo
    {
        IntPtr name;
        IntPtr driver;
        DeviceInfoFlags flags;

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public string Driver { get { return Marshal.PtrToStringAnsi(driver); } }

        public bool IsDefault { get { return flags.HasFlag(DeviceInfoFlags.Default); } }

        public bool IsEnabled { get { return flags.HasFlag(DeviceInfoFlags.Enabled); } }

        public bool IsInitialized { get { return flags.HasFlag(DeviceInfoFlags.Initialized); } }

        public DeviceInfoFlags Flags { get { return flags; } }
    }
}