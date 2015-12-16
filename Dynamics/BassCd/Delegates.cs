using System;

namespace ManagedBass.Dynamics
{
    public delegate void CDDataProcedure(int handle, int pos, int type, IntPtr buffer, int length, IntPtr user);
}