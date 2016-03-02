using System;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        #region SampleGetChannel
        [DllImport(DllName)]
        static extern int BASS_SampleGetChannel(int Sample, bool OnlyNew);

        /// <summary>
        /// Creates/initializes a playback channel for a sample.
        /// </summary>
        /// <param name="Sample">Handle of the sample to play.</param>
        /// <param name="OnlyNew">Do not recycle/override one of the sample's existing channels?</param>
        /// <returns>If successful, the handle of the new channel is returned, else 0 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// Use <see cref="SampleGetInfo(int, ref SampleInfo)" /> and <see cref="SampleSetInfo(int, SampleInfo)" /> to set a sample's default attributes, which are used when creating a channel. 
        /// After creation, a channel's attributes can be changed via <see cref="ChannelSetAttribute(int, ChannelAttribute, float)" />, <see cref="ChannelSet3DAttributes" /> and <see cref="ChannelSet3DPosition" />.
        /// <see cref="Apply3D" /> should be called before starting playback of a 3D sample, even if you just want to use the default settings.
        /// </para>
        /// <para>
        /// If a sample has a maximum number of simultaneous playbacks of 1 (the max parameter was 1 when calling <see cref="SampleLoad(string, long, int, int, BassFlags)" /> or <see cref="CreateSample" />), then the HCHANNEL handle returned will be identical to the HSAMPLE handle. 
        /// That means you can use the HSAMPLE handle with functions that usually require a HCHANNEL handle, but you must still call this function first to initialize the channel.
        /// </para>
        /// <para>
        /// A sample channel is automatically freed when it's overridden by a new channel, or when stopped manually via <see cref="ChannelStop" />, <see cref="SampleStop" /> or <see cref="Stop" />. 
        /// If you wish to stop a channel and re-use it, it should be paused (<see cref="ChannelPause" />) instead of stopped. 
        /// Determining whether a channel still exists can be done by trying to use the handle in a function call, eg. <see cref="ChannelGetAttribute(int, ChannelAttribute, out float)" />.
        /// </para>
        /// <para>When channel overriding has been enabled via an override flag and there are multiple candidates for overriding (eg. with identical volume), the oldest of them will be chosen to make way for the new channel.</para>
        /// <para>
        /// The new channel will have an initial state of being paused (<see cref="PlaybackState.Paused"/>).
        /// This prevents the channel being claimed by another call of this function before it has been played, unless it gets overridden due to a lack of free channels.
        /// </para>
        /// <para>All of a sample's channels share the same sample data, and just have their own individual playback state information (volume/position/etc).</para>
        /// </remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Sample" /> is not a valid sample handle.</exception>
        /// <exception cref="Errors.NoFreeChannelAvailable">The sample has no free channels... the maximum number of simultaneous playbacks has been reached, and no override flag was specified for the sample or onlynew = <see langword="true" />.</exception>
        /// <exception cref="Errors.ConnectionTimedout">The sample's minimum time gap (<see cref="SampleInfo" />) has not yet passed since the last channel was created.</exception>
        public static int SampleGetChannel(int Sample, bool OnlyNew) => Checked(BASS_SampleGetChannel(Sample, OnlyNew));
        #endregion

        #region SampleFree
        [DllImport(DllName)]
        static extern bool BASS_SampleFree(int Handle);

        /// <summary>
		/// Frees a sample's resources.
		/// </summary>
		/// <param name="Handle">The sample handle.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not valid.</exception>
        public static bool SampleFree(int Handle) => Checked(BASS_SampleFree(Handle));
        #endregion

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
