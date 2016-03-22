using System;

namespace ManagedBass.Enc
{
    public delegate bool EncodeClientProcedure(int handle, bool connect, string client, string header, IntPtr user);
}