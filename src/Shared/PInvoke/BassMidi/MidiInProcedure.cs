#if !__ANDROID__
using System;

namespace ManagedBass.Midi
{
    public delegate void MidiInProcedure(int Device, double Time, IntPtr Buffer, int Length, IntPtr User);
}
#endif