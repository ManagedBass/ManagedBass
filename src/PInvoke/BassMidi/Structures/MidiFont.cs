using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiFont
    {
        public int Handle;
        public int Preset;
        public int Bank;
    }
}