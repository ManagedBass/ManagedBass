using System.Runtime.InteropServices;
using System;

namespace ManagedBass.Dynamics
{
    public enum BQFType
    {
        // Summary:
        //     BiQuad Lowpass filter.
        LowPass = 0,
        //
        // Summary:
        //     BiQuad Highpass filter.
        HighPass = 1,
        //
        // Summary:
        //     BiQuad Bandpass filter (constant 0 dB peak gain).
        BandPass = 2,
        //
        // Summary:
        //     BiQuad Bandpass Q filter (constant skirt gain, peak gain = Q).
        BandPassQ = 3,
        //
        // Summary:
        //     BiQuad Notch filter.
        Notch = 4,
        //
        // Summary:
        //     BiQuad All-Pass filter.
        AllPass = 5,
        //
        // Summary:
        //     BiQuad Peaking EQ filter.
        PeakingEQ = 6,
        //
        // Summary:
        //     BiQuad Low-Shelf filter.
        LowShelf = 7,
        //
        // Summary:
        //     BiQuad High-Shelf filter.
        HighShelf = 8,
    }

    [Flags]
    public enum FXChannelFlags
    {
        // Summary:
        //     All channels at once (as by default).
        ChannelAll = -1,
        //
        // Summary:
        //     Disable an effect for all channels (resp. set the global volume of the Un4seen.Bass.AddOn.Fx.BASS_BFX_VOLUME
        //     effect).
        ChannelNone = 0,
        //
        // Summary:
        //     left-front channel
        Channel1 = 1,
        //
        // Summary:
        //     right-front channel
        Channel2 = 2,
        //
        // Summary:
        //     Channel 3: depends on the multi-channel source (see above info).
        Channel3 = 4,
        //
        // Summary:
        //     Channel 4: depends on the multi-channel source (see above info).
        Channel4 = 8,
        //
        // Summary:
        //     Channel 5: depends on the multi-channel source (see above info).
        Channel5 = 16,
        //
        // Summary:
        //     Channel 6: depends on the multi-channel source (see above info).
        Channel6 = 32,
        //
        // Summary:
        //     Channel 7: depends on the multi-channel source (see above info).
        Channel7 = 64,
        //
        // Summary:
        //     Channel 8: depends on the multi-channel source (see above info).
        Channel8 = 128,
        //
        // Summary:
        //     Channel 9: depends on the multi-channel source (see above info).
        Channel9 = 256,
        //
        // Summary:
        //     Channel 10: depends on the multi-channel source (see above info).
        Channel10 = 512,
        //
        // Summary:
        //     Channel 11: depends on the multi-channel source (see above info).
        Channel11 = 1024,
        //
        // Summary:
        //     Channel 12: depends on the multi-channel source (see above info).
        Channel12 = 2048,
        //
        // Summary:
        //     Channel 13: depends on the multi-channel source (see above info).
        Channel13 = 4096,
        //
        // Summary:
        //     Channel 14: depends on the multi-channel source (see above info).
        Channel14 = 8192,
        //
        // Summary:
        //     Channel 15: depends on the multi-channel source (see above info).
        Channel15 = 16384,
        //
        // Summary:
        //     Channel 16: depends on the multi-channel source (see above info).
        Channel16 = 32768,
        //
        // Summary:
        //     Channel 17: depends on the multi-channel source (see above info).
        Channel17 = 65536,
        //
        // Summary:
        //     Channel 18: depends on the multi-channel source (see above info).
        Channel18 = 131072,
        //
        // Summary:
        //     Channel 19: depends on the multi-channel source (see above info).
        Channel19 = 262144,
        //
        // Summary:
        //     Channel 20: depends on the multi-channel source (see above info).
        Channel20 = 524288,
        //
        // Summary:
        //     Channel 21: depends on the multi-channel source (see above info).
        Channel21 = 1048576,
        //
        // Summary:
        //     Channel 22: depends on the multi-channel source (see above info).
        Channel22 = 2097152,
        //
        // Summary:
        //     Channel 23: depends on the multi-channel source (see above info).
        Channel23 = 4194304,
        //
        // Summary:
        //     Channel 24: depends on the multi-channel source (see above info).
        Channel24 = 8388608,
        //
        // Summary:
        //     Channel 25: depends on the multi-channel source (see above info).
        Channel25 = 16777216,
        //
        // Summary:
        //     Channel 26: depends on the multi-channel source (see above info).
        Channel26 = 33554432,
        //
        // Summary:
        //     Channel 27: depends on the multi-channel source (see above info).
        Channel27 = 67108864,
        //
        // Summary:
        //     Channel 28: depends on the multi-channel source (see above info).
        Channel28 = 134217728,
        //
        // Summary:
        //     Channel 29: depends on the multi-channel source (see above info).
        Channel29 = 268435456,
        //
        // Summary:
        //     Channel 30: depends on the multi-channel source (see above info).
        Channel30 = 536870912,
    }

    public delegate void BPMProcedure(int chan, float bpm, IntPtr user);

    public delegate void BPMProgressProcedure(int chan, float percent, IntPtr user);

    public delegate void BPMBeatProcedure(int chan, double beatpos, IntPtr user);

    public static class BassFx
    {
        const string DllName = "bass_fx.dll";

        static BassFx() { BassManager.Load(DllName); }

        [DllImport(DllName)]
        static extern int BASS_FX_GetVersion();

        public static int Version { get { return BASS_FX_GetVersion(); } }

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