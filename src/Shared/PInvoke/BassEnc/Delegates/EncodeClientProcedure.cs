using System;

namespace ManagedBass.Enc
{
    public delegate bool EncodeClientProcedure(int Handle, bool Connect, string Client, string Header, IntPtr User);
}