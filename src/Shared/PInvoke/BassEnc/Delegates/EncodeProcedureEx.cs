using System;

namespace ManagedBass.Enc
{
    public delegate void EncodeProcedureEx(int Handle, int Channel, IntPtr Buffer, int Length, int Offset, IntPtr User);
}