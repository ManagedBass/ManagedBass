using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiRecordingDevice : WasapiDevice
    {
        static Dictionary<int, WasapiRecordingDevice> Singleton = new Dictionary<int, WasapiRecordingDevice>();

        WasapiRecordingDevice() { }

        static WasapiRecordingDevice Create(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                var Dev = new WasapiRecordingDevice() { DeviceIndex = Device };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public static IEnumerable<WasapiRecordingDevice> Devices
        {
            get
            {
                WasapiRecordingDevice dev;

                for (int i = 0; i < DeviceCount; ++i)
                {
                    dev = Create(i);
                    if (dev.IsInput && !dev.IsLoopback)
                        yield return dev;
                }
            }
        }

        public static WasapiRecordingDevice DefaultDevice { get { return Devices.First(); } }

        public static int DeviceCount
        {
            get
            {
                int Count = 0;

                WasapiRecordingDevice dev;

                for (int i = 0; i < DeviceCount; ++i)
                {
                    dev = Create(i);
                    if (dev.IsInput && !dev.IsLoopback)
                        Count++;
                }

                return Count;
            }
        }
    }
}