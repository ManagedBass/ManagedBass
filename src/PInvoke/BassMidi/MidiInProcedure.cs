#if __IOS__ || WINDOWS || LINUX || __MAC__
using System;

namespace ManagedBass.Midi
{
    public delegate void MidiInProcedure(int Device, double Time, IntPtr Buffer, int Length, IntPtr User);
}
#endif