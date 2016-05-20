using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass.Wasapi
{
    public class WasapiLoopbackDevice : WasapiDevice
    {
        WasapiLoopbackDevice(int Index) : base(Index) { }

        public static WasapiLoopbackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device] as WasapiLoopbackDevice;

            WasapiDeviceInfo info;
            if (!(BassWasapi.GetDeviceInfo(Device, out info) && info.IsLoopback))
                throw new ArgumentException("Invalid WasapiLoopbackDevice Index");

            var dev = new WasapiLoopbackDevice(Device);
            Singleton.Add(Device, dev);

            return dev;
        }

        public bool Init() => _Init(0, 0, true, false, 0, 0);

        public static IEnumerable<WasapiLoopbackDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (var i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsLoopback)
                        yield return Get(i);
            }
        }

        public static WasapiLoopbackDevice Default => Devices.First(Dev => Dev.Info.IsDefault);

        public static int Count
        {
            get
            {
                var count = 0;

                WasapiDeviceInfo dev;

                for (var i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsLoopback)
                        count++;

                return count;
            }
        }

        public WasapiPlaybackDevice PlaybackDevice
        {
            get
            {
                foreach (var dev in WasapiPlaybackDevice.Devices.Where(dev => dev.Info.ID == Info.ID))
                    return dev;

                throw new Exception("Could not find a Playback Device.");
            }
        }
    }
}