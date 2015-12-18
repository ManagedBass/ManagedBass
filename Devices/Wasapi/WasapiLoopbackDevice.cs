using ManagedBass.Dynamics;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiLoopbackDevice : WasapiDevice
    {
        static Dictionary<int, WasapiLoopbackDevice> Singleton = new Dictionary<int, WasapiLoopbackDevice>();

        WasapiLoopbackDevice() { }

        static WasapiLoopbackDevice Create(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                var Dev = new WasapiLoopbackDevice() { DeviceIndex = Device };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public static IEnumerable<WasapiLoopbackDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsLoopback)
                        yield return Create(i);
            }
        }

        public static WasapiLoopbackDevice DefaultDevice { get { return Devices.First(); } }

        public static int DeviceCount
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