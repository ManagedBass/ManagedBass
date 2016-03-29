// Adopted from http://www.codeproject.com/Articles/12919/C-Bitwise-Helper-Class

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
        public static int HiDword(this long pDWord) => (int)((pDWord >> 32) & 0xFFFFFFFF);

        /// <summary>
        /// The return value is the low-order word of the specified value.
        /// </summary>
        public static int LoDword(this long pDWord) => (int)pDWord;

        /// <summary>
        /// The return value is the high-order word of the specified value.
        /// </summary>
        public static short HiWord(this int pDWord) => (short)((pDWord >> 16) & 0xFFFF);

        /// <summary>
        /// The return value is the low-order word of the specified value.
        /// </summary>
        public static short LoWord(this int pDWord) => (short)pDWord;

        /// <summary>
        /// The return value is the high-order byte of the specified value.
        /// </summary>
        public static byte HiByte(this short pWord) => (byte)((pWord >> 8) & 0xFF);

        /// <summary>
        /// The return value is the low-order byte of the specified value.
        /// </summary>
        public static byte LoByte(this short pWord) => (byte)pWord;

        /// <summary>
        /// Make an short from 2-bytes.
        /// </summary>
        public static short MakeWord(byte low, byte high) => (short)(low | (ushort)(high << 8));

        /// <summary>
        /// Make an integer putting <paramref name="low"/> in low 2-bytes and <paramref name="high"/> in high 2-bytes.
        /// </summary>
        public static int MakeLong(short low, short high) => (int)((ushort)low | (uint)(high << 16));
    }
}
