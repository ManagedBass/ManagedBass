using ManagedBass.Dynamics;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public static class Extensions
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string dllToLoad);

        public static BassFlags ToBassFlag(this BufferKind BufferKind)
        {
            switch (BufferKind)
            {
                case BufferKind.Byte:
                    return BassFlags.Byte;
                case BufferKind.Float:
                    return BassFlags.Float;
                default:
                    return BassFlags.Default;
            }
        }

        internal static void Load(string DllName, string Folder)
        {
            if (!string.IsNullOrWhiteSpace(Folder))
                LoadLibrary(Path.Combine(Folder, DllName));
        }

        public static short HiWord(this int pDWord) { return ((short)(((pDWord) >> 16) & 0xFFFF)); }

        public static short LoWord(this int pDWord) { return ((short)pDWord); }
    }
}
