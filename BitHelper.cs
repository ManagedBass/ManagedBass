// Adopted from http://www.codeproject.com/Articles/12919/C-Bitwise-Helper-Class
using System;

namespace ManagedBass
{
    /// <summary>
    /// Helps perform certain operations on primative types that deal with bits
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// The return value is the high-order double word of the specified value.
        /// </summary>
        public static int HiDword(this long pDWord) { return ((int)(((pDWord) >> 32) & 0xFFFFFFFF)); }

        /// <summary>
        /// The return value is the low-order word of the specified value.
        /// </summary>
        public static int LoDword(this long pDWord) { return ((int)pDWord); }

        /// <summary>
        /// The return value is the high-order word of the specified value.
        /// </summary>
        public static short HiWord(this int pDWord) { return ((short)(((pDWord) >> 16) & 0xFFFF)); }

        /// <summary>
        /// The return value is the low-order word of the specified value.
        /// </summary>
        public static short LoWord(this int pDWord) { return ((short)pDWord); }

        /// <summary>
        /// The return value is the high-order byte of the specified value.
        /// </summary>
        public static byte HiByte(this short pWord) { return ((byte)(((short)(pWord) >> 8) & 0xFF)); }

        /// <summary>
        /// The return value is the low-order byte of the specified value.
        /// </summary>
        public static byte LoByte(this short pWord) { return ((byte)pWord); }

        /// <summary>
        /// Make an integer putting <paramref name="low"/> in low 2-bits and <paramref name="high"/> in high 2-bits.
        /// </summary>
        public static int MakeLong(short low, short high)
        {
            return (int)(((ushort)low) | (uint)(high << 16));
        }
    }
}
