using System;

namespace ManagedBass.Dynamics
{
    public delegate void WMEncodeProcedure(int handle, int type, IntPtr buffer, int length, IntPtr user);
}