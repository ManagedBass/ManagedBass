using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
        /// Retrieves information on a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="Info"><see cref="ChannelInfo" /> instance where to store the channel information at.</param>
        /// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError"/> to get the error code.
        /// </returns>
        /// <remarks>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not valid.</description>
        /// </item>
        /// </list>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetInfo")]
        public static extern bool ChannelGetInfo(int Handle, out ChannelInfo Info);

        /// <summary>
        /// Retrieves information on a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns>An instance of the <see cref="ChannelInfo" /> class.</returns>
        /// <remarks>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not valid.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        public static ChannelInfo ChannelGetInfo(int Handle)
        {
            ChannelInfo temp;
            ChannelGetInfo(Handle, out temp);
            return temp;
        }

        /// <summary>
        /// Sets up a User DSP function on a stream, MOD music, or recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Procedure">The callback function (see <see cref="DSPProcedure" />).</param>
        /// <param name="User">User instance data to pass to the callback function.</param>
        /// <param name="Priority">
        /// The priority of the new DSP, which determines it's position in the DSP chain.
        /// DSPs with higher priority are called before those with lower.
        /// </param>
        /// <returns>
        /// If succesful, then the new DSP's Handle is returned, else 0 is returned. 
        /// Use <see cref="LastError"/> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>The channel does not have to be playing to set a DSP function, they can be set before and while playing.</para>
        /// <para>
        /// Equally, you can also remove them at any time. 
        /// Use <see cref="ChannelRemoveDSP"/> to remove a DSP function.
        /// </para>
        /// <para>
        /// Multiple DSP functions may be used per channel, in which case the order that the functions are called is determined by their priorities. 
        /// Any DSPs that have the same priority are called in the order that they were added.
        /// </para>
        /// <para>
        /// DSP functions can be applied to MOD musics and streams, but not samples. 
        /// If you want to apply a DSP function to a sample, then you should stream the sample.
        /// </para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSetDSP")]
        public static extern int ChannelSetDSP(int Handle, DSPProcedure Procedure, IntPtr User = default(IntPtr), int Priority = 0);

        /// <summary>
        /// Starts (or resumes) playback of a sample, stream, MOD music, or recording.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL / HMUSIC / HSTREAM / HRECORD Handle.</param>
        /// <param name="Restart">
        /// Restart playback from the beginning? If Handle is a User stream, it's current Buffer contents are flushed. 
        /// If it's a MOD music, it's BPM/etc are automatically reset to their initial values.
        /// </param>
        /// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError"/> to get the error code.
        /// </returns>
        /// <remarks>
        /// When streaming in blocks (<see cref="BassFlags.StreamDownloadBlocks"/>), the restart parameter is ignored as it's not possible to go back to the start.
        /// The <paramref name="Restart" /> parameter is also of no consequence with recording channels.
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.OutputNotStarted"/></term>
        ///     <description>The output is paused/stopped, use <see cref="Start" /> to start it.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DecodingChannel"/></term>
        ///     <description>The channel is not playable, it's a "decoding channel".</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.BufferLost"/></term>
        ///     <description>Should not happen... check that a valid window Handle was used with <see cref="Init"/>.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.NoHardwareVoicesAvailable"/></term>
        ///     <description>
        ///         No hardware voices are available (HCHANNEL only). 
        ///         This only occurs if the sample was loaded/created with the <see cref="BassFlags.VAM"/> flag, 
        ///         and <see cref="VAMMode.Hardware"/> is set in the sample's VAM mode,
        ///         and there are no hardware voices available to play it.
        ///     </description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelPlay")]
        public static extern bool ChannelPlay(int Handle, bool Restart = false);

        /// <summary>
        /// Pauses a sample, stream, MOD music, or recording.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL / HMUSIC / HSTREAM / HRECORD Handle.</param>
        /// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>Use <see cref="ChannelPlay" /> to resume a paused channel. <see cref="ChannelStop" /> can be used to stop a paused channel.
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.NotPlaying"/></term>
        ///     <description>The channel is not playing (or <paramref name="Handle" /> is not a valid channel).</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DecodingChannel"/></term>
        ///     <description>The channel is not playable, it's a "decoding channel".</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Already"/></term>
        ///     <description>The channel is already paused.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelPause")]
        public static extern bool ChannelPause(int Handle);

        /// <summary>
        /// Stops a sample, stream, MOD music, or recording.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM or HRECORD Handle.</param>
        /// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Stopping a User stream (created with <see cref="CreateStream(int,int,BassFlags,StreamProcedure,IntPtr)" />) will clear its Buffer contents, 
        /// and stopping a sample channel (HCHANNEL) will result in it being freed.
        /// Use <see cref="ChannelPause" /> instead if you wish to stop a User stream and then resume it from the same point.
        /// </para>
        /// <para>
        /// When used with a "decoding channel" (<see cref="BassFlags.Decode"/> was used at creation), 
        /// this function will end the channel at its current position, so that it's not possible to decode any more data from it.
        /// Any <see cref="SyncFlags.End"/> syncs that have been set on the channel will not be triggered by this, they are only triggered when reaching the natural end. 
        /// <see cref="ChannelSetPosition" /> can be used to reset the channel and start decoding again.
        /// </para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelStop")]
        public static extern bool ChannelStop(int Handle);

        /// <summary>
        /// Locks a stream, MOD music or recording channel to the current thread.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HMUSIC, HSTREAM or HRECORD Handle.</param>
        /// <param name="Lock">If <see langword="false" />, unlock the channel, else lock it.</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// Locking a channel prevents other threads from performing most functions on it, including Buffer updates. 
        /// Other threads wanting to access a locked channel will block until it is unlocked, so a channel should only be locked very briefly. 
        /// A channel must be unlocked in the same thread that it was locked.
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelLock")]
        public extern static bool ChannelLock(int Handle, bool Lock);

        /// <summary>
        /// Checks if a sample, stream, or MOD music is active (playing) or stalled. Can also check if a recording is in progress.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns><see cref="PlaybackState" /> indicating whether the state of the channel.
        /// </returns>
        /// <remarks>
        /// <para>
        /// When using this function with a decoding channel, <see cref="PlaybackState.Playing"/> will be returned while there is still data to decode. 
        /// Once the end has been reached, <see cref="PlaybackState.Stopped"/> will be returned. 
        /// <see cref="PlaybackState.Stalled"/> is never returned for decoding channels;
        /// you can tell a decoding channel is stalled if <see cref="ChannelGetData" /> returns less data than requested,
        /// and this function still returns <see cref="PlaybackState.Playing"/>.
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelIsActive")]
        public extern static PlaybackState ChannelIsActive(int Handle);

        /// <summary>
        /// Links two MOD music or stream channels together.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HMUSIC or HSTREAM.</param>
        /// <param name="Channel">The Handle of the channel to have linked with it... a HMUSIC or HSTREAM.</param>
        /// <returns>
        /// If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Linked channels are started/stopped/paused/resumed together.
        /// Links are one-way, for example, channel <paramref name="Channel" /> will be started by channel <paramref name="Handle" />, 
        /// but not vice versa unless another link has been set in that direction.
        /// </para>
        /// <para>
        /// If a linked channel has reached the end, it will not be restarted when a channel it is linked to is started. 
        /// If you want a linked channel to be restarted, you need to have resetted it's position using <see cref="ChannelSetPosition" /> beforehand.
        /// </para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description>At least one of <paramref name="Handle" /> and <paramref name="Channel" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DecodingChannel"/></term>
        ///     <description>At least one of <paramref name="Handle" /> and <paramref name="Channel" /> is a "decoding channel", so can't be linked.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Already"/></term>
        ///     <description><paramref name="Channel" /> is already linked to <paramref name="Handle" />.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Unknown"/></term>
        ///     <description>Some other mystery problem!</description>
        /// </item>
        /// </list>
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Except for on Windows, linked channels on the same device are guaranteed to start playing simultaneously.
        /// On Windows, it is possible for there to be a slight gap between them, but it will generally be shorter (and never longer) than starting them individually.
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSetLink")]
        public extern static bool ChannelSetLink(int Handle, int Channel);

        /// <summary>
        /// Removes a links between two MOD music or stream channels.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HMUSIC or HSTREAM.</param>
        /// <param name="Channel">The Handle of the channel to have unlinked with it... a HMUSIC or HSTREAM.</param>
        /// <returns>
        /// If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Already"/></term>
        ///     <description>Either <paramref name="Channel" /> is not a valid channel, or it is already not linked to <paramref name="Handle" />.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveLink")]
        public extern static bool ChannelRemoveLink(int Handle, int Channel);

        /// <summary>
        /// Removes a DSP function from a stream, MOD music, or recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="DSP">Handle of the DSP function to remove from the channel (return value of a previous <see cref="ChannelSetDSP" /> call).</param>
        /// <returns>If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description>At least one of <paramref name="Handle" /> and <paramref name="DSP" /> is not valid.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveDSP")]
        public extern static bool ChannelRemoveDSP(int Handle, int DSP);

        #region Channel Flags
        /// <summary>
        /// Modifies and retrieves a channel's flags.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Flags">A combination of these flags (see <see cref="BassFlags" />):
        /// <list Type="table">
        /// <item>
        ///     <term><see cref="BassFlags.Loop"/></term>
        ///     <description>Loop the channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.AutoFree"/></term>
        ///     <description>Automatically free the channel when playback ends. (HSTREAM/HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.RestrictDownloadRate"/></term>
        ///     <description>Restrict the download rate. (HSTREAM)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicRamp"/></term>
        ///     <description>Use "normal" ramping. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MuicSensitiveRamping"/></term>
        ///     <description>Use "sensitive" ramping. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicSurround"/></term>
        ///     <description>Use surround sound. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicSurround2"/></term>
        ///     <description>Use surround sound mode 2. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicNonInterpolated"/></term>
        ///     <description>Use non-interpolated mixing. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.SincInterpolation"/></term>
        ///     <description>Use sinc interpolated sample mixing. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicFT2Mod"/></term>
        ///     <description>Use FastTracker 2 .MOD playback. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicPT1Mod"/></term>
        ///     <description>Use ProTracker 1 .MOD playback. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicPositionReset"/></term>
        ///     <description>Stop all notes when seeking. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicPositionResetEx"/></term>
        ///     <description>Stop all notes and reset BPM/etc when seeking. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="BassFlags.MusicStopBack"/></term>
        ///     <description>Stop when a backward jump effect is played. (HMUSIC)</description>
        /// </item>
        /// <item>
        ///     <term>Speaker flags</term>
        ///     <description>Speaker assignment flags. (HSTREAM/HMUSIC)</description>
        /// </item>
        /// </list>
        /// <i>Other attributes may be supported by add-ons, see the documentation</i>
        /// </param>
        /// <param name="Mask">
        /// The flags (as above) to modify. Flags that are not included in this are left as they are, so it can be set to 0 in order to just retrieve the current flags. 
        /// To modify the speaker flags, any of the Speaker flags can be used in the mask (no need to include all of them).
        /// </param>
        /// <returns>
        /// If successful, the channel's updated flags are returned, else -1 is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Some flags may not be adjustable in some circumstances, so the return value should be checked to confirm any changes. 
        /// The flags listed above are just the flags that can be modified, and there may be additional flags present in the return value. 
        /// See the <see cref="ChannelInfo" /> documentation for a full list of flags.
        /// </para>
        /// <para>
        /// Streams that are created by add-ons may have additional flags available. 
        /// There is a limited number of possible flag values though, so some add-ons may use the same flag value for different things. 
        /// This means that when using add-on specific flags with a stream created via the plugin system, 
        /// it is a good idea to first confirm that the add-on is handling the stream, by checking its ctype via <see cref="ChannelGetInfo(int,out ChannelInfo)" />.
        /// </para>
        /// <para>
        /// During playback, the effects of flag changes are not heard instantaneously, due to buffering. 
        /// To reduce the delay, use the <see cref="PlaybackBufferLength" /> config option to reduce the Buffer Length.
        /// </para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelFlags")]
        public extern static BassFlags ChannelFlags(int Handle, BassFlags Flags, BassFlags Mask);

        /// <summary>
        /// Checks if a flag is present on a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Flag">see <see cref="BassFlags" /></param>
        public static bool ChannelHasFlag(int Handle, BassFlags Flag) { return ChannelFlags(Handle, 0, 0).HasFlag(Flag); }

        /// <summary>
        /// Adds a flag to a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Flag">see <see cref="BassFlags" /></param>
        public static bool ChannelAddFlag(int Handle, BassFlags Flag) { return ChannelFlags(Handle, Flag, Flag).HasFlag(Flag); }

        /// <summary>
        /// Removes a flag from a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM.</param>
        /// <param name="Flag">see <see cref="BassFlags" /></param>
        public static bool ChannelRemoveFlag(int Handle, BassFlags Flag) { return !(ChannelFlags(Handle, 0, Flag).HasFlag(Flag)); }
        #endregion

        #region Channel Attributes
        /// <summary>
        /// Retrieves the value of an attribute of a sample, stream or MOD music. 
        /// Can also get the sample rate of a recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="Attribute">The attribute to set the value of (one of <see cref="ChannelAttribute" />)</param>
        /// <param name="Value">Reference to a float to receive the attribute value.</param>
        /// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.IllegalType"/></term>
        ///     <description><paramref name="Attribute" /> is not valid.</description>
        /// </item>
        /// </list>
        /// <i>Some attributes may have additional error codes, see the documentation</i>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetAttribute")]
        public extern static bool ChannelGetAttribute(int Handle, ChannelAttribute Attribute, out float Value);

        public static double ChannelGetAttribute(int Handle, ChannelAttribute Attribute)
        {
            float temp = 0;
            ChannelGetAttribute(Handle, Attribute, out temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetAttributeEx")]
        public static extern int ChannelGetAttribute(int Handle, ChannelAttribute Attribute, IntPtr Value, int Size);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetAttribute")]
        public extern static bool ChannelSetAttribute(int Handle, ChannelAttribute Attribute, float Value);

        public static bool ChannelSetAttribute(int Handle, ChannelAttribute Attribute, double Value)
        {
            return ChannelSetAttribute(Handle, Attribute, (float)Value);
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetAttributeEx")]
        public static extern bool ChannelSetAttribute(int Handle, ChannelAttribute Attribute, IntPtr Value, int Size);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetTags")]
        public extern static IntPtr ChannelGetTags(int Handle, TagType Tags);

        /// <summary>
        /// Removes an effect on a stream, MOD music, or recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="FX">Handle of the effect to remove from the channel (return value of a previous <see cref="ChannelSetFX" /> call).</param>
        /// <returns>
        /// If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// Depending on the DX8 effect implementation being used by the channel, the channel may have to be stopped before removing a DX8 effect on it.
        /// If necessary, that is done automatically and the channel is resumed afterwards.
        /// <para><see cref="ChannelRemoveDSP" /> can also be used to remove effects.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description>At least one of <paramref name="Handle" /> and <paramref name="FX" /> is not valid.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveFX")]
        public extern static bool ChannelRemoveFX(int Handle, int FX);

        /// <summary>
        /// Retrieves the playback Length of a channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM. HSAMPLE handles may also be used.</param>
        /// <param name="Mode">How to retrieve the Length (one of the <see cref="PositionFlags" /> flags):
        /// <list Type="table">
        /// <item>
        ///     <term><see cref="PositionFlags.Bytes"/></term>
        ///     <description>Get the Length in bytes. (default)</description>
        /// </item>
        /// <item>
        ///     <term><see cref="PositionFlags.MusicOrders"/></term>
        ///     <description>Get the Length in orders. (HMUSIC only).</description>
        /// </item>
        /// <item>
        ///     <term><see cref="PositionFlags.OGG"/></term>
        ///     <description>Get the number of bitstreams in an OGG file.</description>
        /// </item>
        /// </list>
        /// <i>Other modes may be supported by add-ons, see the documentation.</i>
        /// </param>
        /// <returns>
        /// If succesful, then the channel's Length is returned, else -1 is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The exact Length of a stream will be returned once the whole file has been streamed, but until then it is not always possible to 100% accurately estimate the Length.
        /// The Length is always exact for MP3/MP2/MP1 files when the <see cref="BassFlags.Prescan"/> flag is used in the <see cref="CreateStream(string,long,long,BassFlags)" /> call, otherwise it is an (usually accurate) estimation based on the file size.
        /// The Length returned for OGG files will usually be exact (assuming the file is not corrupt), but when streaming from the internet (or "buffered" User file), it can be a very rough estimation until the whole file has been downloaded.
        /// It will also be an estimate for chained OGG files that are not pre-scanned.
        /// </para>
        /// <para>Unless an OGG file contains a single bitstream, the number of bitstreams it contains will only be available if it was pre-scanned at the stream's creation.</para>
        /// <para>Retrieving the Length of a MOD music requires that the <see cref="BassFlags.Prescan"/> flag was used in the <see cref="MusicLoad(string,long,int,BassFlags,int)" /> call.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DataNotAvailable"/></term>
        ///     <description>The Length is not available.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <example>
        /// Get the duration (in seconds) of a channel:
        /// <code>
        /// // Length in Bytes
        /// long len = Bass.ChannelGetLength(channel);
        /// // Length in Seconds
        /// double s = Bass.ChannelBytes2Seconds(channel, len);
        /// </code>
        /// <code lang="vbnet">
        /// ' Length in Bytes
        /// Dim len As Long = Bass.ChannelGetLength(channel)
        /// ' Length in Seconds
        /// Dim s As Double = Bass.ChannelBytes2Seconds(channel, len)
        /// </code>
        /// </example>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetLength")]
        public extern static long ChannelGetLength(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        /// <summary>
        /// Sets an effect on a stream, MOD music, or recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Type">Type of effect, one of <see cref="EffectType" />.</param>
        /// <param name="Priority">
        /// The priority of the new FX, which determines it's position in the DSP chain.
        /// DSP/FX with higher priority are applied before those with lower. 
        /// This parameter has no effect with DX8 effects when the "with FX flag" DX8 effect implementation is used.
        /// </param>
        /// <returns>
        /// If succesful, then the new effect's Handle is returned, else 0 is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Multiple effects may be used per channel. Use <see cref="ChannelRemoveFX" /> to remove an effect. 
        /// Use <see cref="FXSetParameters" /> to set an effect's parameters.
        /// </para>
        /// <para>
        /// Effects can be applied to MOD musics and streams, but not samples.
        /// If you want to apply an effect to a sample, you could use a stream instead.
        /// </para>
        /// <para>
        /// Depending on the DX8 effect implementation being used by the channel, the channel may have to be stopped before adding or removing DX8 effects on it. 
        /// If necessary, that is done automatically and the channel is resumed afterwards.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// DX8 effects are a Windows feature requiring DirectX 8, or DirectX 9 for floating-point support.
        /// On other platforms, they are emulated by BASS, except for the following which are currently unsupported: COMPRESSOR, GARGLE, and I3DL2REVERB.
        /// </para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.IllegalType"/></term>
        ///     <description>An illegal <paramref name="Type" /> was specified.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.EffectsNotAvailable"/></term>
        ///     <description>DX8 effects are unavailable.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.UnsupportedSampleFormat"/></term>
        ///     <description>
        ///         The channel's format is not supported by the effect. 
        ///         It may be floating-point (without DX9) or more than stereo.
        ///     </description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Unknown"/></term>
        ///     <description>Some other mystery problem!</description>
        /// </item>
        /// </list>
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// DX8 effects are a Windows feature requiring DirectX 8, or DirectX 9 for floating-point support. 
        /// On other platforms, they are emulated by BASS, except for the following which are currently unsupported: COMPRESSOR, GARGLE, and I3DL2REVERB. 
        /// On Windows CE, only PARAMEQ is supported.
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSetFX")]
        public extern static int ChannelSetFX(int Handle, EffectType Type, int Priority);

        /// <summary>
        /// Sets up a synchronizer on a MOD music, stream or recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="Type">The Type of sync (see <see cref="SyncFlags" />), you may also use these flags:
        /// <list Type="table">
        /// <item> 
        ///     <term><see cref="SyncFlags.Onetime"/></term>
        ///     <description>Call the sync only once, and then remove it from the channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="SyncFlags.Mixtime"/></term>
        ///     <description>
        ///         Call the sync function when the sync occurs during decoding/mixing, instead of delaying the call until the sync is actually heard. 
        ///         This is automatically applied with decoding channels, as they can not be played/heard.
        ///     </description>
        /// </item>
        /// </list>
        /// </param>
        /// <param name="Parameter">The sync parameters, depends on the sync Type (see <see cref="SyncFlags"/>).</param>
        /// <param name="Procedure">The callback function which should be invoked with the sync.</param>
        /// <param name="User">User instance data to pass to the callback function.</param>
        /// <returns>
        /// If succesful, then the new synchronizer's Handle is returned, else 0 is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Multiple synchronizers may be used per channel, and they can be set before and while playing. 
        /// Equally, synchronizers can also be removed at any time, using <see cref="ChannelRemoveSync" />.
        /// If the <see cref="SyncFlags.Onetime"/> flag is used, then the sync is automatically removed after its first occurrence.
        /// </para>
        /// <para>The <see cref="SyncFlags.Mixtime"/> flag can be used with <see cref="SyncFlags.End"/> or <see cref="SyncFlags.Position"/>/<see cref="SyncFlags.MusicPosition"/> syncs to implement custom looping, by using <see cref="ChannelSetPosition" /> in the callback.
        /// A <see cref="SyncFlags.Mixtime"/> sync can also be used to add or remove DSP/FX at specific points, or change a HMUSIC channel's flags or attributes (see <see cref="ChannelFlags" />).
        /// The <see cref="SyncFlags.Mixtime"/> flag can also be useful with a <see cref="SyncFlags.Seeking"/> sync, to reset DSP states after seeking.</para>
        /// <para>
        /// Several of the sync types are triggered in the process of rendering the channel's sample data;
        /// for example, <see cref="SyncFlags.Position"/> and <see cref="SyncFlags.End"/> syncs, when the rendering reaches the sync position or the end, respectively. 
        /// Those sync types should be set before starting playback or pre-buffering (ie. before any rendering), to avoid missing any early sync events.
        /// </para>
        /// <para>With recording channels, <see cref="SyncFlags.Position"/> syncs are triggered just before the <see cref="RecordProcedure" /> receives the block of data containing the sync position.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.IllegalType"/></term>
        ///     <description>An illegal <paramref name="Type" /> was specified.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.IllegalParameter"/></term>
        ///     <description>An illegal <paramref name="Parameter" /> was specified.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSetSync")]
        public static extern int ChannelSetSync(int Handle, SyncFlags Type, long Parameter, SyncProcedure Procedure, IntPtr User = default(IntPtr));

        /// <summary>
        /// Removes a synchronizer from a MOD music or stream channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="Sync">Handle of the synchronizer to remove (return value of a previous <see cref="ChannelSetSync" /> call).</param>
        /// <returns>
        /// If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. 
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description>At least one of <paramref name="Handle" /> and <paramref name="Sync" /> is not valid.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelRemoveSync")]
        public static extern bool ChannelRemoveSync(int Handle, int Sync);

        /// <summary>
        /// Translates a byte position into time (seconds), based on a channel's format.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD. HSAMPLE handles may also be used.</param>
        /// <param name="Position">The position in Bytes to translate.</param>
        /// <returns>If successful, then the translated Length in seconds is returned, else a negative value is returned. Use <see cref="LastError"/> to get the error code.</returns>
        /// <remarks>
        /// The translation is based on the channel's initial sample rate, when it was created.
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelBytes2Seconds")]
        public extern static double ChannelBytes2Seconds(int Handle, long Position);

        /// <summary>
        /// Translates a time (seconds) position into bytes, based on a channel's format.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD. HSAMPLE handles may also be used.</param>
        /// <param name="Position">The position to translate (in seconds, e.g. 0.03 = 30ms).</param>
        /// <returns>
        /// If successful, then the translated Length in Bytes is returned, else -1 is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>The translation is based on the channel's initial sample rate, when it was created.</para>
        /// <para>The return value is rounded down to the position of the nearest sample.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSeconds2Bytes")]
        public extern static long ChannelSeconds2Bytes(int Handle, double Position);

        /// <summary>
        /// Retrieves the playback position of a sample, stream, or MOD music. Can also be used with a recording channel.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="Mode">How to retrieve the position. One of the following (see <see cref="PositionFlags" />):
        /// <list>
        /// <item>
        ///     <term><see cref="PositionFlags.Bytes"/></term>
        ///     <description>Get the position in bytes.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="PositionFlags.MusicOrders"/></term>
        ///     <description>Get the position in orders and rows... LoWord = order, HiWord = row * scaler (<see cref="ChannelAttribute.MusicPositionScaler" />). (HMUSIC only).</description>
        /// </item>
        /// <item>
        ///     <term><see cref="PositionFlags.Decode"/></term>
        ///     <description>
        ///         Flag: Get the decoding/rendering position, which may be ahead of the playback position due to buffering. 
        ///         This flag is unnecessary with decoding channels because the decoding position will always be given for them anyway, as they do not have playback buffers.
        ///     </description>
        /// </item>
        /// </list>
        /// <i>Other modes may be supported by add-ons, see the documentation.</i>
        /// </param>
        /// <returns>
        /// If an error occurs, -1 is returned, use <see cref="LastError" /> to get the error code. 
        /// If successful, the position is returned.
        /// </returns>
        /// <remarks>With MOD music you might use the <see cref="BitHelper.LoWord" /> and <see cref="BitHelper.HiWord" /> methods to retrieve the order and the row values respectively.
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DataNotAvailable"/></term>
        ///     <description>The requested position is not available.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Unknown"/></term>
        ///     <description>Some other mystery problem!</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelGetPosition")]
        public extern static long ChannelGetPosition(int Handle, PositionFlags Mode = PositionFlags.Bytes);

        /// <summary>
        /// Sets the playback position of a sample, MOD music, or stream.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HSTREAM or HMUSIC.</param>
        /// <param name="Position">The position, in units determined by the <paramref name="Mode" />.</param>
        /// <param name="Mode">How to set the position. One of <see cref="PositionFlags" />, with optional flags.</param>
        /// <returns>
        /// If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Setting the position of a MOD music in bytes (other than 0) requires that the <see cref="BassFlags.Prescan"/> flag was used in the <see cref="MusicLoad(string,long,int,BassFlags,int)" /> call.
        /// When setting the position in orders/rows, the channel's byte position (as reported by <see cref="ChannelGetPosition" />) is reset to 0. 
        /// This is because it's not possible to get the byte position of an order/row position - it's possible that a position may never be played in the normal cause of events, or it may be played multiple times.
        /// </para>
        /// <para>
        /// When changing the position of a MOD music, and the <see cref="BassFlags.MusicPositionReset"/> flag is active on the channel, all notes that were playing before the position changed will be stopped. 
        /// Otherwise, the notes will continue playing until they are stopped in the MOD music. 
        /// When setting the position in bytes, the BPM, "speed" and "global volume" are updated to what they would normally be at the new position. 
        /// Otherwise they are left as they were prior to the postion change, unless the seek position is 0 (the start), in which case they are also reset to the starting values (when using the <see cref="BassFlags.MusicPositionReset"/> flag). 
        /// When the <see cref="BassFlags.MusicPositionResetEx"/> flag is active, the BPM, speed and global volume are reset with every seek.
        /// </para>
        /// <para>
        /// For MP3/MP2/MP1 streams, unless the file is scanned via the <see cref="PositionFlags.Scan"/> or the <see cref="BassFlags.Prescan"/> flag at stream creation, seeking will be approximate but generally still quite accurate. 
        /// Besides scanning, exact seeking can also be achieved with the <see cref="PositionFlags.DecodeTo"/> flag.
        /// </para>
        /// <para>Seeking in internet file (and "buffered" User file) streams is possible once the download has reached the requested position, so long as the file is not being streamed in blocks <see cref="BassFlags.StreamDownloadBlocks"/>.</para>
        /// <para>User streams (created with <see cref="CreateStream(int,int,BassFlags,StreamProcedure,IntPtr)" />) are not seekable, but it is possible to reset a User stream (including its Buffer contents) by setting its position to byte 0.</para>
        /// <para>The <see cref="PositionFlags.DecodeTo"/> flag can be used to seek forwards in streams that are not normally seekable, like custom streams or internet streams that are using the <see cref="BassFlags.StreamDownloadBlocks"/> flag, but it will only go as far as what is currently available; it will not wait for more data to be downloaded, for example. <see cref="ChannelGetPosition" /> can be used to confirm what the new position actually is.</para>
        /// <para>In some cases, particularly when the <see cref="PositionFlags.Inexact"/> flag is used, the new position may not be what was requested. <see cref="ChannelGetPosition" /> can be used to confirm what the new position actually is.</para>
        /// <para>The <see cref="PositionFlags.Scan"/> flag works the same way as the <see cref="CreateStream(string,long,long,BassFlags)" /> <see cref="BassFlags.Prescan"/> flag, and can be used to delay the scanning until after the stream has been created. When a position beyond the end is requested, the call will fail (<see cref="Errors.InvalidPlaybackPosition"/> error code) but the seek table and exact Length will have been scanned. 
        /// When a file has been scanned, all seeking (even without the <see cref="PositionFlags.Scan"/> flag) within the scanned part of it will use the scanned infomation.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader>
        ///     <term><see cref="Errors">ERROR CODE</see></term>
        ///     <description>Description</description>
        /// </listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.NotFileStream"/></term>
        ///     <description>The stream is not a file stream.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.InvalidPlaybackPosition"/></term>
        ///     <description>The requested position is invalid, eg. beyond the end.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.DataNotAvailable"/></term>
        ///     <description>The download has not yet reached the requested position.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.Unknown"/>
        ///     </term><description>Some other mystery problem!</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSetPosition")]
        public extern static bool ChannelSetPosition(int Handle, long Position, PositionFlags Mode = PositionFlags.Bytes);

        /// <summary>
        /// Checks if an attribute (or any attribute) of a sample, stream, or MOD music is sliding.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="Attribute">The attribute to check for sliding (one of the following, see <see cref="ChannelAttribute" /> or 0 for any attribute):
        /// <list Type="table">
        /// <item><term><see cref="ChannelAttribute.EaxMix"/></term><description>EAX wet/dry mix.</description></item>
        /// <item><term><see cref="ChannelAttribute.Frequency"/></term><description>Sample rate.</description></item>
        /// <item><term><see cref="ChannelAttribute.Pan"/></term><description>Panning/balance position.</description></item>
        /// <item><term><see cref="ChannelAttribute.Volume"/></term><description>Volume level.</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicAmplify"/></term><description>Amplification level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicBPM"/></term><description>BPM. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicPanSeparation"/></term><description>Pan separation level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicPositionScaler"/></term><description>Position scaler. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicSpeed"/></term><description>Speed. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeChannel"/></term><description>A channel volume level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeGlobal"/></term><description>Global volume level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeInstrument"/></term><description>An instrument/sample volume level. (HMUSIC)</description></item>
        /// </list>
        /// <i>Other attributes may be supported by add-ons, see the documentation</i>
        /// </param>
        /// <returns>If the attribute (or any) is sliding, then <see langword="true" /> is returned, else <see langword="false" /> is returned.</returns>
        [DllImport(DllName, EntryPoint = "BASS_ChannelIsSliding")]
        public extern static bool ChannelIsSliding(int Handle, ChannelAttribute Attribute);

        /// <summary>
        /// Slides a channel's attribute from its current value to a new value.
        /// </summary>
        /// <param name="Handle">The channel Handle... a HCHANNEL, HSTREAM or HMUSIC, or HRECORD.</param>
        /// <param name="Attribute">The attribute to slide the value of (one of the following, see <see cref="ChannelAttribute" />):
        /// <list Type="table">
        /// <item><term><see cref="ChannelAttribute.EaxMix"/></term><description>EAX wet/dry mix.</description></item>
        /// <item><term><see cref="ChannelAttribute.Frequency"/></term><description>Sample rate.</description></item>
        /// <item><term><see cref="ChannelAttribute.Pan"/></term><description>Panning/balance position.</description></item>
        /// <item><term><see cref="ChannelAttribute.Volume"/></term><description>Volume level.</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicAmplify"/></term><description>Amplification level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicBPM"/></term><description>BPM. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicPanSeparation"/></term><description>Pan separation level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicPositionScaler"/></term><description>Position scaler. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicSpeed"/></term><description>Speed. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeChannel"/></term><description>A channel volume level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeGlobal"/></term><description>Global volume level. (HMUSIC)</description></item>
        /// <item><term><see cref="ChannelAttribute.MusicVolumeInstrument"/></term><description>An instrument/sample volume level. (HMUSIC)</description></item>
        /// </list>
        /// <i>Other attributes may be supported by add-ons, see the documentation</i>
        /// </param>
        /// <param name="Value">The new attribute value. See the attribute's documentation for details on the possible values.</param>
        /// <param name="Time">The Length of time (in milliseconds) that it should take for the attribute to reach the <paramref name="Value" />.</param>
        /// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>This function is similar to <see cref="Bass.ChannelSetAttribute(int,ChannelAttribute,float)" />, except that the attribute is ramped to the value over the specified period of time. 
        /// Another difference is that the value is not pre-checked. If it is invalid, the slide will simply end early.</para>
        /// <para>If an attribute is already sliding, then the old slide is stopped and replaced by the new one.</para>
        /// <para><see cref="Bass.ChannelIsSliding" /> can be used to check if an attribute is currently sliding. A BASS_SYNC_SLIDE sync can also be set via <see cref="Bass.ChannelSetSync" />, to be triggered at the end of a slide. 
        /// The sync will not be triggered in the case of an existing slide being replaced by a new one.</para>
        /// <para>Attribute slides are unaffected by whether the channel is playing, paused or stopped. They carry on regardless.</para>
        /// <para>
        /// <list Type="table">
        /// <listheader><term><see cref="Errors">ERROR CODE</see></term><description>Description</description></listheader>
        /// <item>
        ///     <term><see cref="Errors.InvalidHandle"/></term>
        ///     <description><paramref name="Handle" /> is not a valid channel.</description>
        /// </item>
        /// <item>
        ///     <term><see cref="Errors.IllegalType"/></term>
        ///     <description><paramref name="Attribute" /> is not valid.</description>
        /// </item>
        /// </list>
        /// <i>Some attributes may have additional error codes, see the documentation.</i>
        /// </para>
        /// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_ChannelSlideAttribute")]
        public extern static bool ChannelSlideAttribute(int Handle, ChannelAttribute Attribute, float Value, int Time);

        #region Channel Get Level
        [DllImport(DllName)]
        static extern int BASS_ChannelGetLevel(int Handle);

        public static double ChannelGetLevel(int Channel)
        {
            int Temp = BASS_ChannelGetLevel(Channel);
            return (Temp.LoWord() + Temp.HiWord()) / (2.0 * 32768);
        }

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetLevelEx")]
        public static extern bool ChannelGetLevel(int handle, [In][Out] float[] levels, float length, LevelRetrievalFlags flags);

        public static float[] ChannelGetLevel(int handle, float length, LevelRetrievalFlags flags)
        {
            int n = ChannelGetInfo(handle).Channels;

            if (flags.HasFlag(LevelRetrievalFlags.Mono))
                n = 1;
            else if (flags.HasFlag(LevelRetrievalFlags.Stereo))
                n = 2;

            float[] levels = new float[n];

            if (ChannelGetLevel(handle, levels, length, flags))
                return levels;

            return null;
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetData")]
        public static extern int ChannelGetData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_ChannelUpdate")]
        public static extern bool ChannelUpdate(int handdle, int length);
    }
}