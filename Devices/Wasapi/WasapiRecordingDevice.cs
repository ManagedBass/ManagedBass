using ManagedBass.Dynamics;
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
                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsInput && !dev.IsLoopback)
                        yield return Create(i);
            }
        }

        public static WasapiRecordingDevice DefaultDevice { get { return Devices.First(); } }

        public static int DeviceCount
        {
            get
            {
                int Count = 0;

                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsInput && !dev.IsLoopback)
                        Count++;

                return Count;
            }
        }
    }
}