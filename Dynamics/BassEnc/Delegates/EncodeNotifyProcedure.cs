using System;

namespace ManagedBass.Dynamics
{
    public delegate void EncodeNotifyProcedure(int handle, EncodeNotifyStatus status, IntPtr user);
}