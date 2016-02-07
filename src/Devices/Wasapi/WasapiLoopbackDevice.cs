using ManagedBass.Dynamics;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiLoopbackDevice : WasapiDevice
    {
        WasapiLoopbackDevice(int Index) : base(Index) { }

        static WasapiLoopbackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device] as WasapiLoopbackDevice;
            else
            {
                var Dev = new WasapiLoopbackDevice(Device);
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public bool Init() { return base._Init(0, 0, true, false, 0, 0); }

        public static IEnumerable<WasapiLoopbackDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsLoopback)
                        yield return Get(i);
            }
        }

        public static WasapiLoopbackDevice DefaultDevice { get { return Devices.First((dev) => dev.DeviceInfo.IsDefault); } }

        public static int Count
        {
            get
            {
                int Count = 0;

                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsLoopback)
                        Count++;

                return Count;
            }
        }
    }
}
