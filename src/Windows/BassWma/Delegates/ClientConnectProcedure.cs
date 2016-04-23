using System;

namespace ManagedBass.Wma
{
    public delegate void ClientConnectProcedure(int handle, bool connect, string ip, IntPtr user);
}