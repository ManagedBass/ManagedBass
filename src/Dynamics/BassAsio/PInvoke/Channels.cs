using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class BassAsio
    {
        /// <summary>
		/// Enable/disable processing of an Asio channel.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <param name="Procedure">The user defined function to process the channel... <see langword="null" /> = disable the channel.</param>
		/// <param name="User">User instance data to pass to the callback function.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// All ASIO channels are mono.
        /// Stereo (and above) channels can be formed by joining multiple channels together using <see cref="ChannelJoin" />.
        /// </para>
		/// <para>Use <see cref="Start" /> to begin processing the enabled channels.</para>
		/// <para>
        /// You might also use this function on an already enabled ASIO channel if you just want to change the <see cref="AsioProcedure" /> which should be used.
		/// However changing the callback procedure to <see langword="null" /> would disable the channel - which is only possible, if the ASIO device is stopped.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Start">The device has been started - it needs to be stopped before (dis)enabling channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnable")]
        public static extern bool ChannelEnable(bool Input, int Channel, AsioProcedure Procedure, IntPtr User = default(IntPtr));
        
		/// <summary>
		/// Enables an output channel, and makes it mirror another channel.
		/// </summary>
		/// <param name="Channel">The output channel number... 0 = first.</param>
		/// <param name="Input2">Mirroring an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel2">The channel to mirror.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// This function allows an input or output channel to be duplicated in other output channel.
        /// This can be achieved using normal <see cref="AsioProcedure" /> processing, but it's more efficient to let BassAsio simply copy the data from one channel to another.
        /// </para>
		/// <para>
        /// Mirror channels can't be joined together to form multi-channel mirrors.
        /// Instead, to mirror multiple channels, an individual mirror should be setup for each of them.
        /// </para>
		/// <para>After <see cref="Start" /> has been called to begin processing, it's not possible to setup new mirror channels, but it is still possible to change the channel that a mirror is mirroring.</para>
		/// <para>
        /// When mirroring an output channel that hasn't been enabled, the mirror channel will just produce silence.
        /// When mirroring an input channel that hasn't already been enabled, the channel is automatically enabled for processing when <see cref="Start" /> is called, so that it can be mirrored.
		/// If the mirror is switched to a disabled input channel once processing has begun, then it will produce silence.
        /// </para>
		/// <para>
        /// A mirror channel can be made to have a different volume level to the channel that it's mirroring, using <see cref="ChannelSetVolume" />.
        /// The volume setting is cumulative.
		/// For example, if the mirror channel has a volume setting of 0.5 and the mirrored channel has a volume setting of 0.4, the effective volume of the mirror channel will be 0.2 (0.5 x 0.4).
        /// </para>
		/// <para><see cref="ChannelEnable" /> can be used to disable a mirror channel.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Start">The device has been started - it needs to be stopped before enabling channels.</exception>
        /// <exception cref="Errors.Parameter">At least one of the channels is invalid.</exception>
        /// <exception cref="Errors.SampleFormat">It is not possible to mirror channels that do not have the same sample format.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnableMirror")]
        public static extern bool ChannelEnableMirror(int Channel, bool Input2, int Channel2);
        
		/// <summary>
		/// Retrieves a channel's sample format.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>If an error occurs, -1 (<see cref="AsioSampleFormat.Unknown"/>) is returned, use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetFormat")]
        public static extern AsioSampleFormat ChannelGetFormat(bool Input, int Channel);
        
        #region ChannelGetInfo
		/// <summary>
		/// Retrieves information on an Asio channel.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <param name="Info">An instance of the <see cref="AsioChannelInfo" /> structure to store the information at.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetInfo")]
        public static extern bool ChannelGetInfo(bool Input, int Channel, out AsioChannelInfo Info);
                
		/// <summary>
		/// Retrieves information on an Asio channel.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>An instance of the <see cref="AsioChannelInfo" /> structure is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        public static AsioChannelInfo ChannelGetInfo(bool Input, int Channel)
        {
            AsioChannelInfo info;
            if (!ChannelGetInfo(Input, Channel, out info))
                throw new BassException(LastError);
            return info;
        }
        #endregion

        #region ChannelGetLevel
        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetLevel(bool input, int channel);
        
		/// <summary>
		/// Retrieves the level (peak amplitude) of a channel.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>
        /// If an error occurs, -1 is returned, use <see cref="LastError" /> to get the error code. 
		/// If successful, the level of the channel is returned, ranging from 0 (silent) to 1 (max).
        /// If the channel's native sample format is floating-point, it is actually possible for the level to go above 1.
        /// </returns>
		/// <remarks>
        /// This function measures the level of a single channel, and is not affected by any other channels that are joined with it.
		/// <para>Volume settings made via <see cref="ChannelSetVolume" /> affect the level reading of output channels, but not input channels.</para>
		/// <para>
        /// When an input channel is paused, it is still possible to get its level.
        /// Paused output channels will have a level of 0.
        /// </para>
		/// <para>Level retrieval is not supported when the sample format is DSD.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        /// <exception cref="Errors.Start">The device hasn't been started, or the channel isn't enabled.</exception>
        /// <exception cref="Errors.SampleFormat">Level retrieval is not supported for the channel's sample format (please report).</exception>
        public static double ChannelGetLevel(bool Input, int Channel) => BASS_ASIO_ChannelGetLevel(Input, Channel);
        #endregion
        
		/// <summary>
		/// Retrieves a channel's sample rate.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>If succesful, the channel's sample rate is returned (0 = device rate), else -1 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetRate")]
        public static extern double ChannelGetRate(bool Input, int Channel);

        #region ChannelGetVolume
        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetVolume(bool input, int channel);
        
		/// <summary>
		/// Retrieves a channel's volume setting.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first, -1 = master.</param>
		/// <returns>If successful, the channel's volume setting is returned, else -1 is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>To set a channel volume use <see cref="ChannelSetVolume" />.</remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        public static double ChannelGetVolume(bool Input, int Channel) => BASS_ASIO_ChannelGetVolume(Input, Channel);
        #endregion
        
		/// <summary>
		/// Checks if a channel is enabled for processing.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>One <see cref="AsioChannelActive"/> value is returned.</returns>
		/// <remarks>
		/// When a channel is joined to another, the status of the other channel is returned, as that is what determines whether the channel is enabled for processing - whether it's been enabled itself is of no consequence while it is joined to another. 
		/// For example, if channel B is joined to channel A, and channel A is not enabled, then neither is channel B.
		/// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelIsActive")]
        public static extern AsioChannelActive ChannelIsActive(bool Input, int Channel);
        
		/// <summary>
		/// Join a channel to another.
		/// </summary>
		/// <param name="Input">Dealing with input channels? <see langword="false" /> = output channels.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <param name="Channel2">The channel to join it to... -1 = remove current join.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// All ASIO channels are mono.
        /// By joining them, stereo (and above) channels can be formed, making it simpler to process stereo (and above) sample data.
        /// </para>
		/// <para>
        /// By default, channels can only be joined to preceding channels.
        /// For example, channel 1 can be joined to channel 0, but not vice versa.
		/// The <see cref="AsioInitFlags.JoinOrder"/> flag can be used in the <see cref="Init" /> call to remove that restriction.
        /// When joining a group of channels, there should be one channel enabled via <see cref="ChannelEnable" /> with the rest joined to it - 
		/// do not join a channel to a channel that is itself joined to another channel.
        /// Mirror channels, setup using <see cref="ChannelEnableMirror" />, cannot be joined with.
        /// </para>
		/// <para>
        /// If a channel has two or more other channels joined to it, then the joined channels will default to being in numerically ascending order in the <see cref="AsioProcedure" /> callback function's sample data unless the <see cref="AsioInitFlags.JoinOrder"/> flag was used in the <see cref="Init" /> call, 
		/// in which case they will be in the order in which they were joined via this function.
        /// In the latter case, if this function is called on an already joined channel, the channel will be moved to the end of the joined group.
        /// </para>
		/// <para>
        /// While a channel is joined to another, it automatically takes on the attributes of the other channel - the other channel's settings determine the sample format, the sample rate and whether it is enabled.
		/// The volume setting remains individual though, allowing balance control over the joined channels.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Start">The device has been started - it needs to be stopped before (dis)enabling channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        /// <exception cref="Errors.SampleFormat">It is not possible to join channels that do not have the same sample format.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelJoin")]
        public static extern bool ChannelJoin(bool Input, int Channel, int Channel2);
        
		/// <summary>
		/// Suspends processing of a channel.
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// Channels can only be disabled when the device is stopped.
        /// When you want to stop processing only some of the enabled channels, there are few ways that could be done.
        /// You could quickly stop the device, disable the unwanted channels, and restart the device. 
		/// In the case of output channels, you could fill the channels' buffers with silence (0s) in the <see cref="AsioProcedure" />.
        /// Or you could pause the channels, using this function. 
		/// The less channels BassAsio has to process, the less CPU it'll use, so stopping and restarting the device would be the most efficient, but that could cause a slight break in the sound of the other channels. 
		/// Filling the buffers with silence is the least efficient, as BassAsio will still process the data as if it was "normal", but it does mean that other channels are unaffected.
		/// Pausing is a compromise between the two - the channels will still be enabled, but BassAsio will bypass any additional processing (resampling/etc) that may normally be required.</para>
		/// <para>Use <see cref="ChannelReset" /> to resume processing of a paused channel.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelPause")]
        public static extern bool ChannelPause(bool Input, int Channel);
                
		/// <summary>
		/// Resets the attributes of a channel (or all channels).
		/// </summary>
		/// <param name="Input">Dealing with an input channel? <see langword="false" /> = an output channel.</param>
		/// <param name="Channel">The input/output channel number... 0 = first, -1 = all channels.</param>
		/// <param name="Flags">The attributes to reset. A combination of <see cref="AsioChannelResetFlags"/>.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>When resetting all channels (channel = -1), the resetting only applies to all channels of the specified type, ie. input or output, not both.</remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Start">The device has been started - it needs to be stopped before disabling or unjoining channels.</exception>
        /// <exception cref="Errors.Parameter">The <paramref name="Input" /> and <paramref name="Channel" /> combination is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelReset")]
        public static extern bool ChannelReset(bool Input, int Channel, AsioChannelResetFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetFormat")]
        public static extern bool ChannelSetFormat(bool input, int channel, AsioSampleFormat format);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetRate")]
        public static extern bool ChannelSetRate(bool input, int channel, double rate);

        #region ChannelSetVolume
        [DllImport(DllName)]
        static extern bool BASS_ASIO_ChannelSetVolume(bool input, int channel, float volume);

        public static bool ChannelSetVolume(bool input, int channel, double volume) => BASS_ASIO_ChannelSetVolume(input, channel, (float)volume);
        #endregion
    }
}
