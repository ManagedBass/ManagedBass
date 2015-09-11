using System;

namespace ManagedBass.Dynamics
{
    public delegate void BPMBeatProcedure(int chan, double beatpos, IntPtr user);
}