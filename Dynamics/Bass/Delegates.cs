using System;

namespace ManagedBass.Dynamics
{
    public delegate bool RecordProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);

    public delegate void DownloadProcedure(IntPtr Buffer, int Length, IntPtr User);

    public delegate void SyncProcedure(int Handle, int Channel, int Data, IntPtr User);

    public delegate int StreamProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);

    public delegate void DSPProcedure(int Handle, int Channel, IntPtr Buffer, int Length, IntPtr User);
}