using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct TOCTrack
    {
        [FieldOffset(0)]
        byte res1;
        [FieldOffset(1)]
        byte adrcon;
        [FieldOffset(2)]
        byte track;
        [FieldOffset(3)]
        byte res2;
        [FieldOffset(4)]
        int lba;
        [FieldOffset(4), MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] hmsf;
    }
}