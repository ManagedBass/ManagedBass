using System;

namespace ManagedBass.Dynamics
{
    public delegate void MidiInProcedure(int device, double time, IntPtr buffer, int length, IntPtr user);
}