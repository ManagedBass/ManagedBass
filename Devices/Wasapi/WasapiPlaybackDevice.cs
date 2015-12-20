using ManagedBass.Dynamics;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiPlaybackDevice : WasapiDevice
    {
        static Dictionary<int, WasapiPlaybackDevice> Singleton = new Dictionary<int, WasapiPlaybackDevice>();

        WasapiPlaybackDevice() { }

        static WasapiPlaybackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                var Dev = new WasapiPlaybackDevice() { DeviceIndex = Device };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public static IEnumerable<WasapiPlaybackDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (!dev.IsInput)
                        yield return Get(i);
            }
        }

        public static WasapiPlaybackDevice DefaultDevice { get { return Devices.First(); } }

        public static int DeviceCount
        {
            get
            {
                int Count = 0;

                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (!dev.IsInput)
                        Count++;

                return Count;
            }
        }
    }
}