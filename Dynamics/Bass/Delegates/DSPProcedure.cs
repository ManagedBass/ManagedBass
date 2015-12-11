using System;

namespace ManagedBass.Dynamics
{
    public delegate void DSPProcedure(int Handle, int Channel, IntPtr Buffer, int Length, IntPtr User);
}