using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceInfo
    {
        IntPtr name;
        IntPtr driver;
        public DeviceInfoFlags Flags;

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public string Driver { get { return Marshal.PtrToStringAnsi(driver); } }

        public bool IsDefault { get { return Flags.HasFlag(DeviceInfoFlags.Enabled); } }

        public bool IsEnabled { get { return Flags.HasFlag(DeviceInfoFlags.Default); } }

        public bool IsInitialized { get { return Flags.HasFlag(DeviceInfoFlags.Initialized); } }
    }
}