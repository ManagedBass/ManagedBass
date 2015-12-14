using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetInfo")]
        public static extern bool ChannelInfo(int Device, out ChannelInfo Info);

        public static ChannelInfo ChannelInfo(int Device)
        {
            ChannelInfo temp;
            ChannelInfo(Device, out temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetDSP")]
        public static extern int ChannelSetDSP(int Handle, DSPProcedure Procedure, IntPtr User = default(IntPtr), int Priority = 0);

        [DllImport(DllName, EntryPoint = "BASS_ChannelPlay")]
        public static extern bool PlayChannel(int Handle, bool Restart = false);

        [DllImport(DllName, EntryPoint = "BASS_ChannelPause")]
        public static extern bool PauseChannel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelStop")]
        public static extern bool StopChannel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelLock")]
        public extern static bool LockChannel(int Handle, bool Lock);

        [DllImport(DllName, EntryPoint = "BASS_ChannelIsActive")]
        public extern static PlaybackState IsChannelActive(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetLink")]
        public extern static bool LinkChannels(int Handle, int Channel);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveLink")]
        public extern static bool ChannelRemoveLink(int Handle, int Channel);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveDSP")]
        public extern static bool ChannelRemoveDSP(int Handle, int DSP);

        #region Channel Flags
        [DllImport(DllName)]
        extern static BassFlags BASS_ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        public static bool ChannelHasFlag(int Handle, BassFlags Flag) { return BASS_ChannelFlags(Handle, 0, 0).HasFlag(Flag); }

        public static bool ChannelAddFlag(int Handle, BassFlags Flag) { return BASS_ChannelFlags(Handle, Flag, Flag).HasFlag(Flag); }

        public static bool ChannelRemoveFlag(int Handle, BassFlags Flag) { return !(BASS_ChannelFlags(Handle, 0, Flag).HasFlag(Flag)); }
        #endregion

        #region Channel Attributes
        [DllImport(DllName)]
        extern static bool BASS_ChannelGetAttribute(int Handle, ChannelAttribute attrib, ref float value);

        public static double GetChannelAttribute(int Handle, ChannelAttribute attrib)
        {
            float temp = 0;
            BASS_ChannelGetAttribute(Handle, attrib, ref temp);
            return temp;
        }

        [DllImport(DllName)]
        extern static bool BASS_ChannelSetAttribute(int Handle, ChannelAttribute attrib, float value);

        public static bool SetChannelAttribute(int Handle, ChannelAttribute attrib, double value)
        {
            return BASS_ChannelSetAttribute(Handle, attrib, (float)value);
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetTags")]
        public extern static IntPtr GetChannelTags(int Handle, TagType Tags);

        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveFX")]
        public extern static bool ChannelRemoveFX(int Handle, int FX);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetLength")]
        public extern static long ChannelGetLength(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetFX")]
        public extern static int ChannelSetFX(int Handle, EffectType Type, int Priority);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetSync")]
        public static extern int ChannelSetSync(int Handle, SyncFlags Type, long Param, SyncProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ChannelBytes2Seconds")]
        public extern static double ChannelBytes2Seconds(int Handle, long Position);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSeconds2Bytes")]
        public extern static long ChannelSeconds2Bytes(int Handle, double Position);

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetPosition")]
        public extern static long GetChannelPosition(int Handle, PositionFlags mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetPosition")]
        public extern static bool SetChannelPosition(int Handle, long pos, PositionFlags mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_ChannelIsSliding")]
        public extern static bool IsChannelSliding(int Handle, ChannelAttribute attrib);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSlideAttribute")]
        public extern static bool SlideChannelAttribute(int Handle, ChannelAttribute attrib, float value, int time);

        #region Channel Get Level
        [DllImport(DllName)]
        static extern int BASS_ChannelGetLevel(int Handle);

        public static double GetChannelLevel(int Channel)
        {
            int Temp = BASS_ChannelGetLevel(Channel);
            return (Temp.LoWord() + Temp.HiWord()) / (2.0 * 32768);
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, IntPtr Buffer, int Length);
    }
}