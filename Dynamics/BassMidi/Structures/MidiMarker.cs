using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiMarker
    {
        public int track;
        public int pos;
        IntPtr text;

        public string Text { get { return Marshal.PtrToStringAnsi(text); } }
    }
}