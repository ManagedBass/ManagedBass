#if WINDOWS || LINUX
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Cd
{
    /// <summary>
	/// Represents one track of a CD's TOC (see <see cref="TOC" />).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public struct TOCTrack
    {
        byte res1;
        byte adrcon;
        byte track;
        byte res2;
        int lba;

        public byte ADR => (byte)(adrcon >> 4 & 15);

        public TOCControlFlags Control => (TOCControlFlags)(byte)(adrcon & 15);

		/// <summary>
		/// The track number... 170 = lead-out area (or index number if <see cref="TOCMode.Index" /> is used).
		/// </summary>
        public byte Track => track;

        public int LBA => lba;

        public byte Frame => (byte)(lba & 15);

        public TimeSpan Address => new TimeSpan(lba >> 24 & 15, lba >> 16 & 15, lba >> 8 & 15);
    }
}
#endif