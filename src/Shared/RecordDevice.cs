using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    /// <summary>
    /// Bass Recording Device.
    /// </summary>
    public class RecordDevice
    {
        #region Singleton
        static readonly Dictionary<int, RecordDevice> Singleton = new Dictionary<int, RecordDevice>();

        RecordDevice(int DeviceIndex)
        {
            Index = DeviceIndex;
        }

        /// <summary>
        /// Get Device by Index.
        /// </summary>
        public static RecordDevice GetByIndex(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device];

            DeviceInfo info;
            if (!Bass.RecordGetDeviceInfo(Device, out info))
                throw new ArgumentException("Invalid RecordingDevice Index");

            var dev = new RecordDevice(Device);
            Singleton.Add(Device, dev);

            return dev;
        }
        #endregion

        /// <summary>
        /// Enumerates available Recording Devices
        /// </summary>
        public static IEnumerable<RecordDevice> Devices
        {
            get
            {
                DeviceInfo info;

                for (var i = 0; Bass.RecordGetDeviceInfo(i, out info); ++i)
                    yield return GetByIndex(i);
            }
        }

        public int Index { get; }

        /// <summary>
        /// Number of available Recording Devices
        /// </summary>
        public static int Count => Bass.RecordingDeviceCount;

        /// <summary>
        /// Gets a DeviceInfo object containing information on the Device like Name, Type, IsEnabled, etc.
        /// </summary>
        public DeviceInfo Info => Bass.RecordGetDeviceInfo(Index);

        /// <summary>
        /// Initialize a Device for Recording
        /// </summary>
        public bool Init() => Bass.RecordInit(Index);

        /// <summary>
        /// Frees an initialized Device
        /// </summary>
        public void Dispose()
        {
            Ensure();
            Bass.RecordFree();
        }

        /// <summary>
        /// Default Audio Recording Devices
        /// </summary>
        public static RecordDevice Default => Devices.First(Dev => Dev.Info.IsDefault);

        /// <summary>
        /// Gets or Sets the Recording Device used with the Current thread.
        /// </summary>
        public static RecordDevice Current
        {
            get { return GetByIndex(Bass.CurrentRecordingDevice); }
            set { Bass.CurrentRecordingDevice = value.Index; }
        }
        
        /// <summary>
        /// Gets the current initialized device's info.
        /// </summary>
        public static RecordInfo CurrentInfo => Bass.RecordingInfo;

        void Ensure() => Current = this;

        /// <summary>
        /// Returns the Name of the Device
        /// </summary>
        public override string ToString() => Info.Name;
    }
}