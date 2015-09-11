using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiEvent
    {
        public int _event;
        public int param;
        public int chan;
        public int tick;
        public int pos;
    }
}