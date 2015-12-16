using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CDInfo
    {
        IntPtr vendor;
        IntPtr product;
        public IntPtr rev;
        public int letter;
        public CDReadFlags rwflags;
        public bool canopen;
        public bool canlock;
        public int maxspeed;
        public int cache;
        public bool cdtext;

        public string Name { get { return Marshal.PtrToStringAnsi(product); } }

        public string Manufacturer { get { return Marshal.PtrToStringAnsi(vendor); } }

        public int SpeedMultiplier { get { return (int)(maxspeed / 176.4); } }

        public char DriveLetter { get { return char.ToUpper((char)(letter + 63)); } }
    }

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

    [StructLayout(LayoutKind.Sequential)]
    public struct TOC
    {
        short size;
        byte first;
        byte last;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        TOCTrack[] tracks;
    }
}