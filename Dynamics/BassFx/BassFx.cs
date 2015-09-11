using System.Runtime.InteropServices;
using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassFx: bassfx.dll
    /// </summary>
    public static class BassFx
    {
        const string DllName = "bass_fx.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        [DllImport(DllName)]
        static extern int BASS_FX_GetVersion();

        public static Version Version { get { return Extensions.GetVersion(BASS_FX_GetVersion()); } }

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoCreate")]
        public static extern int TempoCreate(int chan, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_FX_ReverseCreate")]
        public static extern int ReverseCreate(int chan, float dec_block, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoGetSource")]
        public static extern int TempoGetSource(int chan);

        [DllImport(DllName, EntryPoint = "BASS_FX_TempoGetRatio")]
        public static extern float TempoGetRatio(int chan);

        [DllImport(DllName, EntryPoint = "BASS_FX_ReverseGetSource")]
        public static extern int ReverseGetSource(int chan);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_DecodeGet")]
        public static extern float BPMDecodeGet(int chan, double startSec, double endSec, int minMaxBPM, BassFlags flags, BPMProgressProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_CallbackSet")]
        public static extern bool BPMCallbackSet(int handle, BPMProcedure proc, double period, int minMaxBPM, BassFlags flags, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_CallbackReset")]
        public static extern bool BPMCallbackReset(int handle);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_Free")]
        public static extern bool BPMFree(int handle);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatDecodeGet")]
        public static extern bool BPMBeatDecodeGet(int chan, double startSec, double endSec, BassFlags flags, BPMBeatProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatCallbackSet")]
        public static extern bool BPMBeatCallbackSet(int handle, BPMBeatProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatCallbackReset")]
        public static extern bool BPMBeatCallbackReset(int handle);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatSetParameters")]
        public static extern bool BPMBeatSetParameters(int handle, float bandwidth, float centerfreq, float beat_rtime);

        [DllImport(DllName, EntryPoint="BASS_FX_BPM_BeatGetParameters")]
        public static extern bool BPMBeatGetParameters(int handle, out float bandwidth, out float centerfreq, out float beat_rtime);

        [DllImport(DllName, EntryPoint = "BASS_FX_BPM_BeatFree")]
        public static extern bool BPMBeatFree(int handle);
    }
}