using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChannelInfo
    {
        public int Frequency;
        public int Channels;
        public BassFlags Flags;
        public ChannelTypeFlags ChannelType;
        public int OriginalResolution;
        public int Plugin;
        public int Sample;
        IntPtr filename;

        public string FileName { get { return Marshal.PtrToStringAnsi(filename); } }
    }
}