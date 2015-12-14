using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassMix
    {
        // TODO: BASS_Split_StreamGetSplits
        // TODO: BASS_Mixer_ChannelGetEnvelopePos
        // TODO: BASS_Mixer_ChannelGetLevelEx
        // TODO: BASS_Mixer_ChannelSetEnvelope

        const string DllName = "bassmix.dll";

        static BassMix() { BassManager.Load(DllName); }

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamCreate")]
        public static extern int CreateSplitStream(int channel, BassFlags Flags, [MarshalAs(UnmanagedType.LPArray)] int[] chanmap);

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamGetAvailable")]
        public static extern int SplitStreamGetAvailable(int hndle);

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamReset")]
        public static extern bool ResetSplitStream(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamResetEx")]
        public static extern bool ResetSplitStreamEx(int handle, int offset);

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamGetSource")]
        public static extern int SplitStreamGetSource(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamCreate")]
        public static extern int CreateMixerStream(int freq, int chans, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamAddChannel")]
        public static extern bool MixerAddChannel(int handle, int channel, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemove")]
        public static extern bool MixerRemoveChannel(int handle);

        #region Channel Flags
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelFlags")]
        public static extern BassFlags ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        public static bool ChannelHasFlag(int handle, BassFlags flag) { return ChannelFlags(handle, 0, 0).HasFlag(flag); }

        public static bool ChannelAddFlag(int handle, BassFlags flag) { return ChannelFlags(handle, flag, flag).HasFlag(flag); }

        public static bool ChannelRemoveFlag(int handle, BassFlags flag) { return !(ChannelFlags(handle, 0, flag).HasFlag(flag)); }
        #endregion

        /// <summary>
        /// The splitter buffer length in milliseconds... 100 (min) to 5000 (max).
        /// If the value specified is outside this range, it is automatically capped.
        /// When a source has its first splitter stream created, a buffer is allocated
        /// for its sample data, which all of its subsequently created splitter streams
        /// will share. This config option determines how big that buffer is. The default
        /// is 2000ms.
        /// The buffer will always be kept as empty as possible, so its size does not
        /// necessarily affect latency; it just determines how far splitter streams can
        /// drift apart before there are buffer overflow issues for those left behind.
        /// Changes do not affect buffers that have already been allocated; any sources
        /// that have already had splitter streams created will continue to use their
        /// existing buffers.
        /// </summary>
        public static int SplitBufferLength
        {
            get { return Bass.GetConfig(Configuration.SplitBufferLength); }
            set { Bass.Configure(Configuration.SplitBufferLength, value); }
        }

        /// <summary>
        /// The source channel buffer size multiplier.
        /// multiple (int): The buffer size multiplier... 1 (min) to 5 (max). 
        /// If the value specified is outside this range, it is automatically capped.
        /// When a source channel has buffering enabled, the mixer will buffer the decoded data,
        /// so that it is available to the BassMix.MixerChannelGetData() and BassMix.MixerChannelGetLevel() functions.
        /// To reach the source channel's buffer size, the multiplier (multiple) is applied to the BASS_CONFIG_BUFFER 
        /// setting at the time of the mixer's creation.
        /// If the source is played at it's default rate, then the buffer only need to be as big as the mixer's buffer.
        /// But if it's played at a faster rate, then the buffer needs to be bigger for it to contain the data that 
        /// is currently being heard from the mixer. 
        /// For example, playing a channel at 2x its normal speed would require the buffer to be 2x the normal size (multiple = 2).
        /// Larger buffers obviously require more memory, so the multiplier should not be set higher than necessary.
        /// The default multiplier is 2x. 
        /// Changes only affect subsequently setup channel buffers.
        /// An existing channel can have its buffer reinitilized by disabling and then re-enabling 
        /// the BASS_MIXER_BUFFER flag using BassMix.MixerChannelFlags().
        /// </summary>
        public static int MixerBufferLength
        {
            get { return Bass.GetConfig(Configuration.MixerBufferLength); }
            set { Bass.Configure(Configuration.MixerBufferLength, value); }
        }

        /// <summary>
        /// BASSmix add-on: How far back to keep record of source positions
        /// to make available for BassMix.MixerChannelGetPositionEx().
        /// length (int): The length of time to back, in milliseconds.
        /// If a mixer is not a decoding channel (not using the BassFlag.Decode flag),
        /// this config setting will just be a minimum and the mixer will 
        /// always have a position record at least equal to its playback buffer length, 
        /// as determined by the BASSConfig.BASS_CONFIG_BUFFER config option.
        /// The default setting is 2000ms.
        /// Changes only affect newly created mixers, not any that already exist.
        /// </summary>
        public static int MixerPositionEx
        {
            get { return Bass.GetConfig(Configuration.MixerPositionEx); }
            set { Bass.Configure(Configuration.MixerPositionEx, value); }
        }

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern bool MixerChannelGetData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetLevel")]
        public static extern int MixerChannelGetLevel(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMatrix")]
        public static extern bool MixerChannelGetMatrix(int handle, IntPtr matrix);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMixer")]
        public static extern int MixerChannelGetMixer(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrix")]
        public static extern bool MixerChannelSetMatrix(int handle, IntPtr matrix);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrixEx")]
        public static extern bool MixerChannelSetMatrixEx(int handle, IntPtr matrix, float time);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPosition")]
        public static extern long MixerChannelGetPosition(int handle, PositionFlags mode);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPositionEx")]
        public static extern long MixerChannelGetPositionEx(int handle, PositionFlags mode, int delay);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetPosition")]
        public static extern bool MixerChannelSetPosition(int handle, long pos, PositionFlags mode);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetSync")]
        public static extern int MixerChannelSetSync(int handle, int type, long param, SyncProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemoveSync")]
        public static extern bool MixerChannelRemoveSync(int handle, int sync);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetEnvelopePos")]
        public static extern long MixerChannelSetEnvelopePosition(int handle, int type, long pos);
    }
}