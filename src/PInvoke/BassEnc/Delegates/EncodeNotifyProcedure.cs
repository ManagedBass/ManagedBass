using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeNotifyProcedure(int handle, EncodeNotifyStatus status, IntPtr user);
}