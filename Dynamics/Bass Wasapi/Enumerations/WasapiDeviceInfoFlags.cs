using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum WasapiDeviceInfoFlags
    {
        Unknown = 0,
        Enabled = 1,
        Default = 2,
        Initialized = 4,
        Loopback = 8,
        Input = 16,
        Unplugged = 32,
        Disabled = 64,
    }
}