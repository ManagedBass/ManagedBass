using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Mix
{
    /// <summary>
    /// Wraps BassMix: bassmix.dll
    /// </summary>
    public static class BassMix
    {
#if __IOS__
        const string DllName = "__internal";
#else
        const string DllName = "bassmix";
#endif

#if !__IOS__
        static IntPtr hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = DynamicLibrary.Load(DllName, Folder);

        public static void Unload() => DynamicLibrary.Unload(hLib);
#endif

        #region Split
        [DllImport(DllName, EntryPoint = "BASS_Split_StreamCreate")]
        public static extern int CreateSplitStream(int channel, BassFlags Flags, int[] chanmap);
        
		/// <summary>
		/// Retrieves the amount of buffered data available to a splitter stream, or the amount of data in a splitter source buffer.
		/// </summary>
		/// <param name="Handle">The splitter (as obtained by <see cref="CreateSplitStream" />) or the source channel handle.</param>
		/// <returns>If successful, then the amount of buffered data (in bytes) is returned, else -1 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// With a splitter source, this function reports how much data is in the buffer that is shared by all of its splitter streams.
        /// With a splitter stream, this function reports how much data is ahead of it in the buffer, before it will receive any new data from the source.
		/// A splitter stream can be repositioned within the buffer via the <see cref="SplitStreamReset(int, int)" /> function.
		/// <para>The amount of data that can be buffered is limited by the buffer size, which is determined by the <see cref="SplitBufferLength" /> config option.</para>
		/// <para>The returned buffered byte count is always based on the source's sample format, even with splitter streams that were created with a different channel count.</para>
		/// </remarks>
        /// <exception cref="Errors.Handle">The <paramref name="Handle" /> is neither a splitter stream or source.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Split_StreamGetAvailable")]
        public static extern int SplitStreamGetAvailable(int Handle);
        
		/// <summary>
		/// Resets a splitter stream or all splitter streams of a source.
		/// </summary>
		/// <param name="Handle">The splitter (as obtained by <see cref="CreateSplitStream" />) or the source channel handle.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// This function resets the splitter stream's buffer state, so that the next sample data it receives will be from the source's current position. 
		/// If the stream has ended, that is reset too, so that it can be played again.
        /// Unless called from within a mixtime sync callback, the stream's output buffer (if it has one) is also flushed.
		/// </remarks>
        /// <exception cref="Errors.Handle">The <paramref name="Handle" /> is neither a splitter stream or source.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Split_StreamReset")]
        public static extern bool SplitStreamReset(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_Split_StreamResetEx")]
        public static extern bool SplitStreamReset(int handle, int offset);
        
		/// <summary>
		/// Retrieves the source of a splitter stream.
		/// </summary>
		/// <param name="Handle">The splitter stream handle (which was add via <see cref="CreateSplitStream" /> beforehand).</param>
		/// <returns>If successful, the source stream's handle is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle">The <paramref name="Handle" /> is not a splitter stream.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Split_StreamGetSource")]
        public static extern int SplitStreamGetSource(int Handle);

        [DllImport(DllName)]
        static extern int BASS_Split_StreamGetSplits(int handle, [In, Out] int[] array, int length);
        		
        /// <summary>
		/// Retrieves the channel's splitters.
		/// </summary>
		/// <param name="Handle">The handle to check.</param>
		/// <returns>The array of splitter handles (<see langword="null" /> on error, use <see cref="Bass.LastError" /> to get the error code).</returns>
        public static int[] SplitStreamGetSplits(int Handle)
        {
            var num = BASS_Split_StreamGetSplits(Handle, null, 0);

            if (num <= 0) 
                return null;

            var numArray = new int[num];
            num = BASS_Split_StreamGetSplits(Handle, numArray, num);

            return num <= 0 ? null : numArray;
        }
#endregion

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamCreate")]
        public static extern int CreateMixerStream(int Frequency, int Channels, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamAddChannel")]
        public static extern bool MixerAddChannel(int Handle, int Channel, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_StreamAddChannelEx")]
        public static extern bool MixerAddChannel(int Handle, int Channel, BassFlags Flags, long Start, long Length);
        
		/// <summary>
		/// Unplugs a channel from a mixer.
		/// </summary>
		/// <param name="Handle">The handle of the mixer source channel to unplug (which was addded via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" />) beforehand).</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle">The channel is not plugged into a mixer.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemove")]
        public static extern bool MixerRemoveChannel(int Handle);

        #region Configuration
        /// <summary>
        /// The splitter Buffer Length in milliseconds... 100 (min) to 5000 (max).
        /// </summary>
        /// <remarks>
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
        /// </remarks>
        public static int SplitBufferLength
        {
            get { return Bass.GetConfig(Configuration.SplitBufferLength); }
            set { Bass.Configure(Configuration.SplitBufferLength, value); }
        }

        /// <summary>
        /// The source channel Buffer size multiplier... 1 (min) to 5 (max). 
        /// </summary>
        /// <remarks>
        /// If the value specified is outside this range, it is automatically capped.
        /// When a source channel has buffering enabled, the mixer will Buffer the decoded data,
        /// so that it is available to the <see cref="ChannelGetData(int, IntPtr, int)"/> and <see cref="ChannelGetLevel(int)"/> functions.
        /// To reach the source channel's Buffer size, the multiplier (multiple) is applied to the <see cref="Bass.PlaybackBufferLength"/>
        /// setting at the time of the mixer's creation.
        /// If the source is played at it's default rate, then the Buffer only need to be as big as the mixer's Buffer.
        /// But if it's played at a faster rate, then the Buffer needs to be bigger for it to contain the data that 
        /// is currently being heard from the mixer.
        /// For example, playing a channel at 2x its normal speed would require the Buffer to be 2x the normal size (multiple = 2).
        /// Larger buffers obviously require more memory, so the multiplier should not be set higher than necessary.
        /// The default multiplier is 2x. 
        /// Changes only affect subsequently setup channel buffers.
        /// An existing channel can have its Buffer reinitilized by disabling and then re-enabling 
        /// the <see cref="BassFlags.MixerBuffer"/> flag using <see cref="ChannelFlags"/>.
        /// </remarks>
        public static int MixerBufferLength
        {
            get { return Bass.GetConfig(Configuration.MixerBufferLength); }
            set { Bass.Configure(Configuration.MixerBufferLength, value); }
        }

        /// <summary>
        /// BASSmix add-on: How far back to keep record of source positions
        /// to make available for <see cref="ChannelGetPosition(int, PositionFlags, int)"/>, in milliseconds.
        /// </summary>
        /// <remarks>
        /// If a mixer is not a decoding channel (not using the BassFlag.Decode flag),
        /// this config setting will just be a minimum and the mixer will 
        /// always have a position record at least equal to its playback Buffer Length, 
        /// as determined by the PlaybackBufferLength config option.
        /// The default setting is 2000ms.
        /// Changes only affect newly created mixers, not any that already exist.
        /// </remarks>
        public static int MixerPositionEx
        {
            get { return Bass.GetConfig(Configuration.MixerPositionEx); }
            set { Bass.Configure(Configuration.MixerPositionEx, value); }
        }
#endregion

        #region Mixer Source Channels
#region Channel Flags
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelFlags")]
        public static extern BassFlags ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        public static bool ChannelHasFlag(int handle, BassFlags flag) => ChannelFlags(handle, 0, 0).HasFlag(flag);

        public static bool ChannelAddFlag(int handle, BassFlags flag) => ChannelFlags(handle, flag, flag).HasFlag(flag);

        public static bool ChannelRemoveFlag(int handle, BassFlags flag) => !ChannelFlags(handle, 0, flag).HasFlag(flag);
#endregion

#region Channel Get Data
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, [In, Out] byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, [In, Out] short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, [In, Out] int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, [In, Out] float[] Buffer, int Length);
#endregion
        
		/// <summary>
		/// Retrieves the level (peak amplitude) of a mixer source channel.
		/// </summary>
		/// <param name="Handle">The handle of the mixer source channel (which was add via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" />) beforehand).</param>
		/// <returns>
        /// If an error occurs, -1 is returned, use <see cref="Bass.LastError" /> to get the error code.
		/// <para>
        /// If successful, the level of the left channel is returned in the low word (low 16-bits), and the level of the right channel is returned in the high word (high 16-bits).
        /// If the channel is mono, then the low word is duplicated in the high word. 
		/// The level ranges linearly from 0 (silent) to 32768 (max). 0 will be returned when a channel is stalled.
        /// </para>
		/// </returns>
		/// <remarks>
		/// <para>
        /// This function is like the standard <see cref="Bass.ChannelGetLevel(int)" />, but it gets the level from the channel's buffer instead of decoding data from the channel, which means that the mixer doesn't miss out on any data. 
		/// In order to do this, the source channel must have buffering enabled, via the <see cref="BassFlags.MixerBuffer"/> flag.
        /// </para>
		/// <para>
        /// If the mixer is a decoding channel, then the channel's most recent data will be used to get the level.
        /// Otherwise, the level will be in sync with what is currently being heard from the mixer, unless the buffer is too small so that the currently heard data isn't in it. 
		/// The <see cref="MixerBufferLength"/> config option can be used to set the buffer size.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not plugged into a mixer.</exception>
        /// <exception cref="Errors.NotAvailable">The channel does not have buffering (<see cref="BassFlags.MixerBuffer"/>) enabled.</exception>
        /// <exception cref="Errors.NotPlaying">The mixer is not playing.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetLevel")]
        public static extern int ChannelGetLevel(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetLevelEx")]
        public static extern int ChannelGetLevel(int Handle, [In, Out] float[] Levels, float Length, LevelRetrievalFlags Flags);
        
		/// <summary>
		/// Retrieves a channel's mixing matrix, if it has one.
		/// </summary>
		/// <param name="Handle">The mixer source channel handle (which was add via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" />) beforehand).</param>
		/// <param name="Matrix">The 2-dimentional array (float[,]) where to write the matrix.</param>
		/// <returns>If successful, a <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// For more details see <see cref="ChannelSetMatrix(int, float[,])" />.
        /// The array must be big enough to get the matrix.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not plugged into a mixer.</exception>
        /// <exception cref="Errors.NotAvailable">The channel is not using matrix mixing.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMatrix")]
        public static extern bool ChannelGetMatrix(int Handle, [In, Out] float[,] Matrix);
        
		/// <summary>
		/// Retrieves the mixer that a channel is plugged into.
		/// </summary>
		/// <param name="Handle">The mixer source channel handle (which was add via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" /> beforehand).</param>
		/// <returns>If successful, the mixer stream's handle is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <exception cref="Errors.Handle">The channel is not plugged into a mixer.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetMixer")]
        public static extern int ChannelGetMixer(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrix")]
        public static extern bool ChannelSetMatrix(int Handle, float[,] Matrix);
        
		/// <summary>
		/// Fades to a channel's mixing matrix, if it has one.
		/// </summary>
		/// <param name="Handle">The mixer source channel handle (which was add via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" /> beforehand).</param>
		/// <param name="Matrix">The 2-dimensional array (float[,]) of the new mixing matrix.</param>
		/// <param name="Time">A period (in seconds) for the channel's current matrix to smoothly transition to the specified (new) matrix.</param>
		/// <returns>If successful, a <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// This method is identical to <see cref="ChannelSetMatrix(int, float[,])" /> except for the additional <paramref name="Time" /> parameter.
		/// <para>Note that <see cref="ChannelGetMatrix(int, float[,])" /> will always return the final matrix (that was passed to this function), not a mid-transition matrix.</para>
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not plugged into a mixer.</exception>
        /// <exception cref="Errors.NotAvailable">The channel is not using matrix mixing.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetMatrixEx")]
        public static extern bool ChannelSetMatrix(int Handle, float[,] Matrix, float Time);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPosition")]
        public static extern long ChannelGetPosition(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetPositionEx")]
        public static extern long ChannelGetPosition(int Handle, PositionFlags Mode, int Delay);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetPosition")]
        public static extern bool ChannelSetPosition(int Handle, long Position, PositionFlags Mode = PositionFlags.Bytes);

        [DllImport(DllName)]
        static extern int BASS_Mixer_ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedure Procedure, IntPtr User);

        public static int ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedure Procedure, IntPtr User = default(IntPtr))
        {
            var h = BASS_Mixer_ChannelSetSync(Handle, Type, Parameter, Procedure, User);

            if (h != 0)
                Extensions.ChannelReferences.Add(Handle, h, Procedure);

            return h;
        }

        [DllImport(DllName)]
        static extern int BASS_Mixer_ChannelSetSync(int Handle, int Type, long Parameter, SyncProcedureEx Procedure, IntPtr User);

        public static int ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedureEx Procedure, IntPtr User = default(IntPtr))
        {
            var h = BASS_Mixer_ChannelSetSync(Handle, (int)Type | 0x1000000, Parameter, Procedure, User);
            
            if (h != 0)
                Extensions.ChannelReferences.Add(Handle, h, Procedure);

            return h;
        }
        
		/// <summary>
		/// Removes a synchronizer from a mixer source channel.
		/// </summary>
		/// <param name="Handle">The mixer source channel handle (as returned by <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" />).</param>
		/// <param name="Sync">Handle of the synchronizer to remove (return value of a previous <see cref="ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" /> call).</param>
		/// <returns>If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>This function can only remove syncs that were set via <see cref="ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" />, not those that were set via <see cref="Bass.ChannelSetSync(int, SyncFlags, long, SyncProcedure, IntPtr)" />.</remarks>
        /// <exception cref="Errors.Handle">At least one of <paramref name="Handle" /> and <paramref name="Sync" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelRemoveSync")]
        public static extern bool ChannelRemoveSync(int Handle, int Sync);
        
		/// <summary>
		/// Retrieves the current position and value of an envelope on a channel.
		/// </summary>
		/// <param name="Handle">The mixer source channel handle (which was add via <see cref="MixerAddChannel(int, int, BassFlags)" /> or <see cref="MixerAddChannel(int, int, BassFlags, long, long)" />) beforehand).</param>
		/// <param name="Type">The envelope to get the position/value of.</param>
		/// <param name="Value">A reference to a variable to receive the envelope value at the current position.</param>
		/// <returns>If successful, the current position of the envelope is returned, else -1 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>The envelope's current position is not necessarily what is currently being heard, due to buffering.</remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not plugged into a mixer.</exception>
        /// <exception cref="Errors.Type"><paramref name="Type" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">There is no envelope of the requested type on the channel.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelGetEnvelopePos")]
        public static extern long ChannelGetEnvelopePosition(int Handle, MixEnvelope Type, ref float Value);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetEnvelopePos")]
        public static extern bool ChannelSetEnvelopePosition(int Handle, MixEnvelope Type, long Position);

        [DllImport(DllName, EntryPoint = "BASS_Mixer_ChannelSetEnvelope")]
        public static extern bool ChannelSetEnvelope(int Handle, MixEnvelope Type, MixerNode[] Nodes, int Count);

        public static bool ChannelSetEnvelope(int Handle, MixEnvelope Type, MixerNode[] Nodes)
        {
            return ChannelSetEnvelope(Handle, Type, Nodes, Nodes?.Length ?? 0);
        }
#endregion
    }
}