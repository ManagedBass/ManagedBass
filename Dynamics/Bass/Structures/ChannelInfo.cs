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
        public ChannelType ChannelType;
        int origres;
        public int Plugin;
        public int Sample;
        IntPtr filename;

        public Resolution Resolution
        {
            get
            {
                if (Flags.HasFlag(BassFlags.Byte)) return Resolution.Byte;
                else if (Flags.HasFlag(BassFlags.Float)) return Resolution.Float;
                else return Resolution.Short;
            }
        }

        public Resolution OriginalResolution
        {
            get
            {
                switch (origres)
                {
                    case 8:
                        return Resolution.Byte;

                    case 32:
                        return Resolution.Float;

                    default:
                    case 16:
                        return Resolution.Short;
                }
            }
        }

        public string FileName { get { return Marshal.PtrToStringAnsi(filename); } }
    }
}