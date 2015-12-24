using System;

namespace ManagedBass.Dynamics
{
    public delegate void EncodeProcedureEx(int handle, int channel, IntPtr buffer, int length, int offset, IntPtr user);
}