#if WINDOWS
using System;

namespace ManagedBass.Wasapi
{
    public delegate void WasapiNotifyProcedure(WasapiNotificationType notify, int device, IntPtr User);
}
#endif