using System;

namespace ManagedBass.Midi
{
    public delegate void MidiInProcedure(int device, double time, IntPtr buffer, int length, IntPtr user);
}