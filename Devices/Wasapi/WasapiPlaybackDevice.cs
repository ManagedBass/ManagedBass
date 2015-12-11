using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiPlaybackDevice : WasapiDevice
    {
        static Dictionary<int, WasapiPlaybackDevice> Singleton = new Dictionary<int, WasapiPlaybackDevice>();

        WasapiPlaybackDevice() { }

        static WasapiPlaybackDevice Create(int Device)
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
                WasapiPlaybackDevice dev = null;

                for (int i = 0; i < DeviceCount; ++i)
                {
                    dev = Create(i);
                    if (!dev.IsInput)
                        yield return dev;
                }
            }
        }

        public static WasapiPlaybackDevice DefaultDevice { get { return Devices.First(); } }

        public static int DeviceCount
        {
            get
            {
                int Count = 0;

                WasapiPlaybackDevice dev;

                for (int i = 0; i < DeviceCount; ++i)
                {
                    dev = Create(i);
                    if (!dev.IsInput)
                        Count++;
                }

                return Count;
            }
        }
    }
}