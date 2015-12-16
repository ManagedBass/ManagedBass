using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WasapiInfo
    {
        public WasapiInitFlags initflags;
        public int freq;
        public int chans;
        public WasapiFormat format;
        public int buflen;
        public int volmax;
        public int volmin;
        public int volstep;

        public bool IsEventDriven { get { return initflags.HasFlag(WasapiInitFlags.EventDriven); } }
        public bool IsExclusive { get { return initflags.HasFlag(WasapiInitFlags.Exclusive); } }
    }
}