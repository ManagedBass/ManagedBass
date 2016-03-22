using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeProcedure(int handle, int channel, IntPtr buffer, int length, IntPtr user);
}