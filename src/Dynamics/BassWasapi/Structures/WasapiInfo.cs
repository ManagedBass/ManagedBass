using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WasapiInfo
    {
        WasapiInitFlags initflags;
        int freq;
        int chans;
        WasapiFormat format;
        int buflen;
        int volmax;
        int volmin;
        int volstep;

        public WasapiInitFlags InitFlags { get { return initflags; } }
        public WasapiFormat Format { get { return format; } }

        public int Frequency { get { return freq; } }
        public int Channels { get { return chans; } }
        public int BufferLength { get { return buflen; } }
        public int MaxVolume { get { return volmax; } }
        public int MinVolume { get { return volmin; } }
        public int VolumeStep { get { return volstep; } }

        public bool IsEventDriven { get { return initflags.HasFlag(WasapiInitFlags.EventDriven); } }
        public bool IsExclusive { get { return initflags.HasFlag(WasapiInitFlags.Exclusive); } }
    }
}