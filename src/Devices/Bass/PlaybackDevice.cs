using ManagedBass.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Playback Device
    /// </summary>
    /// <remarks>All Devices need to initialized before use</remarks>
    public class PlaybackDevice : IDisposable
    {
        #region Singleton
        static Dictionary<int, PlaybackDevice> Singleton = new Dictionary<int, PlaybackDevice>();

        PlaybackDevice(int DeviceIndex) { this.DeviceIndex = DeviceIndex; }

        public static PlaybackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device)) return Singleton[Device];
            else
            {
                DeviceInfo info;
                if (!Bass.GetDeviceInfo(Device, out info))
                    throw new ArgumentException("Invalid PlaybackDevice Index");

                var Dev = new PlaybackDevice(Device);
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }
        #endregion

        /// <summary>
        /// The Index of the Device as identified by Bass
        /// </summary>
        public int DeviceIndex { get; private set; }

        /// <summary>
        /// Number of available Playback Devices
        /// </summary>
        public static int Count { get { return Bass.DeviceCount; } }

        /// <summary>
        /// Enumerates available Playback Devices
        /// </summary>
        public static IEnumerable<PlaybackDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (int i = 0; Bass.GetDeviceInfo(i, out info); ++i)
                    yield return Get(i);
            }
        }

        /// <summary>
        /// Dummy Device used for Decoding streams
        /// </summary>
        public static PlaybackDevice NoSoundDevice { get { return Get(0); } }

        /// <summary>
        /// Default Audio Playback Device
        /// </summary>
        public static PlaybackDevice DefaultDevice { get { return Devices.First((dev) => dev.DeviceInfo.IsDefault); } }

        /// <summary>
        /// Gets a DeviceInfo object containing information on the Device like Name, Type, IsEnabled, etc.
        /// </summary>
        public DeviceInfo DeviceInfo { get { return Bass.GetDeviceInfo(DeviceIndex); } }

        /// <summary>
        /// Initialize a Device for Playback
        /// </summary>
        /// <param name="Frequency">Frequency, defaults to 44100</param>
        /// <param name="flags">DeviceInitFlags used to specify options to init the device</param>
        /// <returns>A Return&lt;bool&gt; object containing success and error info</returns>
        public Return<bool> Init(int Frequency = 44100, DeviceInitFlags flags = DeviceInitFlags.Default)
        {
            return Bass.Init(DeviceIndex, Frequency, flags);
        }

        public bool Start()
        {
            Bass.CurrentDevice = DeviceIndex;
            return Bass.Start();
        }

        public bool Pause()
        {
            Bass.CurrentDevice = DeviceIndex;
            return Bass.Pause();
        }

        public bool Stop()
        {
            Bass.CurrentDevice = DeviceIndex;
            return Bass.Stop();
        }

        /// <summary>
        /// Frees an initialized Device
        /// </summary>
        public void Dispose() { Bass.Free(DeviceIndex); }

        /// <summary>
        /// Gets or Sets the Device Volume
        /// </summary>
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

        /// <summary>
        /// Returns the Name of the Device
        /// </summary>
        public override string ToString() { return DeviceInfo.Name; }
    }
}