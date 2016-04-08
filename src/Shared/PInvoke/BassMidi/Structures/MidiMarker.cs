using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiMarker
    {
        int track;
        int pos;
        IntPtr text;

        public string Text => Marshal.PtrToStringAnsi(text);

        public int Track => track;

        public int Position => pos;
    }
}