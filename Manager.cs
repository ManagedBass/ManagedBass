using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    static class BassManager
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

        public static void Load(string DllName)
        {
            //LoadLibrary(DllName);
        }

        public static short HiWord(this int pDWord) { return ((short)(((pDWord) >> 16) & 0xFFFF)); }

        public static short LoWord(this int pDWord) { return ((short)pDWord); }

        static BassManager() { LoadBass(); }

        public static void LoadBass()
        {
            Load("bass.dll");

            //Bass.BASS_PluginLoadDirectory(Bin);

            PlaybackDevice.NoSoundDevice.Initialize();
            PlaybackDevice.DefaultDevice.Initialize();

            Bass.FloatingPointDSP = true;
        }
    }
}
