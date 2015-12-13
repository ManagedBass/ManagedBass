using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BassInfo
    {
        public BASSInfoFlags Flags;
        public int hwsize;
        public int hwfree;
        public int freesam;
        public int free3d;
        public int MinRate;
        public int MaxRate;
        public bool EAX;
        public int MinBuffer;
        public int DSVersion;
        public int Latency;
        public DeviceInitFlags InitFlags;
        public int Speakers;
        public int Frequency;

        /// <summary>
        /// The device driver has been certified by Microsoft. Always true for WDM drivers.
        /// </summary>
        public bool IsCertified { get { return Flags.HasFlag(BASSInfoFlags.Certified); } }
        
        /// <summary>
        /// 16-bit samples are supported by hardware mixing.
        /// </summary>
        public bool Supports16BitSamples { get { return Flags.HasFlag(BASSInfoFlags.Secondary16Bit); } }
        
        /// <summary>
        /// 8-bit samples are supported by hardware mixing.
        /// </summary>
        public bool Supports8BitSamples { get { return Flags.HasFlag(BASSInfoFlags.Secondary8Bit); } }
        
        /// <summary>
        /// The device supports all sample rates between minrate and maxrate.
        /// </summary>
        public bool SupportsContinuousRate { get { return Flags.HasFlag(BASSInfoFlags.ContinuousRate); } }
        
        /// <summary>
        /// The device's drivers has DirectSound support
        /// </summary>
        public bool SupportsDirectSound { get { return !Flags.HasFlag(BASSInfoFlags.EmulatedDrivers); } }
        
        /// <summary>
        /// Mono samples are supported by hardware mixing.
        /// </summary>
        public bool SupportsMonoSamples { get { return Flags.HasFlag(BASSInfoFlags.Mono); } }
        
        /// <summary>
        /// Stereo samples are supported by hardware mixing.
        /// </summary>
        public bool SupportsStereoSamples { get { return Flags.HasFlag(BASSInfoFlags.Stereo); } }
    }
}