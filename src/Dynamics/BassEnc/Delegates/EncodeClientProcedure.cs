using System;

namespace ManagedBass.Dynamics
{
    public delegate bool EncodeClientProcedure(int handle, bool connect, string client, string header, IntPtr user);
}