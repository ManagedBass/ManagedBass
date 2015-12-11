using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Playback Device
    /// </summary>
    /// <remarks>All Devices except NoSound and Default need to initialized before use</remarks>
    public class PlaybackDevice
    {
        /// <summary>
        /// Number of available Playback Devices
        /// </summary>
        public static int DeviceCount { get { return Bass.DeviceCount; } }

        /// <summary>
        /// Gets an array of Playback Devices
        /// </summary>
        public static PlaybackDevice[] Devices
        {
            get
            {
                int NoOfDevices = DeviceCount;

                PlaybackDevice[] Devices = new PlaybackDevice[NoOfDevices];

                for (int i = 0; i < NoOfDevices; ++i) Devices[i] = new PlaybackDevice(i);

                return Devices;
            }
        }

        public static PlaybackDevice NoSoundDevice { get { return new PlaybackDevice(0); } }

        public static PlaybackDevice DefaultDevice { get { return new PlaybackDevice(1); } }

        DeviceInfo DeviceInfo { get { return Bass.DeviceInfo(DeviceId); } }

        public int DeviceId { get; protected set; }

        public string Name { get { return DeviceInfo.Name; } }

        public bool IsEnabled { get { return DeviceInfo.IsEnabled; } }

        public bool IsDefault { get { return DeviceInfo.IsDefault; } }

        public bool IsInitialized { get { return DeviceInfo.IsInitialized; } }

        internal PlaybackDevice(int DeviceId) { this.DeviceId = DeviceId; }

        public Return<bool> Initialize(int Frequency = 44100) { return Bass.Initialize(DeviceId, Frequency, DeviceInitFlags.Default); }

        public Return<bool> Free() { return Bass.Free(DeviceId); }

        public double Volume
        {
            get
            {
                Bass.CurrentDevice = DeviceId;
                return Bass.Volume;
            }

            set
            {
                Bass.CurrentDevice = DeviceId;
                Bass.Volume = value;
            }
        }
    }
}