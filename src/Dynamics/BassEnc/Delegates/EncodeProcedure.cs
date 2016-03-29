using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeProcedure(int Handle, int Channel, IntPtr Buffer, int Length, IntPtr User);
}