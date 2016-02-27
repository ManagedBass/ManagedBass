using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public class WasapiInfo
    {
        WasapiInitFlags initflags;
        int freq;
        int chans;
        WasapiFormat format;
        int buflen;
        int volmax;
        int volmin;
        int volstep;

        public WasapiInitFlags InitFlags => initflags;
        public WasapiFormat Format => format;

        public int Frequency => freq;
        public int Channels => chans;
        public int BufferLength => buflen;
        public int MaxVolume => volmax;
        public int MinVolume => volmin;
        public int VolumeStep => volstep;

        public bool IsEventDriven => initflags.HasFlag(WasapiInitFlags.EventDriven);
        public bool IsExclusive => initflags.HasFlag(WasapiInitFlags.Exclusive);
    }
}