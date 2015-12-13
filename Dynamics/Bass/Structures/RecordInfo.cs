using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RecordInfo
    {
        RecordInfoFlags Flags;
        RecordFormatFlags Formats;
        int InputCount;
        bool SingleIn;
        int Frequency;
    }
}