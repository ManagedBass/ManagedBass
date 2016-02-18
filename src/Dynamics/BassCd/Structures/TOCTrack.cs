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

        public byte ADR => (byte)(adrcon >> 4 & 15);

        public TOCControlFlags Control => (TOCControlFlags)((byte)(adrcon & 15));

        public byte Track => track;

        public int LBA => lba;

        public byte Frame => (byte)(lba & 15);

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