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

        [DllImport(DllName, EntryPoint = "BASS_SampleSetData")]
        public static extern bool SampleSetData(int Handle, IntPtr Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleCreate")]
        public static extern int CreateSample(int Length, int freq, int chans, int max, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetData")]
        public static extern bool SampleGetData(int Handle, IntPtr Buffer);

        [DllImport(DllName, EntryPoint = "BASS_SampleGetInfo")]
        public static extern bool SampleGetInfo(int Handle, ref SampleInfo info);

        public static SampleInfo SampleGetInfo(int Handle)
        {
            SampleInfo temp = new SampleInfo();
            SampleGetInfo(Handle, ref temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_SampleSetInfo")]
        public static extern bool SampleSetInfo(int Handle, SampleInfo info);

        [DllImport(DllName)]
        static extern int BASS_SampleGetChannels(int handle, [MarshalAs(UnmanagedType.LPArray)] int[] channels);

        public static int[] SampleGetChannels(int handle)
        {
            var channels = new int[SampleGetInfo(handle).Max];

            BASS_SampleGetChannels(handle, channels);

            return channels;
        }

        [DllImport(DllName, EntryPoint = "BASS_SampleStop")]
        public static extern bool SampleStop(int handle);

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