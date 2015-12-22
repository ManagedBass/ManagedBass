using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiFont
    {
        public int font;
        public int present;
        public int bank;
    }
}