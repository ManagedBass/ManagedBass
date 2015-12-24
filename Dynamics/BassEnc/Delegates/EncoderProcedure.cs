using System;

namespace ManagedBass.Dynamics
{
    public delegate int EncoderProcedure(int handle, int channel, IntPtr buffer, int length, int maxout, IntPtr user);
}