using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeNotifyProcedure(int Handle, EncodeNotifyStatus Status, IntPtr User);
}