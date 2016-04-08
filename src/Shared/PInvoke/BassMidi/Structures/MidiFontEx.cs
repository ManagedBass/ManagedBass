using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiFontEx
    {
        public int font;
        public int spreset;
        public int sbank;
        public int dpreset;
        public int dbank;
        public int dbanklsb;
    }
}