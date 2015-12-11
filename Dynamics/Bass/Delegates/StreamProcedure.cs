using System;

namespace ManagedBass.Dynamics
{
    public delegate int StreamProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);
}