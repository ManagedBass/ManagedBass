using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Represents a Wasapi Recording Device.
    /// </summary>
    public class WasapiRecordingDevice : WasapiDevice
    {
        WasapiRecordingDevice(int Index) : base(Index) { }

        /// <summary>
        /// Get Device By Index.
        /// </summary>
        public static WasapiRecordingDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device] as WasapiRecordingDevice;

            WasapiDeviceInfo info;
            if (!(BassWasapi.GetDeviceInfo(Device, out info) && !info.IsLoopback && info.IsInput))
                throw new ArgumentException("Invalid WasapiRecordingDevice Index");

            var dev = new WasapiRecordingDevice(Device);
            Singleton.Add(Device, dev);

            return dev;
        }

        /// <summary>
        /// Enumerates <see cref="WasapiRecordingDevice"/>s.
        /// </summary>
        public static IEnumerable<WasapiRecordingDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (var i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsInput && !dev.IsLoopback)
                        yield return Get(i);
            }
        }

        /// <summary>
        /// Initialises the device.
        /// </summary>
        public bool Init(int Frequency = 44100, int Channels = 2, bool Shared = true, bool UseEventSync = false, int Buffer = 0, int Period = 0)
        {
            return _Init(Frequency, Channels, Shared, UseEventSync, Buffer, Period);
        }

        /// <summary>
        /// Gets the Default Recording Device.
        /// </summary>
        public static WasapiRecordingDevice Default => Devices.First(Dev => Dev.Info.IsDefault);

        /// <summary>
        /// Gets the number of Recording Devices.
        /// </summary>
        public static int Count
        {
            get
            {
                var count = 0;

                WasapiDeviceInfo dev;

                for (var i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (dev.IsInput && !dev.IsLoopback)
                        count++;

                return count;
            }
        }
    }
}