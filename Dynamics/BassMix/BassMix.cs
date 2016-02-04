using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    // TODO: Wrap BassMix to Completion
    /// <summary>
    /// Wraps BassMix: bassmix.dll
    /// </summary>
    public static class BassMix
    {
        // TODO: BASS_Mixer_ChannelGetLevelEx

        const string DllName = "bassmix.dll";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        #region Split
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

        [DllImport(DllName)]
        static extern int BASS_Split_StreamGetSplits(int handle, [In][Out] int[] array, int length);

        public static int[] SplitStreamGetSplits(int handle)
        {
            int num = BASS_Split_StreamGetSplits(handle, null, 0);

            if (num < 0) return null;

            int[] numArray = new int[num];
            num = BASS_Split_StreamGetSplits(handle, numArray, num);

            if (num < 0) return null;

            return numArray;
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamCreate")]
        public static extern int CreateMixerStream(int Frequency, int Channels, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamAddChannel")]
        public static extern bool MixerAddChannel(int Handle, int Channel, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamAddChannelEx")]
        public static extern bool MixerAddChannel(int Handle, int Channel, BassFlags Flags, long Start, long Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemove")]
        public static extern bool MixerRemoveChannel(int Handle);

        #region Channel Flags
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelFlags")]
        public static extern BassFlags ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        public static bool ChannelHasFlag(int handle, BassFlags flag) { return ChannelFlags(handle, 0, 0).HasFlag(flag); }

        public static bool ChannelAddFlag(int handle, BassFlags flag) { return ChannelFlags(handle, flag, flag).HasFlag(flag); }

        public static bool ChannelRemoveFlag(int handle, BassFlags flag) { return !(ChannelFlags(handle, 0, flag).HasFlag(flag)); }
        #endregion

        #region Configuration
        /// <summary>
        /// The splitter Buffer Length in milliseconds... 100 (min) to 5000 (max).
        /// If the value specified is outside this range, it is automatically capped.
        /// When a source has its first splitter stream created, a Buffer is allocated
        /// for its sample data, which all of its subsequently created splitter streams
        /// will share. This config option determines how big that Buffer is. The default
        /// is 2000ms.
        /// The Buffer will always be kept as empty as possible, so its size does not
        /// necessarily affect latency; it just determines how far splitter streams can
        /// drift apart before there are Buffer overflow issues for those left behind.
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
        /// The source channel Buffer size multiplier.
        /// multiple (int): The Buffer size multiplier... 1 (min) to 5 (max). 
        /// If the value specified is outside this range, it is automatically capped.
        /// When a source channel has buffering enabled, the mixer will Buffer the decoded data,
        /// so that it is available to the BassMix.MixerChannelGetData() and BassMix.MixerChannelGetLevel() functions.
        /// To reach the source channel's Buffer size, the multiplier (multiple) is applied to the BASS_CONFIG_BUFFER 
        /// setting at the time of the mixer's creation.
        /// If the source is played at it's default rate, then the Buffer only need to be as big as the mixer's Buffer.
        /// But if it's played at a faster rate, then the Buffer needs to be bigger for it to contain the data that 
        /// is currently being heard from the mixer. 
        /// For example, playing a channel at 2x its normal speed would require the Buffer to be 2x the normal size (multiple = 2).
        /// Larger buffers obviously require more memory, so the multiplier should not be set higher than necessary.
        /// The default multiplier is 2x. 
        /// Changes only affect subsequently setup channel buffers.
        /// An existing channel can have its Buffer reinitilized by disabling and then re-enabling 
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
        /// Length (int): The Length of time to back, in milliseconds.
        /// If a mixer is not a decoding channel (not using the BassFlag.Decode flag),
        /// this config setting will just be a minimum and the mixer will 
        /// always have a position record at least equal to its playback Buffer Length, 
        /// as determined by the PlaybackBufferLength config option.
        /// The default setting is 2000ms.
        /// Changes only affect newly created mixers, not any that already exist.
        /// </summary>
        public static int MixerPositionEx
        {
            get { return Bass.GetConfig(Configuration.MixerPositionEx); }
            set { Bass.Configure(Configuration.MixerPositionEx, value); }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetLevel")]
        public static extern int ChannelGetLevel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMatrix")]
        public static extern bool ChannelGetMatrix(int Handle, IntPtr Matrix);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMixer")]
        public static extern int ChannelGetMixer(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrix")]
        public static extern bool ChannelSetMatrix(int Handle, float[,] Matrix);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrixEx")]
        public static extern bool ChannelSetMatrix(int Handle, float[,] Matrix, float Time);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPosition")]
        public static extern long ChannelGetPosition(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPositionEx")]
        public static extern long ChannelGetPosition(int Handle, PositionFlags Mode, int Delay);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetPosition")]
        public static extern bool ChannelSetPosition(int Handle, long Position, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetSync")]
        public static extern int ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName)]
        static extern int BASS_Mixer_ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedureEx Procedure, IntPtr User);

        public static int ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedureEx Procedure, IntPtr User = default(IntPtr))
        {
            return BASS_Mixer_ChannelSetSync(Handle, (SyncFlags)((int)Type | 0x1000000), Parameter, Procedure, User);
        }

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemoveSync")]
        public static extern bool ChannelRemoveSync(int Handle, int Sync);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetEnvelopePos")]
        public static extern long ChannelGetEnvelopePosition(int Handle, MixEnvelope Type, ref float Value);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetEnvelopePos")]
        public static extern bool ChannelSetEnvelopePosition(int Handle, MixEnvelope Type, long Position);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetEnvelope")]
        public static extern bool ChannelSetEnvelope(int Handle, MixEnvelope Type, MixerNode[] Nodes, int Count);

        public static bool ChannelSetEnvelope(int Handle, MixEnvelope Type, MixerNode[] Nodes)
        {
            return ChannelSetEnvelope(Handle, Type, Nodes, Nodes != null ? Nodes.Length : 0);
        }
    }
}