using ManagedBass.Dynamics;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    public class WasapiRecordingDevice : WasapiDevice
    {
        WasapiRecordingDevice(int Index) : base(Index) { }

        static WasapiRecordingDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device] as WasapiRecordingDevice;
            else
            {
                var Dev = new WasapiRecordingDevice(Device);
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
                        yield return Get(i);
            }
        }

        public bool Init(int Frequency = 44100, int Channels = 2, bool Shared = true, bool UseEventSync = false, int Buffer = 0, int Period = 0)
        {
            return base._Init(Frequency, Channels, Shared, UseEventSync, Buffer, Period);
        }

        public static WasapiRecordingDevice DefaultDevice { get { return Devices.First((dev) => dev.DeviceInfo.IsDefault); } }

        public static int Count
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
