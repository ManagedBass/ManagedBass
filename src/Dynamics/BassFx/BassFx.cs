using System.Runtime.InteropServices;
using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassFx: bassfx.dll
    /// </summary>
    public static class BassFx
    {
        const string DllName = "bass_fx";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        [DllImport(DllName)]
        static extern int BASS_FX_GetVersion();

        public static Version Version { get { return Extensions.GetVersion(BASS_FX_GetVersion()); } }

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoCreate")]
        public static extern int TempoCreate(int Channel, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_FX_ReverseCreate")]
        public static extern int ReverseCreate(int Channel, float DecodingBlockLength, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoGetSource")]
        public static extern int TempoGetSource(int Channel);

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoGetRatio")]
        public static extern float TempoGetRatio(int Channel);

        [DllImport(DllName, EntryPoint = "BASS_FX_ReverseGetSource")]
        public static extern int ReverseGetSource(int Channel);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_DecodeGet")]
        public static extern float BPMDecodeGet(int Channel, double StartSec, double EndSec, int MinMaxBPM, BassFlags Flags, BPMProgressProcedure Procedure, IntPtr User);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_CallbackSet")]
        public static extern bool BPMCallbackSet(int Handle, BPMProcedure Procedure, double Period, int MinMaxBPM, BassFlags Flags, IntPtr User);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_CallbackReset")]
        public static extern bool BPMCallbackReset(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_Free")]
        public static extern bool BPMFree(int Handle);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatDecodeGet")]
        public static extern bool BPMBeatDecodeGet(int Channel, double StartSec, double EndSec, BassFlags Flags, BPMBeatProcedure Procedure, IntPtr User);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatCallbackSet")]
        public static extern bool BPMBeatCallbackSet(int Handle, BPMBeatProcedure Procedure, IntPtr User);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatCallbackReset")]
        public static extern bool BPMBeatCallbackReset(int Handle);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatSetParameters")]
        public static extern bool BPMBeatSetParameters(int Handle, float Bandwidth, float CenterFreq, float Beat_rTime);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatGetParameters")]
        public static extern bool BPMBeatGetParameters(int Handle, out float Bandwidth, out float CenterFreq, out float Beat_rTime);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_BeatFree")]
        public static extern bool BPMBeatFree(int Handle);
    }
}