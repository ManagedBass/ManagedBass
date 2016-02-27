using System;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        #region Music Free
        [DllImport(DllName)]
        extern static bool BASS_MusicFree(int Handle);

        /// <summary>
        /// Frees a MOD music's resources, including any sync/DSP/FX it has.
        /// </summary>
        /// <param name="Handle">The MOD music handle.</param>
        /// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle"/> is not valid.</exception>
        public static bool MusicFree(int Handle) => Checked(BASS_MusicFree(Handle));
        #endregion

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        extern static int BASS_MusicLoad(bool mem, string file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, IntPtr file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, byte[] file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, short[] file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, int[] file, long offset, int Length, BassFlags flags, int freq);

        [DllImport(DllName)]
        extern static int BASS_MusicLoad(bool mem, float[] file, long offset, int Length, BassFlags flags, int freq);

        public static int MusicLoad(string File, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency));
        }

        public static int MusicLoad(IntPtr Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency));
        }

        public static int MusicLoad(byte[] Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency));
        }

        public static int MusicLoad(short[] Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency));
        }

        public static int MusicLoad(int[] Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency));
        }

        public static int MusicLoad(float[] Memory, long Offset, int Length, BassFlags Flags, int Frequency = 44100)
        {
            return Checked(BASS_MusicLoad(true, Memory, Offset, Length, Flags, Frequency));
        }
    }
}
