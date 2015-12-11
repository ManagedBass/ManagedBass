using System;

namespace ManagedBass.Dynamics
{
    public delegate bool RecordProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);
}