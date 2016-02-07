using System;

namespace ManagedBass.Dynamics
{
    public delegate void ClientConnectProcedure(int handle, bool connect, string ip, IntPtr user);
}