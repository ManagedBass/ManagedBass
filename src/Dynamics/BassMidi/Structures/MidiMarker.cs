using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MidiMarker
    {
        int track;
        int pos;
        IntPtr text;

        public string Text { get { return Marshal.PtrToStringAnsi(text); } }

        public int Track { get { return track; } }

        public int Position { get { return pos; } }
    }
}