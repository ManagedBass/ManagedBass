using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Playback Device
    /// </summary>
    /// <remarks>All Devices except NoSound and Default need to initialized before use</remarks>
    public class PlaybackDevice : BassDevice
    {
        static Dictionary<int, PlaybackDevice> Singleton = new Dictionary<int, PlaybackDevice>();

        PlaybackDevice() { }

        internal static PlaybackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                var Dev = new PlaybackDevice() { DeviceIndex = Device };
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        /// <summary>
        /// Number of available Playback Devices
        /// </summary>
        public static int Count { get { return Bass.DeviceCount; } }

        /// <summary>
        /// Gets an array of Playback Devices
        /// </summary>
        public static IEnumerable<PlaybackDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (int i = 0; Bass.GetDeviceInfo(i, out info); ++i)
                    if (info.IsEnabled)
                        yield return Get(i);
            }
        }

        public static PlaybackDevice NoSoundDevice { get { return Get(0); } }

        public static PlaybackDevice DefaultDevice { get { return Get(1); } }

        protected override DeviceInfo DeviceInfo { get { return Bass.GetDeviceInfo(DeviceIndex); } }

        public Return<bool> Initialize(int Frequency = 44100) { return Bass.Initialize(DeviceIndex, Frequency, DeviceInitFlags.Default); }

        public override void Dispose() { Bass.Free(DeviceIndex); }

        public double Volume
        {
            get
            {
                Bass.CurrentDevice = DeviceIndex;
                return Bass.Volume;
            }

            set
            {
                Bass.CurrentDevice = DeviceIndex;
                Bass.Volume = value;
            }
        }
    }
}