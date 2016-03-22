using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeProcedureEx(int handle, int channel, IntPtr buffer, int length, int offset, IntPtr user);
}