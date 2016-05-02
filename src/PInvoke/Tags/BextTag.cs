using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    [StructLayout(LayoutKind.Sequential)]
    public class BextTag
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Description;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Originator;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string OriginatorReference;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string OriginationDate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string OriginationTime;

        public long TimeReference;

        public short Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] UMID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 190)]
        byte[] reserved;
        
        IntPtr codinghistory;

        public string CodingHistory => Marshal.PtrToStringAnsi(codinghistory);

        public static BextTag Read(int Handle)
        {
            return (BextTag)Marshal.PtrToStructure(Bass.ChannelGetTags(Handle, TagType.RiffBext), typeof(BextTag));
        }
    }
}