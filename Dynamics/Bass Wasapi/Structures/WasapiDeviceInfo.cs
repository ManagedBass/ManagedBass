using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WasapiDeviceInfo
    {
        IntPtr name;
        public IntPtr id;
        public int type;
        public WasapiDeviceInfoFlags flags;
        public float minperiod;
        public float defperiod;
        public int mixfreq;
        public int mixchans;

        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        public bool IsDefault { get { return flags.HasFlag(WasapiDeviceInfoFlags.Default); } }

        public bool IsEnabled { get { return flags.HasFlag(WasapiDeviceInfoFlags.Enabled); } }

        public bool IsInput { get { return flags.HasFlag(WasapiDeviceInfoFlags.Input); } }

        public bool IsLoopback { get { return flags.HasFlag(WasapiDeviceInfoFlags.Loopback); } }

        public bool IsInitialized { get { return flags.HasFlag(WasapiDeviceInfoFlags.Initialized); } }
    }
}