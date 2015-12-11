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

        // Summary:
        //     The device driver has been certified by Microsoft. Always true for WDM drivers.
        public bool IsCertified { get { return Flags.HasFlag(BASSInfoFlags.Certified); } }
        //
        // Summary:
        //     16-bit samples are supported by hardware mixing.
        public bool Supports16BitSamples { get { return Flags.HasFlag(BASSInfoFlags.Secondary16Bit); } }
        //
        // Summary:
        //     8-bit samples are supported by hardware mixing.
        public bool Supports8BitSamples { get { return Flags.HasFlag(BASSInfoFlags.Secondary8Bit); } }
        //
        // Summary:
        //     The device supports all sample rates between minrate and maxrate.
        public bool SupportsContinuousRate { get { return Flags.HasFlag(BASSInfoFlags.ContinuousRate); } }
        //
        // Summary:
        //     The device's drivers has DirectSound support
        public bool SupportsDirectSound { get { return !Flags.HasFlag(BASSInfoFlags.EmulatedDrivers); } }
        //
        // Summary:
        //     Mono samples are supported by hardware mixing.
        public bool SupportsMonoSamples { get { return Flags.HasFlag(BASSInfoFlags.Mono); } }
        //
        // Summary:
        //     Stereo samples are supported by hardware mixing.
        public bool SupportsStereoSamples { get { return Flags.HasFlag(BASSInfoFlags.Stereo); } }
    }
}