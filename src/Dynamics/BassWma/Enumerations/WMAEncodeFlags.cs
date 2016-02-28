using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum WMAEncodeFlags
    {
        Unicode = -2147483648,
        Default = 0,
        Byte = 1,
        Float = 256
    }
}