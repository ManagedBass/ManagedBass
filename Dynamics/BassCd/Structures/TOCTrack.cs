using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TOCTrack
    {
        byte res1;
        byte adrcon;
        byte track;
        byte res2;
        int lba;

        public byte ADR { get { return (byte)(adrcon >> 4 & 15); } }

        public TOCControlFlags Control
        {
            get { return (TOCControlFlags)((byte)(adrcon & 15)); }
        }

        public byte Track { get { return track; } }

        public int LBA { get { return lba; } }

        public byte Frame { get { return (byte)(lba & 15); } }

        public TimeSpan Address
        {
            get
            {
                return new TimeSpan(lba >> 24 & 15,
                                    lba >> 16 & 15,
                                    lba >> 8 & 15);
            }
        }
    }
}