using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    /// <summary>
    /// Bass Playback Device.
    /// </summary>
    public class PlaybackDevice : IDisposable
    {
        #region Singleton
        static readonly Dictionary<int, PlaybackDevice> Singleton = new Dictionary<int, PlaybackDevice>();

        PlaybackDevice(int DeviceIndex)
        {
            Index = DeviceIndex;
        }

        /// <summary>
        /// Get Device by Index.
        /// </summary>
        public static PlaybackDevice GetByIndex(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device];

            DeviceInfo info;
            if (!Bass.GetDeviceInfo(Device, out info))
                throw new ArgumentOutOfRangeException(nameof(Device), "Invalid PlaybackDevice Index");

            var dev = new PlaybackDevice(Device);
            Singleton.Add(Device, dev);

            return dev;
        }
        #endregion
        
        public int Index { get; }

        #region Static Properties
        /// <summary>
        /// Number of available Playback Devices.
        /// </summary>
        public static int Count => Bass.DeviceCount;

        /// <summary>
        /// No Sound Device.
        /// </summary>
        public static PlaybackDevice NoSound => GetByIndex(0);

        /// <summary>
        /// Default Audio Playback Device.
        /// </summary>
        public static PlaybackDevice Default => Devices.First(Dev => Dev.Info.IsDefault);
        
        /// <summary>
        /// Gets or Sets the Device used on the Current thread.
        /// </summary>
        public static PlaybackDevice Current
        {
            get { return GetByIndex(Bass.CurrentDevice); }
            set { Bass.CurrentDevice = value.Index; }
        }

        /// <summary>
        /// Gets the current initialized device's info.
        /// </summary>
        public static BassInfo CurrentInfo => Bass.Info;

        /// <summary>
        /// Enumerates available Playback Devices.
        /// </summary>
        public static IEnumerable<PlaybackDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (var i = 0; Bass.GetDeviceInfo(i, out info); ++i)
                    yield return GetByIndex(i);
            }
        }
        #endregion

        /// <summary>
        /// Gets a DeviceInfo object containing information on the Device like Name, Type, IsEnabled, etc.
        /// </summary>
        public DeviceInfo Info => Bass.GetDeviceInfo(Index);

        /// <summary>
        /// Initialize a Device for Playback
        /// </summary>
        public bool Init(int Frequency = 44100, DeviceInitFlags Flags = DeviceInitFlags.Default)
        {
            return Bass.Init(Index, Frequency, Flags);
        }

        /// <summary>
        /// Start Output.
        /// </summary>
        public bool Start()
        {
            Ensure();
            return Bass.Start();
        }

        /// <summary>
        /// Pause Output.
        /// </summary>
        public bool Pause()
        {
            Ensure();
            return Bass.Pause();
        }

        /// <summary>
        /// Stop Output.
        /// </summary>
        public bool Stop()
        {
            Ensure();
            return Bass.Stop();
        }

        /// <summary>
        /// Frees an initialized Device
        /// </summary>
        public void Dispose()
        {
            Ensure();
            Bass.Free();
        }

        /// <summary>
        /// Gets or Sets the Device Volume
        /// </summary>
        public double Volume
        {
            get
            {
                Ensure();
                return Bass.Volume;
            }
            set
            {
                Ensure();
                Bass.Volume = value;
            }
        }

        void Ensure() => Current = this;

        /// <summary>
        /// Returns the Name of the Device
        /// </summary>
        public override string ToString() => Info.Name;
    }
}