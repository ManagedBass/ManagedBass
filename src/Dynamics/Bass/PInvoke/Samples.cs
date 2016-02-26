using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_SampleGetChannel")]
        public static extern int SampleGetChannel(int sample, bool onlynew);

        [DllImport(DllName, EntryPoint = "BASS_SampleFree")]
        public static extern bool SampleFree(int Handle);

        #region Sample Set Data
        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, IntPtr Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, byte[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, int[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, short[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, float[] Buffer);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_SampleCreate")]
        public static extern int CreateSample(int Length, int freq, int chans, int max, BassFlags flags);

        #region Sample Get Data
        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, IntPtr Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, [In, Out] byte[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, [In, Out] short[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, [In, Out] int[] Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, [In, Out] float[] Buffer);
        #endregion

        /// <summary>
        /// Retrieves a sample's default attributes and other information.
        /// </summary>
        /// <param name="Handle">The sample handle.</param>
        /// <param name="Info">An instance of the <see cref="SampleInfo" /> class to store the sample information at.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_SampleGetInfo")]
        public static extern bool SampleGetInfo(int Handle, ref SampleInfo Info);

        public static SampleInfo SampleGetInfo(int Handle)
        {
            SampleInfo temp = new SampleInfo();
            SampleGetInfo(Handle, ref temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_SampleSetInfo")]
        public static extern bool SampleSetInfo(int Handle, SampleInfo info);

        [DllImport(DllName)]
        static extern int BASS_SampleGetChannels(int handle, [In, Out] int[] channels);

        public static int[] SampleGetChannels(int handle)
        {
            var channels = new int[SampleGetInfo(handle).Max];

            BASS_SampleGetChannels(handle, channels);

            return channels;
        }

        /// <summary>
        /// Stops all instances of a sample.
        /// </summary>
        /// <param name="Handle">The sample handle.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not a valid sample handle.</exception>
        /// <remarks>If a sample is playing simultaneously multiple times, calling this function will stop them all, which is obviously simpler than calling <see cref="ChannelStop" /> multiple times.</remarks>
        [DllImport(DllName, EntryPoint = "BASS_SampleStop")]
        public static extern bool SampleStop(int Handle);

        #region Sample Load
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_SampleLoad(bool mem, string file, long offset, int Length, int max, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_SampleLoad(bool mem, IntPtr file, long offset, int Length, int max, BassFlags flags);

        public static int SampleLoad(string File, long Offset, int Length, int MaxNoOfPlaybacks, BassFlags Flags)
        {
            return BASS_SampleLoad(false, File, Offset, Length, MaxNoOfPlaybacks, Flags | BassFlags.Unicode);
        }

        public static int SampleLoad(IntPtr Memory, long Offset, int Length, int MaxNoOfPlaybacks, BassFlags Flags)
        {
            return BASS_SampleLoad(true, Memory, Offset, Length, MaxNoOfPlaybacks, Flags);
        }
        #endregion
    }
}
