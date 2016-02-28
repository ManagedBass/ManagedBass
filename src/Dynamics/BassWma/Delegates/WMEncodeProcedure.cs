using System;

namespace ManagedBass.Dynamics
{
    public delegate void WMEncodeProcedure(int handle, WMEncodeType type, IntPtr buffer, int length, IntPtr user);
}