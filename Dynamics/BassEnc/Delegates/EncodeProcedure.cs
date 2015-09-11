using System;

namespace ManagedBass.Dynamics
{
    public delegate void EncodeProcedure(int handle, int channel, IntPtr buffer, int length, IntPtr user);
}