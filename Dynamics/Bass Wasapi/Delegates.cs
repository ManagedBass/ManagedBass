using System;

namespace ManagedBass.Dynamics
{
    public delegate void WasapiNotifyProcedure(WasapiNotificationType notify, int device, IntPtr User);

    public delegate int WasapiProcedure(IntPtr Buffer, int Length, IntPtr User);
}