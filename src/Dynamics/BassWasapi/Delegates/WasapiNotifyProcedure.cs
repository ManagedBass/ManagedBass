using System;

namespace ManagedBass.Dynamics
{
    public delegate void WasapiNotifyProcedure(WasapiNotificationType notify, int device, IntPtr User);
}