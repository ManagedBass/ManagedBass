using System;

namespace ManagedBass.Dynamics
{
    public enum PlaybackState
    {
        // Summary:
        //     The channel is not active, or handle is not a valid channel.
        Stopped = 0,
        //
        // Summary:
        //     The channel is playing (or recording).
        Playing = 1,
        //
        // Summary:
        //     Playback of the stream has been stalled due to there not being enough sample
        //     data to continue playing. The playback will automatically resume once there's
        //     sufficient data to do so.
        Stalled = 2,
        //
        // Summary:
        //     The channel is paused.
        Paused = 3,
    }

    public enum StreamProcedureType
    {
        // Summary:
        //     Flag to signify that the end of the stream is reached.
        End = -2147483648,
        //
        // Summary:
        //     Create a "push" stream.
        //     Instead of BASS pulling data from a STREAMPROC function, data is pushed to
        //     BASS via Un4seen.Bass.Bass.BASS_StreamPutData(System.Int32,System.IntPtr,System.Int32).
        Push = -1,
        //
        // Summary:
        //     Create a "dummy" stream.
        //     A dummy stream doesn't have any sample data of its own, but a decoding dummy
        //     stream (with BASS_STREAM_DECODE flag) can be used to apply DSP/FX processing
        //     to any sample data, by setting DSP/FX on the stream and feeding the data
        //     through Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32).
        //     The dummy stream should have the same sample format as the data being fed
        //     through it.
        Dummy = 0,
    }

    public enum BASS3DMode
    {
        // Summary:
        //     To be used with Un4seen.Bass.Bass.BASS_ChannelSet3DAttributes(System.Int32,Un4seen.Bass.BASS3DMode,System.Single,System.Single,System.Int32,System.Int32,System.Int32)
        //     in order to leave the current 3D processing mode unchanged.
        LeaveCurrent = -1,
        //
        // Summary:
        //     normal 3D processing
        Normal = 0,
        //
        // Summary:
        //     The channel's 3D position (position/velocity/orientation) are relative to
        //     the listener.
        //     When the listener's position/velocity/orientation is changed with Un4seen.Bass.Bass.BASS_Set3DPosition(Un4seen.Bass.BASS_3DVECTOR,Un4seen.Bass.BASS_3DVECTOR,Un4seen.Bass.BASS_3DVECTOR,Un4seen.Bass.BASS_3DVECTOR),
        //     the channel's position relative to the listener does not change.
        Relative = 1,
        //
        // Summary:
        //     Turn off 3D processing on the channel, the sound will be played in the center.
        Off = 2,
    }

    [Flags]
    public enum BASSVam
    {
        // Summary:
        //     Play the sample in hardware. If no hardware voices are available then the
        //     "play" call will fail
        Hardware = 1,
        //
        // Summary:
        //     Play the sample in software (ie. non-accelerated). No other VAM flags may
        //     be used together with this flag.
        Software = 2,
        //
        // Summary:
        //     If there are no free hardware voices, the buffer to be terminated will be
        //     the one with the least time left to play.
        TerminateTime = 4,
        //
        // Summary:
        //     If there are no free hardware voices, the buffer to be terminated will be
        //     one that was loaded/created with the BASS_SAMPLE_MUTEMAX flag and is beyond
        //     it's max distance. If there are no buffers that match this criteria, then
        //     the "play" call will fail.
        TerminateDistance = 8,
        //
        // Summary:
        //     If there are no free hardware voices, the buffer to be terminated will be
        //     the one with the lowest priority.
        TerminatePriority = 16,
    }

    [Flags]
    public enum LevelRetrievalFlags
    {
        // Summary:
        //     Retrieves mono levels
        All = 0,
        //
        // Summary:
        //     Retrieves mono levels
        Mono = 1,
        //
        // Summary:
        //     Retrieves stereo levels
        Stereo = 2,
        //
        // Summary:
        //     Optional Flag: If set it returns RMS levels instead of peak leavels
        RMS = 4,
    }

    public enum FileStreamPosition
    {
        // Summary:
        //     Position that is to be decoded for playback next. This will be a bit ahead
        //     of the position actually being heard due to buffering.
        Current = 0,
        //
        // Summary:
        //     Download progress of an internet file stream or "buffered" user file stream.
        Download = 1,
        //
        // Summary:
        //     End of the file, in other words the file length. When streaming in blocks,
        //     the file length is unknown, so the download buffer length is returned instead.
        End = 2,
        //
        // Summary:
        //     Start of stream data in the file.
        Start = 3,
        //
        // Summary:
        //     Internet file stream or "buffered" user file stream is still connected? 0
        //     = no, 1 = yes.
        Connected = 4,
        //
        // Summary:
        //     The amount of data in the buffer of an internet file stream or "buffered"
        //     user file stream.
        //     Unless streaming in blocks, this is the same as BASS_FILEPOS_DOWNLOAD.
        Buffer = 5,
        //
        // Summary:
        //     Returns the socket hanlde used for streaming.
        Socket = 6,
        //
        // Summary:
        //     The amount of data in the asynchronous file reading buffer. This requires
        //     that the BASS_ASYNCFILE flag was used at the stream's creation.
        AsyncBuffer = 7,
        //
        // Summary:
        //     WMA add-on: internet buffering progress (0-100%)
        WmaBuffer = 1000,
    }

    [Flags]
    public enum PositionFlags
    {
        // Summary:
        //     Byte position.
        Bytes = 0,
        //
        // Summary:
        //     Order.Row position (HMUSIC only).
        MusicOrders = 1,
        //
        // Summary:
        //     Tick position (MIDI streams only).
        MIDITick = 2,
        //
        // Summary:
        //     OGG bitstream number.
        OGG = 3,
        //
        // Summary:
        //     CD Add-On: the track number.
        CDTrack = 4,
        //
        // Summary:
        //     Midi Add-On: Let the old sound decay naturally (including reverb) when changing
        //     the position, including looping and such can also be used in Un4seen.Bass.Bass.BASS_ChannelSetPosition(System.Int32,System.Int64,Un4seen.Bass.BASSMode)
        //     calls to have it apply to particular position changes.
        MIDIDecaySeek = 16384,
        //
        // Summary:
        //     MOD Music Flag: Stop all notes when moving position.
        MusicPositionReset = 32768,
        //
        // Summary:
        //     MOD Music Flag: Stop all notes and reset bmp/etc when moving position.
        MusicPositionResetEx = 4194304,
        //
        // Summary:
        //     Mixer Flag: Don't ramp-in the start after seeking.
        MixerNoRampIn = 8388608,
        //
        // Summary:
        //     Flag: Allow inexact seeking. For speed, seeking may stop at the beginning
        //     of a block rather than partially processing the block to reach the requested
        //     position.
        Inexact = 134217728,
        //
        // Summary:
        //     Get the decoding (not playing) position.
        Decode = 268435456,
        //
        // Summary:
        //     Flag: decode to the position instead of seeking.
        DecodeTo = 536870912,
        //
        // Summary:
        //     Flag: Scan the file to build a seek table up to the position, if it has not
        //     already been scanned.
        //     Scanning will continue from where it left off previously rather than restarting
        //     from the beginning of the file each time. This flag only applies to MP3/MP2/MP1
        //     files and will be ignored with other file formats.
        Scan = 1073741824,
    }

    public enum ChannelAttribute
    {
        // Summary:
        //     The sample rate of a channel.
        //     freq: The sample rate... 0 = original rate (when the channel was created).
        //     This attribute applies to playback of the channel, and does not affect the
        //     channel's sample data, so has no real effect on decoding channels. It is
        //     still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix
        //     add-on, and anything else that wants to use it.
        //     It is not possible to change the sample rate of a channel if the "with FX
        //     flag" DX8 effect implementation enabled on it, unless DirectX 9 or above
        //     is installed.
        //     Increasing the sample rate of a stream or MOD music increases its CPU usage,
        //     and reduces the length of its playback buffer in terms of time. If you intend
        //     to raise the sample rate above the original rate, then you may also need
        //     to increase the buffer length via the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to avoid break-ups in the sound.
        //     Platform-specific
        //     On Windows, the sample rate will get rounded down to a whole number during
        //     playback.
        Frequency = 1,
        //
        // Summary:
        //     The volume level of a channel.
        //     volume: The volume level... 0 (silent) to 1 (full). This can go above 1.0
        //     on decoding channels.
        //     This attribute applies to playback of the channel, and does not affect the
        //     channel's sample data, so has no real effect on decoding channels. It is
        //     still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix
        //     add-on, and anything else that wants to use it.
        //     When using Un4seen.Bass.Bass.BASS_ChannelSlideAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single,System.Int32)
        //     to slide this attribute, a negative volume value can be used to fade-out
        //     and then stop the channel.
        Volume = 2,
        //
        // Summary:
        //     The panning/balance position of a channel.
        //     pan: The pan position... -1 (full left) to +1 (full right), 0 = centre.
        //     This attribute applies to playback of the channel, and does not affect the
        //     channel's sample data, so has no real effect on decoding channels. It is
        //     still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix
        //     add-on, and anything else that wants to use it.
        //     It is not possible to set the pan position of a 3D channel. It is also not
        //     possible to set the pan position when using speaker assignment, but if needed,
        //     it can be done via a Un4seen.Bass.DSPPROC instead (not on mono channels).
        //     Platform-specific
        //     On Windows, this attribute has no effect when speaker assignment is used,
        //     except on Windows Vista and newer with the Un4seen.Bass.BASSConfig.BASS_CONFIG_VISTA_SPEAKERS
        //     config option enabled. Balance control could be implemented via a Un4seen.Bass.DSPPROC
        //     instead
        Pan = 3,
        //
        // Summary:
        //     The wet (reverb) / dry (no reverb) mix ratio on a sample, stream, or MOD
        //     music channel with 3D functionality.
        //     mix: The wet / dry ratio... 0 (full dry) to 1 (full wet), -1 = automatically
        //     calculate the mix based on the distance (the default).
        //     Obviously, EAX functions have no effect if the output device does not support
        //     EAX. Un4seen.Bass.Bass.BASS_GetInfo(Un4seen.Bass.BASS_INFO) can be used to
        //     check that. EAX only affects 3D channels, but EAX functions do not require
        //     Un4seen.Bass.Bass.BASS_Apply3D() to apply the changes.
        //     Un4seen.Bass.BASSErrorDescription BASS_ERROR_NOEAXThe channel does not have
        //     EAX support. EAX only applies to 3D channels that are mixed by the hardware/drivers.
        //     Un4seen.Bass.Bass.BASS_ChannelGetInfo(System.Int32,Un4seen.Bass.BASS_CHANNELINFO)
        //     can be used to check if a channel is being mixed by the hardware.
        //     EAX is only supported on Windows.
        EaxMix = 4,
        //
        // Summary:
        //     Non-Windows: Disable playback buffering?
        //     nobuffer: Disable playback buffering... 0 = no, else yes..
        //     A playing channel is normally asked to render data to its playback buffer
        //     in advance, via automatic buffer updates or the Un4seen.Bass.Bass.BASS_Update(System.Int32)
        //     and Un4seen.Bass.Bass.BASS_ChannelUpdate(System.Int32,System.Int32) functions,
        //     ready for mixing with other channels to produce the final mix that is given
        //     to the output device.  When this attribute is switched on (the default is
        //     off), that buffering is skipped and the channel will only be asked to produce
        //     data as it is needed during the generation of the final mix. This allows
        //     the lowest latency to be achieved, but also imposes tighter timing requirements
        //     on the channel to produce its data and apply any DSP/FX (and run mixtime
        //     syncs) that are set on it; if too long is taken, there will be a break in
        //     the output, affecting all channels that are playing on the same device.
        //     The channel's data is still placed in its playback buffer when this attribute
        //     is on, which allows Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     and Un4seen.Bass.Bass.BASS_ChannelGetLevel(System.Int32) to be used, although
        //     there is likely to be less data available to them due to the buffer being
        //     less full.
        //     This attribute can be changed mid-playback. If switched on, any already buffered
        //     data will still be played, so that there is no break in sound.
        //     This attribute is not available on Windows, as BASS does not generate the
        //     final mix.
        NoBuffer = 5,
        //
        // Summary:
        //     The CPU usage of a channel.
        //     cpu: The CPU usage (in percentage).
        //     This attribute gives the percentage of CPU that the channel is using, including
        //     the time taken by decoding and DSP processing, and any FX that are not using
        //     the "with FX flag" DX8 effect implementation. It does not include the time
        //     taken to add the channel's data to the final output mix during playback.
        //     The processing of some add-on stream formats may also not be entirely included,
        //     if they use additional decoding threads; see the add-on documentation for
        //     details.
        //     Like Un4seen.Bass.Bass.BASS_GetCPU(), this function does not strictly tell
        //     the CPU usage, but rather how timely the processing is. For example, if it
        //     takes 10ms to generate 100ms of data, that would be 10%. If the reported
        //     usage exceeds 100%, that means the channel's data is taking longer to generate
        //     than to play. The duration of the data is based on the channel's current
        //     sample rate (Un4seen.Bass.BASSAttribute.BASS_ATTRIB_FREQ).
        //     A channel's CPU usage is updated whenever it generates data. That could be
        //     during a playback buffer update cycle, or a Un4seen.Bass.Bass.BASS_Update(System.Int32)
        //     call, or a Un4seen.Bass.Bass.BASS_ChannelUpdate(System.Int32,System.Int32)
        //     call. For a decoding channel, it would be in a Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     or Un4seen.Bass.Bass.BASS_ChannelGetLevel(System.Int32) call.
        //     This attribute is read-only, so cannot be modified via Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single).
        CPUUsage = 7,
        //
        // Summary:
        //     The sample rate conversion quality of a channel.
        //     quality: The sample rate conversion quality... 0 = linear interpolation,
        //     1 = 8 point sinc interpolation, 2 = 16 point sinc interpolation, 3 = 32 point
        //     sinc interpolation. Other values are also accepted but will be interpreted
        //     as 0 or 3, depending on whether they are lower or higher.
        //     When a channel has a different sample rate to what the output device is using,
        //     the channel's sample data will need to be converted to match the output device's
        //     rate during playback. This attribute determines how that is done. The linear
        //     interpolation option uses less CPU, but the sinc interpolation gives better
        //     sound quality (less aliasing), with the quality and CPU usage increasing
        //     with the number of points. A good compromise for lower spec systems could
        //     be to use sinc interpolation for music playback and linear interpolation
        //     for sound effects.
        //     Whenever possible, a channel's sample rate should match the output device's
        //     rate to avoid the need for any sample rate conversion. The device's sample
        //     rate could be used in Un4seen.Bass.Bass.BASS_StreamCreate(System.Int32,System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.STREAMPROC,System.IntPtr)
        //     or Un4seen.Bass.Bass.BASS_MusicLoad(System.String,System.Int64,System.Int32,Un4seen.Bass.BASSFlag,System.Int32)
        //     or Un4seen.Bass.AddOn.Midi stream creation calls, for example.
        //     The sample rate conversion occurs (when required) during playback, after
        //     the sample data has left the channel's playback buffer, so it does not affect
        //     the data delivered by Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32).
        //     Although this attribute has no direct effect on decoding channels, it is
        //     still available so that it can be used by the Un4seen.Bass.AddOn.Mix add-on
        //     and anything else that wants to use it.
        //     This attribute can be set at any time, and changes take immediate effect.
        //     A channel's initial setting is determined by the Un4seen.Bass.BASSConfig.BASS_CONFIG_SRC
        //     config option, or Un4seen.Bass.BASSConfig.BASS_CONFIG_SRC_SAMPLE in the case
        //     of a sample channel.
        //     Platform-specific
        //     On Windows, sample rate conversion is handled by Windows or the output device/driver
        //     rather than BASS, so this setting has no effect on playback there.
        SampleRateConversion = 8,
        //
        // Summary:
        //     The download buffer level required to resume stalled playback.
        //     resume: The resumption level in percent... 0 - 100 (the default is 50%).
        //     This attribute determines what percentage of the download buffer (Un4seen.Bass.BASSConfig.BASS_CONFIG_NET_BUFFER)
        //     needs to be filled before playback of a stalled internet stream will resume.
        //      It also applies to 'buffered' user file streams created with Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr).
        NetworkResumeBufferLevel = 9,
        //
        // Summary:
        //     The scanned info of a channel.
        SacnnedInfo = 10,
        //
        // Summary:
        //     The amplification level of a MOD music.
        //     amp: Amplification level... 0 (min) to 100 (max). This will be rounded down
        //     to a whole number.
        //     As the amplification level get's higher, the sample data's range increases,
        //     and therefore, the resolution increases. But if the level is set too high,
        //     then clipping can occur, which can result in distortion of the sound.
        //     You can check the current level of a MOD music at any time by using Un4seen.Bass.Bass.BASS_ChannelGetLevel(System.Int32).
        //     By doing so, you can decide if a MOD music's amplification level needs adjusting.
        //     The default amplification level is 50.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        MusicAmplify = 256,
        //
        // Summary:
        //     The pan separation level of a MOD music.
        //     pansep: Pan separation... 0 (min) to 100 (max), 50 = linear. This will be
        //     rounded down to a whole number.
        //     By default BASS uses a linear panning "curve". If you want to use the panning
        //     of FT2, use a pan separation setting of around 35. To use the Amiga panning
        //     (ie. full left and right) set it to 100.
        MusicPanSeparation = 257,
        //
        // Summary:
        //     The position scaler of a MOD music.
        //     scale: The scaler... 1 (min) to 256 (max). This will be rounded down to a
        //     whole number.
        //     When calling Un4seen.Bass.Bass.BASS_ChannelGetPosition(System.Int32,Un4seen.Bass.BASSMode),
        //     the row (HIWORD) will be scaled by this value. By using a higher scaler,
        //     you can get a more precise position indication.
        //     The default scaler is 1.
        //     Get the position of a MOD music accurate to within a 10th of a row: // set
        //     the scaler Bass.BASS_ChannelSetAttribute(music, BASSAttribute.BASS_ATTRIB_MUSIC_PSCALER,
        //     10f); int pos = Bass.BASS_MusicGetOrderPosition(music); // the order int
        //     order = Utils.LowWord32(pos); // the row int row = HighWord32(pos) / 10;
        //     // the 10th of a row int row10th = HighWord32(pos) % 10; ' set the scaler
        //     Bass.BASS_ChannelSetAttribute(music, BASSAttribute.BASS_ATTRIB_MUSIC_PSCALER,
        //     10F) Dim pos As Integer = Bass.BASS_MusicGetOrderPosition(music) ' the order
        //     Dim order As Integer = Utils.LowWord32(pos) ' the row Dim row As Integer
        //     = HighWord32(pos) / 10 ' the 10th of a row Dim row10th As Integer = HighWord32(pos)
        //     Mod 10
        MusicPositionScaler = 258,
        //
        // Summary:
        //     The BPM of a MOD music.
        //     bpm: The BPM... 1 (min) to 255 (max). This will be rounded down to a whole
        //     number.
        //     This attribute is a direct mapping of the MOD's BPM, so the value can be
        //     changed via effects in the MOD itself.
        //     Note that by changing this attribute, you are changing the playback length.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        MusicBPM = 259,
        //
        // Summary:
        //     The speed of a MOD music.
        //     speed: The speed... 0 (min) to 255 (max). This will be rounded down to a
        //     whole number.
        //     This attribute is a direct mapping of the MOD's speed, so the value can be
        //     changed via effects in the MOD itself.
        //     The "speed" is the number of ticks per row. Setting it to 0, stops and ends
        //     the music. Note that by changing this attribute, you are changing the playback
        //     length.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        MusicSpeed = 260,
        //
        // Summary:
        //     The global volume level of a MOD music.
        //     volume: The global volume level... 0 (min) to 64 (max, 128 for IT format).
        //     This will be rounded down to a whole number.
        //     This attribute is a direct mapping of the MOD's global volume, so the value
        //     can be changed via effects in the MOD itself.  The "speed" is the number
        //     of ticks per row. Setting it to 0, stops and ends the music. Note that by
        //     changing this attribute, you are changing the playback length.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        MusicVolumeGlobal = 261,
        //
        // Summary:
        //     The number of active channels in a MOD music.
        //     active: The number of channels.
        //     This attribute gives the number of channels (including virtual) that are
        //     currently active in the decoder, which may not match what is being heard
        //     during playback due to buffering. To reduce the time difference, use the
        //     Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER config option to reduce the buffer
        //     length.
        //     This attribute is read-only, so cannot be modified via Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single).
        MusicActiveChannelCount = 262,
        //
        // Summary:
        //     The volume level of a channel in a MOD music + channel#.
        //     channel: The channel to set the volume of... 0 = 1st channel.
        //     volume: The volume level... 0 (silent) to 1 (full).
        //     The volume curve used by this attribute is always linear, eg. 0.5 = 50%.
        //     The Un4seen.Bass.BASSConfig.BASS_CONFIG_CURVE_VOL config option setting has
        //     no effect on this. The volume level of all channels is initially 1 (full).
        //     This attribute can also be used to count the number of channels in a MOD
        //     Music.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        //     Count the number of channels in a MOD music: int channels = 0; float dummy;
        //     while (Bass.BASS_ChannelGetAttribute(music, (BASSAttribute)((int)BASS_ATTRIB_MUSIC_VOL_CHAN
        //     + channels), ref dummy)) { channels++; } Dim channels As Integer = 0 Dim
        //     dummy As Single While Bass.BASS_ChannelGetAttribute(music, CType(CInt(BASS_ATTRIB_MUSIC_VOL_CHAN)
        //     + channels, BASSAttribute), dummy) channels += 1 End While
        MusicVolumeChannel = 512,
        //
        // Summary:
        //     The volume level of an instrument in a MOD music + inst#.
        //     inst: The instrument to set the volume of... 0 = 1st instrument.
        //     volume: The volume level... 0 (silent) to 1 (full).
        //     The volume curve used by this attribute is always linear, eg. 0.5 = 50%.
        //     The Un4seen.Bass.BASSConfig.BASS_CONFIG_CURVE_VOL config option setting has
        //     no effect on this. The volume level of all instruments is initially 1 (full).
        //     For MOD formats that do not use instruments, read "sample" for "instrument".
        //     This attribute can also be used to count the number of instruments in a MOD
        //     music.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        //     config option to reduce the buffer length.
        //     Count the number of instruments in a MOD music: int instruments = 0; float
        //     dummy; while (Bass.BASS_ChannelGetAttribute(music, (BASSAttribute)((int)BASSAttribute.BASS_ATTRIB_MUSIC_VOL_INST
        //     + instruments), ref dummy)) { instruments++; } Dim instruments As Integer
        //     = 0 Dim dummy As Single While Bass.BASS_ChannelGetAttribute(music, CType(CInt(BASSAttribute.BASS_ATTRIB_MUSIC_VOL_INST)
        //     + instruments, BASSAttribute), dummy) instruments += 1 End While
        MusicVolumeInstrument = 768,
        //
        // Summary:
        //     BASS_FX Tempo: The Tempo in percents (-95%..0..+5000%).
        Tempo = 65536,
        //
        // Summary:
        //     BASS_FX Tempo: The Pitch in semitones (-60..0..+60).
        Pitch = 65537,
        //
        // Summary:
        //     BASS_FX Tempo: The Samplerate in Hz, but calculates by the same % as BASS_ATTRIB_TEMPO.
        TempoFrequency = 65538,
        //
        // Summary:
        //     BASS_FX Tempo Option: Use FIR low-pass (anti-alias) filter (gain speed, lose
        //     quality)? true=1 (default), false=0.
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        //     On iOS, Android, WinCE and Linux ARM platforms this is by default disabled
        //     for lower CPU usage.
        TempoUseAAFilter = 65552,
        //
        // Summary:
        //     BASS_FX Tempo Option: FIR low-pass (anti-alias) filter length in taps (8
        //     .. 128 taps, default = 32, should be %4).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoAAFilterLength = 65553,
        //
        // Summary:
        //     BASS_FX Tempo Option: Use quicker tempo change algorithm (gain speed, lose
        //     quality)? true=1, false=0 (default).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoUseQuickAlgorithm = 65554,
        //
        // Summary:
        //     BASS_FX Tempo Option: Tempo Sequence in milliseconds (default = 82).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoSequenceMilliseconds = 65555,
        //
        // Summary:
        //     BASS_FX Tempo Option: SeekWindow in milliseconds (default = 14).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoSeekWindowMilliseconds = 65556,
        //
        // Summary:
        //     BASS_FX Tempo Option: Tempo Overlap in milliseconds (default = 12).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoOverlapMilliseconds = 65557,
        //
        // Summary:
        //     BASS_FX Tempo Option: Prevents clicks with tempo changes (default = FALSE).
        //     See Un4seen.Bass.AddOn.Fx.BassFx.BASS_FX_TempoCreate(System.Int32,Un4seen.Bass.BASSFlag)
        //     for details.
        TempoPreventClick = 65558,
        //
        // Summary:
        //     BASS_FX Reverse: The Playback direction (-1=BASS_FX_RVS_REVERSE or 1=BASS_FX_RVS_FORWARD).
        ReverseDirection = 69632,
        //
        // Summary:
        //     BASSMIDI: Gets the Pulses Per Quarter Note (or ticks per beat) value of the
        //     MIDI file.
        //     ppqn: The PPQN value.
        //     This attribute is the number of ticks per beat as defined by the MIDI file;
        //     it will be 0 for MIDI streams created via Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamCreate(System.Int32,Un4seen.Bass.BASSFlag,System.Int32),
        //     It is also read-only, so can't be modified via Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single).
        //     Get the currnet position of a MIDI stream in beats: float ppqn; Bass.BASS_ChannelGetAttribute(midi,
        //     BASSAttribute.BASS_ATTRIB_MIDI_PPQN, ref ppqn); long tick = Bass.BASS_ChannelGetPosition(midi,
        //     BASSMode.BASS_POS_MIDI_TICK); float beat = tick / ppqn; Dim ppqn As Single
        //     Bass.BASS_ChannelGetAttribute(midi, BASSAttribute.BASS_ATTRIB_MIDI_PPQN,
        //     ppqn) Dim tick As Long = Bass.BASS_ChannelGetPosition(midi, BASSMode.BASS_POS_MIDI_TICK)
        //     Dim beat As Single = tick / ppqn
        MidiPPQN = 73728,
        //
        // Summary:
        //     BASSMIDI: The maximum percentage of CPU time that a MIDI stream can use.
        //     limit: The CPU usage limit... 0 to 100, 0 = unlimited.
        //     It is not strictly the CPU usage that is measured, but rather how timely
        //     the stream is able to render data. For example, a limit of 50% would mean
        //     that the rendering would need to be at least 2x real-time speed. When the
        //     limit is exceeded, BASSMIDI will begin killing voices, starting with the
        //     most quiet.
        //     When the CPU usage is limited, the stream's samples are loaded asynchronously
        //     so that any loading delays (eg. due to slow disk) do not hold up the stream
        //     for too long. If a sample cannot be loaded in time, then it will be silenced
        //     until it is available and the stream will continue playing other samples
        //     as normal in the meantime.  This does not affect sample loading via Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamLoadSamples(System.Int32),
        //     which always operates synchronously.
        //     By default, a MIDI stream will have no CPU limit.
        MidiCPU = 73729,
        //
        // Summary:
        //     BASSMIDI: The number of MIDI channels in a MIDI stream.
        //     channels: The number of MIDI channels... 1 (min) - 128 (max). For a MIDI
        //     file stream, the minimum is 16.
        //     New channels are melodic by default. Any notes playing on a removed channel
        //     are immediately stopped.
        MidiChannels = 73730,
        //
        // Summary:
        //     BASSMIDI: The maximum number of samples to play at a time (polyphony) in
        //     a MIDI stream.
        //     voices: The number of voices... 1 (min) - 1000 (max).
        //     If there are currently more voices active than the new limit, then some voices
        //     will be killed to meet the limit. The number of voices currently active is
        //     available via the Un4seen.Bass.BASSAttribute.BASS_ATTRIB_MIDI_VOICES_ACTIVE
        //     attribute.
        //     A MIDI stream will initially have a default number of voices as determined
        //     by the Un4seen.Bass.BASSConfig.BASS_CONFIG_MIDI_VOICES config option.
        MidiVoices = 73731,
        //
        // Summary:
        //     BASSMIDI: The number of samples currently playing in a MIDI stream.
        //     voices: The number of voices.
        //     This attribute is read-only, so cannot be modified via Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single).
        MidiVoicesActive = 73732,
        //
        // Summary:
        //     BASSMIDI: The volume level of a track in a MIDI file stream + track#.
        //     track#: The track to set the volume of... 0 = first track.
        //     volume: The volume level (0.0=silent, 1.0=normal/default).
        //     The volume curve used by this attribute is always linear, eg. 0.5 = 50%.
        //     The BASS_CONFIG_CURVE_VOL config option setting has no effect on this.
        //     During playback, the effect of changes to this attribute are not heard instantaneously,
        //     due to buffering. To reduce the delay, use the BASS_CONFIG_BUFFER config
        //     option to reduce the buffer length.
        //     This attribute can also be used to count the number of tracks in a MIDI file
        //     stream. MIDI streams created via Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamCreate(System.Int32,Un4seen.Bass.BASSFlag,System.Int32)
        //     do not have any tracks.
        //     Count the number of tracks in a MIDI stream: int tracks = 0; float dummy;
        //     while (Bass.BASS_ChannelGetAttribute(midi, (BASSAttribute)((int)BASSAttribute.BASS_ATTRIB_MIDI_TRACK_VOL
        //     + tracks), ref dummy)) { tracks++; } Dim tracks As Integer = 0 Dim dummy
        //     As Single While Bass.BASS_ChannelGetAttribute(midi, CType(CInt(BASSAttribute.BASS_ATTRIB_MIDI_TRACK_VOL)
        //     + tracks, BASSAttribute), dummy) tracks += 1 End While
        MidiTrackVolume = 73984
    }

    public enum EffectType
    {
        // Summary:
        //     DX8 Chorus. Use Un4seen.Bass.BASS_DX8_CHORUS structure to set/get parameters.
        DXChorus = 0,
        //
        // Summary:
        //     DX8 Compressor. Use Un4seen.Bass.BASS_DX8_COMPRESSOR structure to set/get
        //     parameters.
        DXCompressor = 1,
        //
        // Summary:
        //     DX8 Distortion. Use Un4seen.Bass.BASS_DX8_DISTORTION structure to set/get
        //     parameters.
        DXDistortion = 2,
        //
        // Summary:
        //     DX8 Echo. Use Un4seen.Bass.BASS_DX8_ECHO structure to set/get parameters.
        DXEcho = 3,
        //
        // Summary:
        //     DX8 Flanger. Use Un4seen.Bass.BASS_DX8_FLANGER structure to set/get parameters.
        DXFlanger = 4,
        //
        // Summary:
        //     DX8 Gargle. Use Un4seen.Bass.BASS_DX8_GARGLE structure to set/get parameters.
        DXGargle = 5,
        //
        // Summary:
        //     DX8 I3DL2 (Interactive 3D Audio Level 2) reverb. Use Un4seen.Bass.BASS_DX8_I3DL2REVERB
        //     structure to set/get parameters.
        DX_I3DL2Reverb = 6,
        //
        // Summary:
        //     DX8 Parametric equalizer. Use Un4seen.Bass.BASS_DX8_PARAMEQ structure to
        //     set/get parameters.
        DXParamEQ = 7,
        //
        // Summary:
        //     DX8 Reverb. Use Un4seen.Bass.BASS_DX8_REVERB structure to set/get parameters.
        DXReverb = 8,
        //
        // Summary:
        //     BASS_FX Channel Volume Ping-Pong (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_ROTATE
        //     structure to set/get parameters.
        Rotate = 65536,
        //
        // Summary:
        //     BASS_FX Volume control (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_VOLUME
        //     structure to set/get parameters.
        Volume = 65539,
        //
        // Summary:
        //     BASS_FX Peaking Equalizer (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PEAKEQ
        //     structure to set/get parameters.
        PeakEQ = 65540,
        //
        // Summary:
        //     BASS_FX Channel Swap/Remap/Downmix (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_MIX
        //     structure to set/get parameters.
        Mix = 65543,
        //
        // Summary:
        //     BASS_FX Dynamic Amplification (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_DAMP
        //     structure to set/get parameters.
        Damp = 65544,
        //
        // Summary:
        //     BASS_FX Auto WAH (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_AUTOWAH
        //     structure to set/get parameters.
        AutoWah = 65545,
        //
        // Summary:
        //     BASS_FX Phaser (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PHASER
        //     structure to set/get parameters.
        Phaser = 65547,
        //
        // Summary:
        //     BASS_FX Chorus (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_CHORUS
        //     structure to set/get parameters.
        Chorus = 65549,
        //
        // Summary:
        //     BASS_FX Distortion (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_DISTORTION
        //     structure to set/get parameters.
        Distortion = 65552,
        //
        // Summary:
        //     BASS_FX Dynamic Range Compressor (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_COMPRESSOR2
        //     structure to set/get parameters.
        Compressor = 65553,
        //
        // Summary:
        //     BASS_FX Volume Envelope (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_VOLUME_ENV
        //     structure to set/get parameters.
        VolumeEnvelope = 65554,
        //
        // Summary:
        //     BASS_FX BiQuad filters (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_BQF
        //     structure to set/get parameters.
        BQF = 65555,
        //
        // Summary:
        //     BASS_FX Echo/Reverb 4 (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_ECHO4
        //     structure to set/get parameters.
        Echo = 65556,
        //
        // Summary:
        //     BASS_FX Pitch Shift using FFT (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_PITCHSHIFT
        //     structure to set/get parameters.
        PitchShift = 65557,
        //
        // Summary:
        //     BASS_FX Pitch Shift using FFT (multi channel). Use Un4seen.Bass.AddOn.Fx.BASS_BFX_FREEVERB
        //     structure to set/get parameters.
        Freeverb = 65558,
    }

    [Flags]
    public enum DeviceInfoFlags : int
    {
        // Summary:
        //     Bitmask to identify the device type.
        TypeMask = -16777216,
        //
        // Summary:
        //     The device is not enabled and not initialized.
        None = 0,
        //
        // Summary:
        //     The device is enabled. It will not be possible to initialize the device if
        //     this flag is not present.
        Enabled = 1,
        //
        // Summary:
        //     The device is the system default.
        Default = 2,
        //
        // Summary:
        //     The device is initialized, ie. Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     or Un4seen.Bass.Bass.BASS_RecordInit(System.Int32) has been called.
        Initialized = 4,
        //
        // Summary:
        //     An audio endpoint Device that the user accesses remotely through a network.
        Network = 16777216,
        //
        // Summary:
        //     A set of speakers.
        Speakers = 33554432,
        //
        // Summary:
        //     An audio endpoint Device that sends a line-level analog signal to a line-input
        //     jack on an audio adapter or that receives a line-level analog signal from
        //     a line-output jack on the adapter.
        Line = 50331648,
        //
        // Summary:
        //     A set of headphones.
        Headphones = 67108864,
        //
        // Summary:
        //     A microphone.
        Microphone = 83886080,
        //
        // Summary:
        //     An earphone or a pair of earphones with an attached mouthpiece for two-way
        //     communication.
        Headset = 100663296,
        //
        // Summary:
        //     The part of a telephone that is held in the hand and that contains a speaker
        //     and a microphone for two-way communication.
        Handset = 117440512,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a connector
        //     for a digital interface of unknown type.
        Digital = 134217728,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a Sony/Philips
        //     Digital Interface (S/PDIF) connector.
        SPDIF = 150994944,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a High-Definition
        //     Multimedia Interface (HDMI) connector.
        HDMI = 167772160,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a DisplayPort
        //     connector.
        DisplayPort = 1073741824,
    }

    [Flags]
    public enum BassFlags
    {
        // Summary:
        //     File is a Unicode (16-bit characters) filename
        Unicode = -2147483648,
        //
        // Summary:
        //     0 = default create stream: 16 Bit, stereo, no Float, hardware mixing, no
        //     Loop, no 3D, no speaker assignments...
        Default = 0,
        //
        // Summary:
        //     BASS_FX add-on: If in use, then you can do other stuff while detection's
        //     in process.
        BASS_FX_BPM_BKGRND = 1,
        //
        // Summary:
        //     BASSMIDI add-on: Don't send a WAVE header to the encoder. If this flag is
        //     used then the sample format (mono 16-bit) must be passed to the encoder some
        //     other way, eg. via the command-line.
        BASS_MIDI_PACK_NOHEAD = 1,
        //
        // Summary:
        //     Use 8-bit resolution. If neither this or the BASS_SAMPLE_FLOAT flags are
        //     specified, then the stream is 16-bit.
        Byte = 1,
        //
        // Summary:
        //     BASSMIDI add-on: Reduce 24-bit sample data to 16-bit before encoding.
        BASS_MIDI_PACK_16BIT = 2,
        //
        // Summary:
        //     Decode/play the stream (MP3/MP2/MP1 only) in mono, reducing the CPU usage
        //     (if it was originally stereo).
        //     This flag is automatically applied if BASS_DEVICE_MONO was specified when
        //     calling Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr).
        //
        // Summary:
        //     Music: Decode/play the mod music in mono, reducing the CPU usage (if it was
        //     originally stereo).  This flag is automatically applied if BASS_DEVICE_MONO
        //     was specified when calling Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr).
        Mono = 2,
        //
        // Summary:
        //     BASS_FX add-on: If in use, then will auto multiply bpm by 2 (if BPM < MinBPM*2)
        BASS_FX_BPM_MULT2 = 2,
        //
        // Summary:
        //     Music: Loop the music. This flag can be toggled at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        //
        // Summary:
        //     Loop the file. This flag can be toggled at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        Loop = 4,
        //
        // Summary:
        //     Use 3D functionality. This is ignored if BASS_DEVICE_3D wasn't specified
        //     when calling Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr).
        //     3D streams must be mono (chans=1). The SPEAKER flags can not be used together
        //     with this flag.
        //
        // Summary:
        //     Music: Use 3D functionality. This is ignored if BASS_DEVICE_3D wasn't specified
        //     when calling Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr).
        //     3D streams must be mono (chans=1). The SPEAKER flags can not be used together
        //     with this flag.
        Bass3D = 8,
        //
        // Summary:
        //     Force the stream to not use hardware mixing.
        BASS_SAMPLE_SOFTWARE = 16,
        //
        // Summary:
        //     Sample: muted at max distance (3D only)
        BASS_SAMPLE_MUTEMAX = 32,
        //
        // Summary:
        //     Sample: uses the DX7 voice allocation & management
        BASS_SAMPLE_VAM = 64,
        //
        // Summary:
        //     Enable the old implementation of DirectX 8 effects. See the DX8 effect implementations
        //     section for details.
        //     Use Un4seen.Bass.Bass.BASS_ChannelSetFX(System.Int32,Un4seen.Bass.BASSFXType,System.Int32)
        //     to add effects to the stream. Requires DirectX 8 or above.
        //
        // Summary:
        //     Music: Enable the old implementation of DirectX 8 effects. See the DX8 effect
        //     implementations section for details.
        //     Use Un4seen.Bass.Bass.BASS_ChannelSetFX(System.Int32,Un4seen.Bass.BASSFXType,System.Int32)
        //     to add effects to the stream. Requires DirectX 8 or above.
        FX = 128,
        //
        // Summary:
        //     Music: Use 32-bit floating-point sample data (see Floating-Point Channels
        //     for details). WDM drivers or the BASS_STREAM_DECODE flag are required to
        //     use this flag.
        //
        // Summary:
        //     Use 32-bit floating-point sample data (see Floating-Point Channels for details).
        //     WDM drivers or the BASS_STREAM_DECODE flag are required to use this flag.
        Float = 256,
        //
        // Summary:
        //     Music: Use "normal" ramping (as used in FastTracker 2).
        BASS_MUSIC_RAMP = 512,
        //
        // Summary:
        //     BASS_FX add-on (AddOn.Fx.BassFx.BASS_FX_TempoCreate): Uses a linear interpolation
        //     mode (simple).
        BASS_FX_TEMPO_ALGO_LINEAR = 512,
        //
        // Summary:
        //     BASSCD add-on: Read sub-channel data. 96 bytes of de-interleaved sub-channel
        //     data will be returned after each 2352 bytes of audio. This flag can not be
        //     used with the BASS_SAMPLE_FLOAT flag, and is ignored if the BASS_STREAM_DECODE
        //     flag is not used.
        BASS_CD_SUBCHANNEL = 512,
        //
        // Summary:
        //     BASSCD add-on: Read sub-channel data, without using any hardware de-interleaving.
        //     This is identical to the BASS_CD_SUBCHANNEL flag, except that the de-interleaving
        //     is always performed by BASSCD even if the drive is apparently capable of
        //     de-interleaving itself.
        BASS_CD_SUBCHANNEL_NOHW = 1024,
        //
        // Summary:
        //     BASS_FX add-on (AddOn.Fx.BassFx.BASS_FX_TempoCreate): Uses a cubic interpolation
        //     mode (recommended, default).
        BASS_FX_TEMPO_ALGO_CUBIC = 1024,
        //
        // Summary:
        //     Music: Use "sensitive" ramping.
        BASS_MUSIC_RAMPS = 1024,
        //
        // Summary:
        //     BASSMIDI add-on: Ignore system reset events (MIDI_EVENT_SYSTEM) when the
        //     system mode is unchanged. This flag can be toggled at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        BASS_MIDI_NOSYSRESET = 2048,
        //
        // Summary:
        //     BASS_FX add-on (AddOn.Fx.BassFx.BASS_FX_TempoCreate): Uses a 8-tap band-limited
        //     Shannon interpolation (complex, but not much better than cubic).
        BASS_FX_TEMPO_ALGO_SHANNON = 2048,
        //
        // Summary:
        //     Music: Apply XMPlay's surround sound to the music (ignored in mono).
        BASS_MUSIC_SURROUND = 2048,
        //
        // Summary:
        //     BASSCD add-on: Include C2 error info. 296 bytes of C2 error info is returned
        //     after each 2352 bytes of audio (and optionally 96 bytes of sub-channel data).
        //      This flag cannot be used with the BASS_SAMPLE_FLOAT flag, and is ignored
        //     if the BASS_STREAM_DECODE flag is not used.
        //     The first 294 bytes contain the C2 error bits (one bit for each byte of audio),
        //     followed by a byte containing the logical OR of all 294 bytes, which can
        //     be used to quickly check if there were any C2 errors. The final byte is just
        //     padding.
        //     Note that if you request both sub-channel data and C2 error info, the C2
        //     info will come before the sub-channel data!
        BASS_CD_C2ERRORS = 2048,
        //
        // Summary:
        //     BASSmix add-on: filter the sample data when resampling (only available/used
        //     in pre BASSmix v2.4.7).
        BASS_MIXER_FILTER = 4096,
        //
        // Summary:
        //     BASSmix add-on: only read buffered data.
        BASS_SPLIT_SLAVE = 4096,
        //
        // Summary:
        //     Music: Apply XMPlay's surround sound mode 2 to the music (ignored in mono).
        BASS_MUSIC_SURROUND2 = 4096,
        //
        // Summary:
        //     BASSMIDI add-on: Let the sound decay naturally (including reverb) instead
        //     of stopping it abruptly at the end of the file. This flag can be toggled
        //     at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        BASS_MIDI_DECAYEND = 4096,
        //
        // Summary:
        //     BASSmix add-on: resume a stalled mixer immediately upon new/unpaused source
        BASS_MIXER_RESUME = 4096,
        //
        // Summary:
        //     BASSmix add-on: enable Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetPositionEx(System.Int32,Un4seen.Bass.BASSMode,System.Int32)
        //     support.
        BASS_MIXER_POSEX = 8192,
        //
        // Summary:
        //     Music: Play .MOD file as FastTracker 2 would.
        BASS_MUSIC_FT2MOD = 8192,
        //
        // Summary:
        //     BASSMIDI add-on: Disable the MIDI reverb/chorus processing. This flag can
        //     be toggled at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        BASS_MIDI_NOFX = 8192,
        //
        // Summary:
        //     BASSmix add-on: buffer source data for Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     and Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetLevel(System.Int32).
        BASS_MIXER_BUFFER = 8192,
        //
        // Summary:
        //     BASSMIDI add-on: Let the old sound decay naturally (including reverb) when
        //     changing the position, including looping. This flag can be toggled at any
        //     time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag),
        //     and can also be used in Un4seen.Bass.Bass.BASS_ChannelSetPosition(System.Int32,System.Int64,Un4seen.Bass.BASSMode)
        //     calls to have it apply to particular position changes.
        BASS_MIDI_DECAYSEEK = 16384,
        //
        // Summary:
        //     Music: Play .MOD file as ProTracker 1 would.
        BASS_MUSIC_PT1MOD = 16384,
        //
        // Summary:
        //     BASSmix add-on: Limit mixer processing to the amount available from this
        //     source.
        BASS_MIXER_LIMIT = 16384,
        //
        // Summary:
        //     Music: Stop all notes when seeking (using Un4seen.Bass.Bass.BASS_ChannelSetPosition(System.Int32,System.Int64,Un4seen.Bass.BASSMode)).
        BASS_MUSIC_POSRESET = 32768,
        //
        // Summary:
        //     Recording: Start the recording paused. Use Un4seen.Bass.Bass.BASS_ChannelPlay(System.Int32,System.Boolean)
        //     to start it.
        RecordPause = 32768,
        //
        // Summary:
        //     BASSMIDI add-on: Do not remove empty space (containing no events) from the
        //     end of the file.
        BASS_MIDI_NOCROP = 32768,
        //
        // Summary:
        //     Music: Use non-interpolated mixing. This generally reduces the sound quality,
        //     but can be good for chip-tunes.
        BASS_MUSIC_NONINTER = 65536,
        //
        // Summary:
        //     BASSMIDI add-on: Only release the oldest instance upon a note off event (MIDI_EVENT_NOTE
        //     with velocity=0) when there are overlapping instances of the note. Otherwise
        //     all instances are released. This flag can be toggled at any time using Un4seen.Bass.Bass.BASS_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        BASS_MIDI_NOTEOFF1 = 65536,
        //
        // Summary:
        //     BASS_FX add-on: Free the source handle as well?
        FXFreeSource = 65536,
        //
        // Summary:
        //     Sample: override lowest volume
        BASS_SAMPLE_OVER_VOL = 65536,
        //
        // Summary:
        //     BASSmix add-on: end the stream when there are no sources
        BASS_MIXER_END = 65536,
        //
        // Summary:
        //     BASSmix add-on: Matrix mixing
        BASS_MIXER_MATRIX = 65536,
        //
        // Summary:
        //     Enable pin-point accurate seeking (to the exact byte) on the MP3/MP2/MP1
        //     stream.
        //     This also increases the time taken to create the stream, due to the entire
        //     file being pre-scanned for the seek points.  Note: BASS_STREAM_PRESCAN is
        //     ONLY needed for files with a VBR, files with a CBR are always accurate.
        //
        // Summary:
        //     Music: Calculate the playback length of the music, and enable seeking in
        //     bytes. This slightly increases the time taken to load the music, depending
        //     on how long it is.
        //     In the case of musics that loop, the length until the loop occurs is calculated.
        //     Use Un4seen.Bass.Bass.BASS_ChannelGetLength(System.Int32,Un4seen.Bass.BASSMode)
        //     to retrieve the length.
        Prescan = 131072,
        //
        // Summary:
        //     BASSMIDI add-on: Map the file into memory. This flag is ignored if the soundfont
        //     is packed as the sample data cannot be played directly from a mapping; it
        //     needs to be decoded. This flag is also ignored if the file is too large to
        //     be mapped into memory.
        BASS_MIDI_FONT_MMAP = 131072,
        //
        // Summary:
        //     BASSmix add-on: don't stall when there are no sources
        BASS_MIXER_NONSTOP = 131072,
        //
        // Summary:
        //     Sample: override longest playing
        BASS_SAMPLE_OVER_POS = 131072,
        //
        // Summary:
        //     BASSmix add-on: don't process the source
        BASS_MIXER_PAUSE = 131072,
        //
        // Summary:
        //     Sample: override furthest from listener (3D only)
        BASS_SAMPLE_OVER_DIST = 196608,
        //
        // Summary:
        //     Automatically free the stream's resources when it has reached the end, or
        //     when Un4seen.Bass.Bass.BASS_ChannelStop(System.Int32) (or Un4seen.Bass.Bass.BASS_Stop())
        //     is called.
        //
        // Summary:
        //     Music: Automatically free the music when it ends. This allows you to play
        //     a music and forget about it, as BASS will automatically free the music's
        //     resources when it has reached the end or when Un4seen.Bass.Bass.BASS_ChannelStop(System.Int32)
        //     (or Un4seen.Bass.Bass.BASS_Stop()) is called.
        //     Note that some musics never actually end on their own (ie. without you stopping
        //     them).
        AutoFree = 262144,
        //
        // Summary:
        //     Restrict the download rate of the file to the rate required to sustain playback.
        //     If this flag is not used, then the file will be downloaded as quickly as
        //     possible.  This flag has no effect on "unbuffered" streams (buffer=false).
        BASS_STREAM_RESTRATE = 524288,
        //
        // Summary:
        //     Music: Stop the music when a backward jump effect is played. This stops musics
        //     that never reach the end from going into endless loops.
        //     Some MOD musics are designed to jump all over the place, so this flag would
        //     cause those to be stopped prematurely. If this flag is used together with
        //     the BASS_SAMPLE_LOOP flag, then the music would not be stopped but any BASS_SYNC_END
        //     sync would be triggered.
        BASS_MUSIC_STOPBACK = 524288,
        //
        // Summary:
        //     Download and play the file in smaller chunks.
        //     Uses a lot less memory than otherwise, but it's not possible to seek or loop
        //     the stream - once it's ended, the file must be opened again to play it again.
        //     This flag will automatically be applied when the file length is unknown.
        //     This flag also has the effect of resticting the download rate. This flag
        //     has no effect on "unbuffered" streams (buffer=false).
        BASS_STREAM_BLOCK = 1048576,
        //
        // Summary:
        //     Music: Don't load the samples. This reduces the time taken to load the music,
        //     notably with MO3 files, which is useful if you just want to get the name
        //     and length of the music without playing it.
        BASS_MUSIC_NOSAMPLE = 1048576,
        //
        // Summary:
        //     Decode the sample data, without outputting it. Use Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     to retrieve decoded sample data.
        //     BASS_SAMPLE_SOFTWARE/3D/FX/AUTOFREE are all ignored when using this flag,
        //     as are the SPEAKER flags.
        //
        // Summary:
        //     Music: Decode the music into sample data, without outputting it.
        //     Use Un4seen.Bass.Bass.BASS_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     to retrieve decoded sample data. BASS_SAMPLE_SOFTWARE/3D/FX/AUTOFREE are
        //     ignored when using this flag, as are the SPEAKER flags.
        Decode = 2097152,
        //
        // Summary:
        //     Music: Stop all notes and reset bpm/etc when seeking.
        BASS_MUSIC_POSRESETEX = 4194304,
        //
        // Summary:
        //     BASSmix add-on: downmix to stereo (or mono if mixer is mono)
        BASS_MIXER_DOWNMIX = 4194304,
        //
        // Summary:
        //     BASSMIDI add-on: Use sinc interpolated sample mixing. This increases the
        //     sound quality, but also requires more CPU. Otherwise linear interpolation
        //     is used.
        BASS_MIDI_SINCINTER = 8388608,
        //
        // Summary:
        //     BASSmix add-on: don't ramp-in the start
        BASS_MIXER_NORAMPIN = 8388608,
        //
        // Summary:
        //     Pass status info (HTTP/ICY tags) from the server to the DOWNLOADPROC callback
        //     during connection.
        //     This can be useful to determine the reason for a failure.
        BASS_STREAM_STATUS = 8388608,
        //
        // Summary:
        //     Music: Sinc interpolated sample mixing.  This increases the sound quality,
        //     but also requires quite a bit more processing. If neither this or the BASS_MUSIC_NONINTER
        //     flag is specified, linear interpolation is used.
        BASS_MUSIC_SINCINTER = 8388608,
        //
        // Summary:
        //     Front speakers (channel 1/2)
        BASS_SPEAKER_FRONT = 16777216,
        //
        // Summary:
        //     speakers Pair 1
        BASS_SPEAKER_PAIR1 = 16777216,
        //
        // Summary:
        //     speakers Pair 2
        BASS_SPEAKER_PAIR2 = 33554432,
        //
        // Summary:
        //     Rear/Side speakers (channel 3/4)
        BASS_SPEAKER_REAR = 33554432,
        //
        // Summary:
        //     speakers Pair 3
        BASS_SPEAKER_PAIR3 = 50331648,
        //
        // Summary:
        //     Center & LFE speakers (5.1, channel 5/6)
        BASS_SPEAKER_CENLFE = 50331648,
        //
        // Summary:
        //     speakers Pair 4
        BASS_SPEAKER_PAIR4 = 67108864,
        //
        // Summary:
        //     Rear Center speakers (7.1, channel 7/8)
        BASS_SPEAKER_REAR2 = 67108864,
        //
        // Summary:
        //     speakers Pair 5
        BASS_SPEAKER_PAIR5 = 83886080,
        //
        // Summary:
        //     Speakers Pair 6
        BASS_SPEAKER_PAIR6 = 100663296,
        //
        // Summary:
        //     Speakers Pair 7
        BASS_SPEAKER_PAIR7 = 117440512,
        //
        // Summary:
        //     Speakers Pair 8
        BASS_SPEAKER_PAIR8 = 134217728,
        //
        // Summary:
        //     Speakers Pair 9
        BASS_SPEAKER_PAIR9 = 150994944,
        //
        // Summary:
        //     Speakers Pair 10
        BASS_SPEAKER_PAIR10 = 167772160,
        //
        // Summary:
        //     Speakers Pair 11
        BASS_SPEAKER_PAIR11 = 184549376,
        //
        // Summary:
        //     Speakers Pair 12
        BASS_SPEAKER_PAIR12 = 201326592,
        //
        // Summary:
        //     Speakers Pair 13
        BASS_SPEAKER_PAIR13 = 218103808,
        //
        // Summary:
        //     Speakers Pair 14
        BASS_SPEAKER_PAIR14 = 234881024,
        //
        // Summary:
        //     Speakers Pair 15
        BASS_SPEAKER_PAIR15 = 251658240,
        //
        // Summary:
        //     Speaker Modifier: left channel only
        BASS_SPEAKER_LEFT = 268435456,
        //
        // Summary:
        //     Front Left speaker only (channel 1)
        BASS_SPEAKER_FRONTLEFT = 285212672,
        //
        // Summary:
        //     Rear/Side Left speaker only (channel 3)
        BASS_SPEAKER_REARLEFT = 301989888,
        //
        // Summary:
        //     Center speaker only (5.1, channel 5)
        BASS_SPEAKER_CENTER = 318767104,
        //
        // Summary:
        //     Rear Center Left speaker only (7.1, channel 7)
        BASS_SPEAKER_REAR2LEFT = 335544320,
        //
        // Summary:
        //     Speaker Modifier: right channel only
        BASS_SPEAKER_RIGHT = 536870912,
        //
        // Summary:
        //     Front Right speaker only (channel 2)
        BASS_SPEAKER_FRONTRIGHT = 553648128,
        //
        // Summary:
        //     Rear/Side Right speaker only (channel 4)
        BASS_SPEAKER_REARRIGHT = 570425344,
        //
        // Summary:
        //     LFE speaker only (5.1, channel 6)
        BASS_SPEAKER_LFE = 587202560,
        //
        // Summary:
        //     Rear Center Right speaker only (7.1, channel 8)
        BASS_SPEAKER_REAR2RIGHT = 603979776,
        //
        // Summary:
        //     Use an async look-ahead cache.
        BASS_ASYNCFILE = 1073741824,
    }

    [Flags]
    public enum SyncFlags
    {
        // Summary:
        //     FLAG: sync only once, else continuously
        Onetime = -2147483648,
        //
        // Summary:
        //     Sync when a channel reaches a position.
        //     param : position in bytes
        //     data : not used
        Position = 0,
        //
        // Summary:
        //     Sync when an instrument (sample for the non-instrument based formats) is
        //     played in a MOD music (not including retrigs).
        //     param : LOWORD=instrument (1=first) HIWORD=note (0=c0...119=b9, -1=all)
        //     data : LOWORD=note HIWORD=volume (0-64)
        BASS_SYNC_MUSICINST = 1,
        //
        // Summary:
        //     Sync when a channel reaches the end.
        //     param : not used
        //     data : 1 = the sync is triggered by a backward jump in a MOD music, otherwise
        //     not used
        End = 2,
        //
        // Summary:
        //     Sync when the "sync" effect (XM/MTM/MOD: E8x/Wxx, IT/S3M: S2x) is used.
        //     param : 0:data=pos, 1:data="x" value
        //     data : param=0: LOWORD=order HIWORD=row, param=1: "x" value
        BASS_SYNC_MUSICFX = 3,
        //
        // Summary:
        //     Sync when metadata is received in a stream.
        //     param : not used
        //     data : not used - the updated metadata is available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag)
        //     (BASS_TAG_META)
        MetadataReceived = 4,
        //
        // Summary:
        //     Sync when an attribute slide is completed.
        //     param : not used
        //     data : the type of slide completed (one of the BASS_SLIDE_xxx values)
        Slided = 5,
        //
        // Summary:
        //     Sync when playback has stalled.
        //     param : not used
        //     data : 0=stalled, 1=resumed
        Stalled = 6,
        //
        // Summary:
        //     Sync when downloading of an internet (or "buffered" user file) stream has
        //     ended.
        //     param : not used
        //     data : not used
        Downloaded = 7,
        //
        // Summary:
        //     Sync when a channel is freed.
        //     param : not used
        //     data : not used
        Freed = 8,
        //
        // Summary:
        //     Sync when a MOD music reaches an order:row position.
        //     param : LOWORD=order (0=first, -1=all) HIWORD=row (0=first, -1=all)
        //     data : LOWORD=order HIWORD=row
        BASS_SYNC_MUSICPOS = 10,
        //
        // Summary:
        //     Sync when seeking (inc. looping and restarting). So it could be used to reset
        //     DSP/etc.
        //     param : position in bytes
        //     data : 0=playback is unbroken, 1=if is it broken (eg. buffer flushed). The
        //     latter would be the time to reset DSP/etc.
        Seeking = 11,
        //
        // Summary:
        //     Sync when a new logical bitstream begins in a chained OGG stream. Updated
        //     tags are available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used
        BASS_SYNC_OGG_CHANGE = 12,
        //
        // Summary:
        //     Sync when the DirectSound buffer fails during playback, eg. when the device
        //     is no longer available.
        //     param : not used
        //     data : not used
        BASS_SYNC_STOP = 14,
        //
        // Summary:
        //     WINAMP add-on: Sync when bitrate is changed or retrieved from a winamp input
        //     plug-in.
        //     param : not used
        //     data : the bitrate retrieved from the winamp input plug-in - called when
        //     it is retrieved or changed (VBR MP3s, OGGs, etc).
        BASS_WINAMP_SYNC_BITRATE = 100,
        //
        // Summary:
        //     CD add-on: Sync when playback is stopped due to an error. For example, the
        //     drive door being opened.
        //     param : not used
        //     data : the position that was being read from the CD track at the time.
        CDError = 1000,
        //
        // Summary:
        //     CD add-on: Sync when the read speed is automatically changed due to the BASS_CONFIG_CD_AUTOSPEED
        //     setting.
        //     param : not used
        //     data : the new read speed.
        BASS_SYNC_CD_SPEED = 1002,
        //
        // Summary:
        //     MIDI add-on: Sync when a marker is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_MARKER,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_MARKER, data) ...  End Sub
        BASS_SYNC_MIDI_MARKER = 65536,
        //
        // Summary:
        //     MIDI add-on: Sync when a cue is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_CUE,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_CUE, data) ...  End Sub
        BASS_SYNC_MIDI_CUE = 65537,
        //
        // Summary:
        //     MIDI add-on: Sync when a lyric event is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.  If the text begins with a '/' (slash) character, a new line should
        //     be started. If it begins with a '\' (backslash) character, the display should
        //     be cleared.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_LYRIC,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_LYRIC, data) ...  End Sub
        BASS_SYNC_MIDI_LYRIC = 65538,
        //
        // Summary:
        //     MIDI add-on: Sync when a text event is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.  Lyrics can sometimes be found in BASS_MIDI_MARK_TEXT instead of BASS_MIDI_MARK_LYRIC
        //     markers.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_SYNC_MIDI_TEXT,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_SYNC_MIDI_TEXT, data) ...  End Sub
        BASS_SYNC_MIDI_TEXT = 65539,
        //
        // Summary:
        //     MIDI add-on: Sync when a type of event is processed, in either a MIDI file
        //     or Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamEvent(System.Int32,System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIEvent,System.Int32).
        //     param : event type (0 = all types).
        //     data : LOWORD = event parameter, HIWORD = channel (high 8 bits contain the
        //     event type when syncing on all types). See Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamEvent(System.Int32,System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIEvent,System.Int32)
        //     for a list of event types and their parameters.
        BASS_SYNC_MIDI_EVENT = 65540,
        //
        // Summary:
        //     MIDI add-on: Sync when reaching a tick position.
        //     param : tick position.
        //     data : not used
        BASS_SYNC_MIDI_TICK = 65541,
        //
        // Summary:
        //     MIDI add-on: Sync when a time signature event is processed.
        //     param : event type.
        //     data : The time signature events are given (by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK))
        //     in the form of "numerator/denominator metronome-pulse 32nd-notes-per-MIDI-quarter-note",
        //     eg. "4/4 24 8".
        BASS_SYNC_MIDI_TIMESIG = 65542,
        //
        // Summary:
        //     MIDI add-on: Sync when a key signature event is processed.
        //     param : event type.
        //     data : The key signature events are given (by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK))
        //     in the form of "a b", where a is the number of sharps (if positive) or flats
        //     (if negative), and b signifies major (if 0) or minor (if 1).
        BASS_SYNC_MIDI_KEYSIG = 65543,
        //
        // Summary:
        //     WMA add-on: Sync on a track change in a server-side playlist. Updated tags
        //     are available via Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used
        BASS_SYNC_WMA_CHANGE = 65792,
        //
        // Summary:
        //     WMA add-on: Sync on a mid-stream tag change in a server-side playlist. Updated
        //     tags are available via Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used - the updated metadata is available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag)
        //     (BASS_TAG_WMA_META)
        BASS_SYNC_WMA_META = 65793,
        //
        // Summary:
        //     MIX add-on: Sync when an envelope reaches the end.
        //     param : not used
        //     data : envelope type
        BASS_SYNC_MIXER_ENVELOPE = 66048,
        //
        // Summary:
        //     MIX add-on: Sync when an envelope node is reached.
        //     param : Optional limit the sync to a certain envelope type (one of the Un4seen.Bass.AddOn.Mix.BASSMIXEnvelope
        //     values).
        //     data : Will contain the envelope type in the LOWORD and the current node
        //     number in the HIWORD.
        BASS_SYNC_MIXER_ENVELOPE_NODE = 66049,
        //
        // Summary:
        //     FLAG: sync at mixtime, else at playtime
        Mixtime = 1073741824,
    }

    [Flags]
    public enum InputTypeFlags
    {
        // Summary:
        //     The type of input is also indicated in the high 8-bits of Un4seen.Bass.Bass.BASS_RecordGetInput(System.Int32,System.Single@)
        //     (use BASS_INPUT_TYPE_MASK to test the return value).
        BASS_INPUT_TYPE_MASK = -16777216,
        //
        // Summary:
        //     The type of input is errorness.
        Error = -1,
        //
        // Summary:
        //     Anything that is not covered by the other types
        Undefined = 0,
        //
        // Summary:
        //     Digital input source, for example, a DAT or audio CD.
        Digital = 16777216,
        //
        // Summary:
        //     Line-in. On some devices, "Line-in" may be combined with other analog sources
        //     into a single BASS_INPUT_TYPE_ANALOG input.
        Line = 33554432,
        //
        // Summary:
        //     Microphone.
        Microphone = 50331648,
        //
        // Summary:
        //     Internal MIDI synthesizer.
        MIDISynthesizer = 67108864,
        //
        // Summary:
        //     Analog audio CD.
        AnalogCD = 83886080,
        //
        // Summary:
        //     Telephone.
        Phone = 100663296,
        //
        // Summary:
        //     PC speaker.
        Speaker = 117440512,
        //
        // Summary:
        //     The device's WAVE/PCM output.
        Wave = 134217728,
        //
        // Summary:
        //     Auxiliary. Like "Line-in", "Aux" may be combined with other analog sources
        //     into a single BASS_INPUT_TYPE_ANALOG input on some devices.
        Auxiliary = 150994944,
        //
        // Summary:
        //     Analog, typically a mix of all analog sources (what you hear).
        Analog = 167772160,
    }

    [Flags]
    public enum RecordFormatFlags
    {
        // Summary:
        //     unknown format
        WAVE_FORMAT_UNKNOWN = 0,
        //
        // Summary:
        //     11.025 kHz, Mono, 8-bit
        WAVE_FORMAT_1M08 = 1,
        //
        // Summary:
        //     11.025 kHz, Stereo, 8-bit
        WAVE_FORMAT_1S08 = 2,
        //
        // Summary:
        //     11.025 kHz, Mono, 16-bit
        WAVE_FORMAT_1M16 = 4,
        //
        // Summary:
        //     11.025 kHz, Stereo, 16-bit
        WAVE_FORMAT_1S16 = 8,
        //
        // Summary:
        //     22.05 kHz, Mono, 8-bit
        WAVE_FORMAT_2M08 = 16,
        //
        // Summary:
        //     22.05 kHz, Stereo, 8-bit
        WAVE_FORMAT_2S08 = 32,
        //
        // Summary:
        //     22.05 kHz, Mono, 16-bit
        WAVE_FORMAT_2M16 = 64,
        //
        // Summary:
        //     22.05 kHz, Stereo, 16-bit
        WAVE_FORMAT_2S16 = 128,
        //
        // Summary:
        //     44.1 kHz, Mono, 8-bit
        WAVE_FORMAT_4M08 = 256,
        //
        // Summary:
        //     44.1 kHz, Stereo, 8-bit
        WAVE_FORMAT_4S08 = 512,
        //
        // Summary:
        //     44.1 kHz, Mono, 16-bit
        WAVE_FORMAT_4M16 = 1024,
        //
        // Summary:
        //     44.1 kHz, Stereo, 16-bit
        WAVE_FORMAT_4S16 = 2048,
        //
        // Summary:
        //     48 kHz, Mono, 8-bit
        WAVE_FORMAT_48M08 = 4096,
        //
        // Summary:
        //     48 kHz, Stereo, 8-bit
        WAVE_FORMAT_48S08 = 8192,
        //
        // Summary:
        //     48 kHz, Mono, 16-bit
        WAVE_FORMAT_48M16 = 16384,
        //
        // Summary:
        //     48 kHz, Stereo, 16-bit
        WAVE_FORMAT_48S16 = 32768,
        //
        // Summary:
        //     96 kHz, Mono, 8-bit
        WAVE_FORMAT_96M08 = 65536,
        //
        // Summary:
        //     96 kHz, Stereo, 8-bit
        WAVE_FORMAT_96S08 = 131072,
        //
        // Summary:
        //     96 kHz, Mono, 16-bit
        WAVE_FORMAT_96M16 = 262144,
        //
        // Summary:
        //     96 kHz, Stereo, 16-bit
        WAVE_FORMAT_96S16 = 524288,
    }

    [Flags]
    public enum RecordInfoFlags
    {
        // Summary:
        //     Non of the flags is set
        None = 0,
        //
        // Summary:
        //     The device's drivers do NOT have DirectSound support, so it is being emulated.
        //     Updated drivers should be installed.
        EmulatedDrivers = 32,
        //
        // Summary:
        //     The device driver has been certified by Microsoft. This flag is always set
        //     on WDM drivers.
        Certified = 64,
    }

    [Flags]
    public enum InputFlags
    {
        // Summary:
        //     No input flag change.
        None = 0,
        //
        // Summary:
        //     Disable the input. This flag can't be used when the device supports only
        //     one input at a time.
        Off = 65536,
        //
        // Summary:
        //     Enable the input. If the device only allows one input at a time, then any
        //     previously enabled input will be disabled by this.
        On = 131072,
    }

    [Flags]
    public enum DeviceInitFlags : int
    {
        // Summary:
        //     0 = 16 bit, stereo, no 3D, no Latency calc, no Speaker Assignments
        Default = 0,
        //
        // Summary:
        //     Use 8 bit resolution, else 16 bit.
        Byte = 1,
        //
        // Summary:
        //     Use mono, else stereo.
        Mono = 2,
        //
        // Summary:
        //     Enable 3D functionality.
        //     Note: If the BASS_DEVICE_3D flag is not specified when initilizing BASS,
        //     then the 3D flags (BASS_SAMPLE_3D and BASS_MUSIC_3D) are ignored when loading/creating
        //     a sample/stream/music.
        Device3D = 4,
        //
        // Summary:
        //     Calculate device latency (BASS_INFO struct).
        Latency = 256,
        //
        // Summary:
        //     Use the Windows control panel setting to detect the number of speakers.
        //     Only use this option if BASS doesn't detect the correct number of supported
        //     speakers automatically and you want to force BASS to use the number of speakers
        //     as configured in the windows control panel.
        CPSpeakers = 1024,
        //
        // Summary:
        //     Force enabling of speaker assignment (always 8 speakers will be used regardless
        //     if the soundcard supports them).
        //     Only use this option if BASS doesn't detect the correct number of supported
        //     speakers automatically and you want to force BASS to use 8 speakers.
        ForcedSpeakerAssignment = 2048,
        //
        // Summary:
        //     ignore speaker arrangement
        NoSpeakerAssignment = 4096,
        //
        // Summary:
        //     Set the device's output rate to freq, otherwise leave it as it is.
        Frequency = 16384,
    }

    [Flags]
    public enum ChannelTypeFlags
    {
        // Summary:
        //     Unknown channel format.
        Unknown = 0,
        //
        // Summary:
        //     Sample channel. (HCHANNEL)
        Sample = 1,
        //
        // Summary:
        //     Recording channel. (HRECORD)
        Recording = 2,
        //
        // Summary:
        //     MO3 format music.
        MO3 = 256,
        //
        // Summary:
        //     User sample stream. This can also be used as a flag to test if the channel
        //     is any kind of HSTREAM.
        Stream = 65536,
        //
        // Summary:
        //     OGG format stream.
        OGG = 65538,
        //
        // Summary:
        //     MP1 format stream.
        MP1 = 65539,
        //
        // Summary:
        //     MP2 format stream.
        MP2 = 65540,
        //
        // Summary:
        //     MP3 format stream.
        MP3 = 65541,
        //
        // Summary:
        //     WAV format stream.
        AIFF = 65542,
        //
        // Summary:
        //     CoreAudio codec stream. Additional information is avaliable via the Un4seen.Bass.BASS_TAG_CACODEC
        //     tag.
        CA = 65543,
        //
        // Summary:
        //     Media Foundation codec stream. Additional information is avaliable via the
        //     Un4seen.Bass.BASSTag.BASS_TAG_MF tag.
        MF = 65544,
        //
        // Summary:
        //     Audio-CD, CDA
        CD = 66048,
        //
        // Summary:
        //     WMA format stream.
        WMA = 66304,
        //
        // Summary:
        //     MP3 over WMA format stream.
        WMA_MP3 = 66305,
        //
        // Summary:
        //     Winamp input format stream.
        WINAMP = 66560,
        //
        // Summary:
        //     WavPack Lossless format stream.
        WV = 66816,
        //
        // Summary:
        //     WavPack Hybrid Lossless format stream.
        WV_H = 66817,
        //
        // Summary:
        //     WavPack Lossy format stream.
        WV_L = 66818,
        //
        // Summary:
        //     WavPack Hybrid Lossy format stream.
        WV_LH = 66819,
        //
        // Summary:
        //     Optimfrog format stream.
        OFR = 67072,
        //
        // Summary:
        //     APE format stream.
        APE = 67328,
        //
        // Summary:
        //     BASSmix mixer stream.
        Mixer = 67584,
        //
        // Summary:
        //     BASSmix splitter stream.
        Split = 67585,
        //
        // Summary:
        //     FLAC format stream.
        FLAC = 67840,
        //
        // Summary:
        //     FLAC OGG format stream.
        FLAC_OGG = 67841,
        //
        // Summary:
        //     MPC format stream.
        MPC = 68096,
        //
        // Summary:
        //     AAC format stream.
        AAC = 68352,
        //
        // Summary:
        //     MP4 format stream.
        MP4 = 68353,
        //
        // Summary:
        //     Speex format stream.
        SPX = 68608,
        //
        // Summary:
        //     MIDI sound format stream.
        MIDI = 68864,
        //
        // Summary:
        //     Apple Lossless (ALAC) format stream.
        ALAC = 69120,
        //
        // Summary:
        //     TTA format stream.
        TTA = 69376,
        //
        // Summary:
        //     AC3 format stream.
        AC3 = 69632,
        //
        // Summary:
        //     Video format stream.
        Video = 69888,
        //
        // Summary:
        //     Opus format stream.
        OPUS = 70144,
        //
        // Summary:
        //     Direct Stream Digital (DSD) format stream.
        DSD = 71424,
        //
        // Summary:
        //     ADX format stream.
        //     ADX is a lossy proprietary audio storage and compression format developed
        //     by CRI Middleware specifically for use in video games, it is derived from
        //     ADPCM.
        ADX = 126976,
        //
        // Summary:
        //     AIX format stream.
        //     Only for ADX of all versions (with AIXP support).
        AIX = 126977,
        //
        // Summary:
        //     BASS_FX tempo stream.
        Tempo = 127488,
        //
        // Summary:
        //     BASS_FX reverse stream.
        Reverse = 127489,
        //
        // Summary:
        //     MOD format music. This can also be used as a flag to test if the channel
        //     is any kind of HMUSIC.
        MOD = 131072,
        //
        // Summary:
        //     MTM format music.
        MTM = 131073,
        //
        // Summary:
        //     S3M format music.
        S3M = 131074,
        //
        // Summary:
        //     XM format music.
        XM = 131075,
        //
        // Summary:
        //     IT format music.
        IT = 131076,
        //
        // Summary:
        //     WAV format stream, LOWORD=codec.
        WAV = 262144,
        //
        // Summary:
        //     WAV format stream, PCM 16-bit.
        WAV_PCM = 327681,
        //
        // Summary:
        //     WAV format stream, FLOAT 32-bit.
        WAV_FLOAT = 327683,
    }

    [Flags]
    public enum BASSInfoFlags
    {
        // Summary:
        //     Non of the falgs are set
        None = 0,
        //
        // Summary:
        //     The device supports all sample rates between minrate and maxrate.
        ContinuousRate = 16,
        //
        // Summary:
        //     The device's drivers do NOT have DirectSound support, so it is being emulated.
        //     Updated drivers should be installed.
        EmulatedDrivers = 32,
        //
        // Summary:
        //     The device driver has been certified by Microsoft. This flag is always set
        //     on WDM drivers.
        Certified = 64,
        //
        // Summary:
        //     Mono samples are supported by hardware mixing.
        Mono = 256,
        //
        // Summary:
        //     Stereo samples are supported by hardware mixing.
        Stereo = 512,
        //
        // Summary:
        //     8-bit samples are supported by hardware mixing.
        Secondary8Bit = 1024,
        //
        // Summary:
        //     16-bit samples are supported by hardware mixing.
        Secondary16Bit = 2048,
    }

    public enum Configuration
    {
        PlaybackBufferLength = 0,

        UpdatePeriod = 1,
        //
        // Summary:
        //     Global sample volume.
        //     volume (int): Sample global volume level... 0 (silent) - 10000 (full).
        //     This config option allows you to have control over the volume levels of all
        //     the samples, which is useful for setup options (eg. separate music and fx
        //     volume controls).
        //     A channel's final volume = channel volume * global volume / max volume. So,
        //     for example, if a stream channel's volume is 0.5 and the global stream volume
        //     is 8000, then effectively the stream's volume level is 0.4 (0.5 * 8000 /
        //     10000 = 0.4).
        GlobalSampleVolume = 4,
        //
        // Summary:
        //     Global stream volume.
        //     volume (int): Stream global volume level... 0 (silent) - 10000 (full).
        //     This config option allows you to have control over the volume levels of all
        //     streams, which is useful for setup options (eg. separate music and fx volume
        //     controls).
        //     A channel's final volume = channel volume * global volume / max volume. So,
        //     for example, if a stream channel's volume is 0.5 and the global stream volume
        //     is 8000, then effectively the stream's volume level is 0.4 (0.5 * 8000 /
        //     10000 = 0.4).
        GlobalStreamVolume = 5,
        //
        // Summary:
        //     Global music volume.
        //     volume (int): MOD music global volume level... 0 (silent) - 10000 (full).
        //     This config option allows you to have control over the volume levels of all
        //     the MOD musics, which is useful for setup options (eg. separate music and
        //     fx volume controls).
        //     A channel's final volume = channel volume * global volume / max volume. So,
        //     for example, if a stream channel's volume is 0.5 and the global stream volume
        //     is 8000, then effectively the stream's volume level is 0.4 (0.5 * 8000 /
        //     10000 = 0.4).
        GlobalMusicVolume = 6,
        //
        // Summary:
        //     Volume translation curve.
        //     logvol (bool): Volume curve... false = linear, true = logarithmic.
        //     DirectSound uses logarithmic volume and panning curves, which can be awkward
        //     to work with. For example, with a logarithmic curve, the audible difference
        //     between 10000 and 9000, is not the same as between 9000 and 8000. With a
        //     linear "curve" the audible difference is spread equally across the whole
        //     range of values, so in the previous example the audible difference between
        //     10000 and 9000, and between 9000 and 8000 would be identical.
        //     When using the linear curve, the volume range is from 0% (silent) to 100%
        //     (full). When using the logarithmic curve, the volume range is from -100 dB
        //     (effectively silent) to 0 dB (full). For example, a volume level of 0.5 is
        //     50% linear or -50 dB logarithmic.
        //     The linear curve is used by default.
        VolumeCurve = 7,
        //
        // Summary:
        //     Panning translation curve.
        //     logpan (bool): Panning curve... false = linear, true = logarithmic.
        //     The panning curve affects panning in exactly the same way as the volume curve
        //     (BASS_CONFIG_CURVE_VOL) affects the volume.
        //     The linear curve is used by default.
        PanCurve = 8,
        //
        // Summary:
        //     Pass 32-bit floating-point sample data to all DSP functions?
        //     floatdsp (bool): If true, 32-bit floating-point sample data is passed to
        //     all Un4seen.Bass.DSPPROC callback functions.
        //     Normally DSP functions receive sample data in whatever format the channel
        //     is using, ie. it can be 8, 16 or 32-bit. But using this config option, BASS
        //     will convert 8/16-bit sample data to 32-bit floating-point before passing
        //     it to DSP functions, and then convert it back after all the DSP functions
        //     are done. As well as simplifying the DSP code (no need for 8/16-bit processing),
        //     this also means that there is no degradation of quality as sample data passes
        //     through a chain of DSP.
        //     This config option also applies to effects set via Un4seen.Bass.Bass.BASS_ChannelSetFX(System.Int32,Un4seen.Bass.BASSFXType,System.Int32),
        //     except for DX8 effects when using the "With FX flag" DX8 effect implementation.
        //     Changing the setting while there are DSP or FX set could cause problems,
        //     so should be avoided.
        //     Platform-specific: On Android and Windows CE, 8.24 bit fixed-point is used
        //     instead of floating-point. Floating-point DX8 effect processing requires
        //     DirectX 9 (or above) on Windows.
        FloatDSP = 9,
        //
        // Summary:
        //     The 3D algorithm for software mixed 3D channels.
        //     algo (int): Use one of the Un4seen.Bass.BASS3DAlgorithm flags.
        //     These algorithms only affect 3D channels that are being mixed in software.
        //     Un4seen.Bass.Bass.BASS_ChannelGetInfo(System.Int32,Un4seen.Bass.BASS_CHANNELINFO)
        //     can be used to check whether a channel is being software mixed.
        //     Changing the algorithm only affects subsequently created or loaded samples,
        //     musics, or streams; it does not affect any that already exist.
        //     On Windows, DirectX 7 or above is required for this option to have effect.
        //     On other platforms, only the BASS_3DALG_DEFAULT and BASS_3DALG_OFF options
        //     are available.
        BASS_CONFIG_3DALGORITHM = 10,
        //
        // Summary:
        //     Time to wait for a server to respond to a connection request.
        //     timeout (int): The time to wait, in milliseconds.
        //     The default timeout is 5 seconds (5000 milliseconds).
        NetTimeOut = 11,
        //
        // Summary:
        //     The internet download buffer length.
        //     length (int): The buffer length, in milliseconds.
        //     Increasing the buffer length decreases the chance of the stream stalling,
        //     but also increases the time taken by Un4seen.Bass.Bass.BASS_StreamCreateURL(System.String,System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.DOWNLOADPROC,System.IntPtr)
        //     to create the stream, as it has to pre-buffer more data (adjustable via the
        //     Un4seen.Bass.BASSConfig.BASS_CONFIG_NET_PREBUF option). Aside from the pre-buffering,
        //     this setting has no effect on streams without either the Un4seen.Bass.BASSFlag.BASS_STREAM_BLOCK
        //     or Un4seen.Bass.BASSFlag.BASS_STREAM_RESTRATE flags.
        //     When streaming in blocks, this option determines the download buffer length.
        //     The effective buffer length can actually be a bit more than that specified,
        //     including data that has been read from the buffer by the decoder but not
        //     yet decoded.
        //     This config option also determines the buffering used by "buffered" user
        //     file streams created with Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr).
        //     The default buffer length is 5 seconds (5000 milliseconds). The net buffer
        //     length should be larger than the length of the playback buffer (Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER),
        //     otherwise the stream is likely to briefly stall soon after starting playback.
        //     Using this config option only affects streams created afterwards, not any
        //     that have already been created.
        NetBufferLength = 12,
        //
        // Summary:
        //     Prevent channels being played when the output is paused?
        //     noplay (bool): If true, channels can't be played while the output is paused.
        //     When the output is paused using Un4seen.Bass.Bass.BASS_Pause(), and this
        //     config option is enabled, channels can't be played until the output is resumed
        //     using Un4seen.Bass.Bass.BASS_Start(). Attempts to play a channel will give
        //     a Un4seen.Bass.BASSError.BASS_ERROR_START error.
        //     By default, this config option is enabled.
        PauseNoPlay = 13,
        //
        // Summary:
        //     Amount to pre-buffer when opening internet streams.
        //     prebuf (int): Amount (percentage) to pre-buffer.
        //     This setting determines what percentage of the buffer length (Un4seen.Bass.BASSConfig.BASS_CONFIG_NET_BUFFER)
        //     should be filled by Un4seen.Bass.Bass.BASS_StreamCreateURL(System.String,System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.DOWNLOADPROC,System.IntPtr).
        //     The default is 75%. Setting this lower (eg. 0) is useful if you want to display
        //     a "buffering progress" (using Un4seen.Bass.Bass.BASS_StreamGetFilePosition(System.Int32,Un4seen.Bass.BASSStreamFilePosition))
        //     when opening internet streams, but note that this setting is just a minimum
        //     - BASS will always pre-download a certain amount to verify the stream.
        //     As well as internet streams, this config setting also applies to "buffered"
        //     user file streams created with Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr).
        NetPreBuffer = 15,
        //
        // Summary:
        //     The "User-Agent" request header sent to servers.
        //     agent (string pointer): The "User-Agent" header.
        //     BASS does not make a copy of the config string, so it must reside in the
        //     heap (not the stack), eg. a global variable. This also means that the agent
        //     setting can subsequently be changed at that location without having to call
        //     this function again.
        //     Changes take effect from the next internet stream creation call.
        NetAgent = 16,
        //
        // Summary:
        //     Proxy server settings (in the form of "user:pass@server:port"... null = don't
        //     use a proxy).
        //     proxy (string pointer): The proxy server settings, in the form of "user:pass@server:port"...
        //     NULL = don't use a proxy. "" (empty string) = use the OS's default proxy
        //     settings. If only the "user:pass@" part is specified, then those authorization
        //     credentials are used with the default proxy server. If only the "server:port"
        //     part is specified, then that proxy server is used without any authorization
        //     credentials.
        //     BASS does not make a copy of the config string, so it must reside in the
        //     heap (not the stack), eg. a global variable. This also means that the proxy
        //     settings can subsequently be changed at that location without having to call
        //     this function again.
        //     Changes take effect from the next internet stream creation call.
        NetProxy = 17,
        //
        // Summary:
        //     Use passive mode in FTP connections?
        //     passive (bool): If true, passive mode is used, otherwise normal/active mode
        //     is used.
        //     Changes take effect from the next internet stream creation call. By default,
        //     passive mode is enabled.
        NetPassive = 18,
        //
        // Summary:
        //     The buffer length for recording channels.
        //     length (int): The buffer length in milliseconds... 1000 (min) - 5000 (max).
        //     If the length specified is outside this range, it is automatically capped.
        //     Unlike a playback buffer, where the aim is to keep the buffer full, a recording
        //     buffer is kept as empty as possible and so this setting has no effect on
        //     latency. The default recording buffer length is 2000 milliseconds. Unless
        //     processing of the recorded data could cause significant delays, or you want
        //     to use a large recording period with Un4seen.Bass.Bass.BASS_RecordStart(System.Int32,System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.RECORDPROC,System.IntPtr),
        //     there should be no need to increase this.
        //     Using this config option only affects the recording channels that are created
        //     afterwards, not any that have already been created. So you can have channels
        //     with differing buffer lengths by using this config option each time before
        //     creating them.
        RecordingBufferLength = 19,
        //
        // Summary:
        //     Process URLs in PLS, M3U, WPL or ASX playlists?
        //     netlists (int): When to process URLs in PLS, M3U, WPL or ASX playlists...
        //     0 = never, 1 = in Un4seen.Bass.Bass.BASS_StreamCreateURL(System.String,System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.DOWNLOADPROC,System.IntPtr)
        //     only, 2 = in Un4seen.Bass.Bass.BASS_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag)
        //     and Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr)
        //     too.
        //     When enabled, BASS will process PLS, M3U, WPL and ASX playlists, going through
        //     each entry until it finds a URL that it can play. By default, playlist procesing
        //     is disabled.
        NetPlaylist = 21,
        //
        // Summary:
        //     The maximum number of virtual channels to use in the rendering of IT files.
        //     number (int): The number of virtual channels... 1 (min) to 512 (max). If
        //     the value specified is outside this range, it is automatically capped.
        //     This setting only affects IT files, as the other MOD music formats do not
        //     have virtual channels. The default setting is 64. Changes only apply to subsequently
        //     loaded files, not any that are already loaded.
        BASS_CONFIG_MUSIC_VIRTUAL = 22,
        //
        // Summary:
        //     The amount of data to check in order to verify/detect the file format.
        //     length (int): The amount of data to check, in bytes... 1000 (min) to 100000
        //     (max). If the value specified is outside this range, it is automatically
        //     capped.
        //     Of the file formats supported as standard, this setting only affects the
        //     detection of MP3/MP2/MP1 formats, but it may also be used by add-ons (see
        //     the documentation). For internet (and "buffered" user file) streams, a quarter
        //     of the length is used, up to a minimum of 1000 bytes.
        //     The verification length excludes any tags that may be at the start of the
        //     file. The default length is 16000 bytes.
        //     For internet (and "buffered" user file) streams, the BASS_CONFIG_VERIFY_NET
        //     setting determines how much data is checked.
        FileVerificationBytes = 23,
        //
        // Summary:
        //     The number of threads to use for updating playback buffers.
        //     threads (int): The number of threads to use... 0 = disable automatic updating.
        //     The number of update threads determines how many HSTREAM/HMUSIC channel playback
        //     buffers can be updated in parallel; each thread can process one channel at
        //     a time. The default is to use a single thread, but additional threads can
        //     be used to take advantage of multiple CPU cores. There is generally nothing
        //     much to be gained by creating more threads than there are CPU cores, but
        //     one benefit of using multiple threads even with a single CPU core is that
        //     a slow updating channel need not delay the updating of other channels.
        //     When automatic updating is disabled (threads = 0), Un4seen.Bass.Bass.BASS_Update(System.Int32)
        //     or Un4seen.Bass.Bass.BASS_ChannelUpdate(System.Int32,System.Int32) should
        //     be used instead.
        //     The number of update threads can be changed at any time, including during
        //     playback.
        //     Platform-specific: The number of update threads is limited to 1 on Windows
        //     CE platforms.
        UpdateThreads = 24,
        //
        // Summary:
        //     Linux, Android and CE only: The output device buffer length.
        //     length (int): The buffer length in milliseconds.
        //     The device buffer is where the final mix of all playing channels is placed,
        //     ready for the device to play. Its length affects the latency of things like
        //     starting and stopping playback of a channel, so you will probably want to
        //     avoid setting it unnecessarily high, but setting it too short could result
        //     in breaks in the output.
        //     When using a large device buffer, the Un4seen.Bass.BASSAttribute.BASS_ATTRIB_NOBUFFER
        //     attribute could be used to skip the channel buffering stage, to avoid further
        //     increasing latency for real-time generated sound and/or DSP/FX changes.
        //     Changes to this config setting only affect subsequently initialized devices,
        //     not any that are already initialized.
        //     This config option is only available on Linux, Android and Windows CE. The
        //     device's buffer is determined automatically on other platforms.
        //     Platform-specific: On Linux, the driver may choose to use a different buffer
        //     length if it decides that the specified length is too short or long. The
        //     buffer length actually being used can be obtained with Un4seen.Bass.BASS_INFO,
        //     like this: latency + minbuf / 2.
        DeviceBufferLength = 27,
        //
        // Summary:
        //     Enable true play position mode on Windows Vista and newer?
        //     truepos (bool): If enabled, DirectSound's 'true play position' mode is enabled
        //     on Windows Vista and newer (default is true).
        //     Unless this option is enabled, the reported playback position will advance
        //     in 10ms steps on Windows Vista and newer. As well as affecting the precision
        //     of Un4seen.Bass.Bass.BASS_ChannelGetPosition(System.Int32,Un4seen.Bass.BASSMode),
        //     this also affects the timing of non-mixtime syncs. When this option is enabled,
        //     it allows finer position reporting but it also increases latency
        //     The default setting is enabled. Changes only affect channels that are created
        //     afterwards, not any that already exist. The latency and minbuf values in
        //     the Un4seen.Bass.BASS_INFO structure reflect the setting at the time of the
        //     device's Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     call.
        TruePlayPosition = 30,
        //
        // Summary:
        //     Suppress silencing for corrupted MP3 frames.
        //     errors (bool): Suppress error correction silences? (default is false).
        //     When BASS is detecting some corruption in an MP3 file's Huffman coding, it
        //     silences the frame to avoid any unpleasent noises that can result from corruption.
        //      Set this parameter to true in order to suppress this behavior and
        //     This applies only to the regular BASS version and NOT the "mp3-free" version.
        SuppressMP3ErrorCorruptionSilence = 35,
        //
        // Summary:
        //     Windows-only: Include a "Default" entry in the output device list?
        //     default (bool): If true, a 'Default' device will be included in the device
        //     list (default is false).
        //     BASS does not usually include a "Default" entry in its device list, as that
        //     would ultimately map to one of the other devices and be a duplicate entry.
        //     When the default device is requested in a Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     call (with device = -1), BASS will check the default device at that time,
        //     and initialize it. But Windows 7 has the ability to automatically switch
        //     the default output to the new default device whenever it changes, and in
        //     order for that to happen, the default device (rather than a specific device)
        //     needs to be used. That is where this option comes in.
        //     When enabled, the "Default" device will also become the default device to
        //     Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     (with device = -1). When the "Default" device is used, the Un4seen.Bass.Bass.BASS_SetVolume(System.Single)
        //     and Un4seen.Bass.Bass.BASS_GetVolume() functions work a bit differently to
        //     usual; they deal with the "session" volume, which only affects the current
        //     process's output on the device, rather than the device's volume.
        //     This option can only be set before Un4seen.Bass.Bass.BASS_GetDeviceInfo(System.Int32,Un4seen.Bass.BASS_DEVICEINFO)
        //     or Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     has been called.
        //     Platform-specific: This config option is only available on Windows. It is
        //     available on all Windows versions (not including CE), but only Windows 7
        //     has the default output switching feature.
        IncludeDefaultDevice = 36,
        //
        // Summary:
        //     The time to wait for a server to deliver more data for an internet stream.
        //     timeout (int): The time to wait in milliseconds (default=0, infinite).
        //     When the timeout is hit, the connection with the server will be closed. The
        //     default setting is 0, no timeout.
        NetReadTimeOut = 37,
        //
        // Summary:
        //     Enable speaker assignment with panning/balance control on Windows Vista and
        //     newer?
        //     enable (bool): If true, speaker assignment with panning/balance control is
        //     enabled on Windows Vista and newer.
        //     Panning/balance control via the Un4seen.Bass.BASSAttribute.BASS_ATTRIB_PAN
        //     attribute is not available when speaker assignment is used on Windows due
        //     to the way that the speaker assignment needs to be implemented there. The
        //     situation is improved with Windows Vista, and speaker assignment can generally
        //     be done in a way that does permit panning/balance control to be used at the
        //     same time, but there may still be some drivers that it does not work properly
        //     with, so it is disabled by default and can be enabled via this config option.
        //     Changes only affect channels that are created afterwards, not any that already
        //     exist.
        //     Platform-specific: This config option is only available on Windows. It is
        //     available on all Windows versions (not including CE), but only has effect
        //     on Windows Vista and newer. Speaker assignment with panning/balance control
        //     is always possible on other platforms, where BASS generates the final mix.
        EnableSpeakerAssignment = 38,
        //
        // Summary:
        //     Gets the total number of HSTREAM/HSAMPLE/HMUSIC/HRECORD handles.
        //     none: only used with Un4seen.Bass.Bass.BASS_GetConfig(Un4seen.Bass.BASSConfig).
        //     The handle count may not only include the app-created stuff but also internal
        //     stuff, eg. BASS_WASAPI_Init will create a stream when the BASS_WASAPI_BUFFER
        //     flag is used.
        HandleCount = 41,
        //
        // Summary:
        //     Gets or Sets the Unicode character set in device information.
        //     utf8 (bool): If true, device information will be in UTF-8 form. Otherwise
        //     it will be ANSI.
        //     This config option determines what character set is used in the Un4seen.Bass.BASS_DEVICEINFO
        //     structure and by the Un4seen.Bass.Bass.BASS_RecordGetInputName(System.Int32)
        //     function. The default setting is ANSI, and it can only be changed before
        //     Un4seen.Bass.Bass.BASS_GetDeviceInfo(System.Int32,Un4seen.Bass.BASS_DEVICEINFO)
        //     or Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     or Un4seen.Bass.Bass.BASS_RecordGetDeviceInfo(System.Int32,Un4seen.Bass.BASS_DEVICEINFO)
        //     or Un4seen.Bass.Bass.BASS_RecordInit(System.Int32) has been called.
        //     Platform-specific: This config option is only available on Windows.
        Unicode = 42,
        //
        // Summary:
        //     Gets or Sets the default sample rate conversion quality.
        //     quality (int): The sample rate conversion quality... 0 = linear interpolation,
        //     1 = 8 point sinc interpolation, 2 = 16 point sinc interpolation, 3 = 32 point
        //     sinc interpolation. Other values are also accepted.
        //     This config option determines what sample rate conversion quality new channels
        //     will initially have, except for sample channels (HCHANNEL), which use the
        //     BASS_CONFIG_SRC_SAMPLE setting.  A channel's sample rate conversion quality
        //     can subsequently be changed via the BASS_ATTRIB_SRC attribute (see Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single)).
        //     The default setting is 1 (8 point sinc interpolation).
        SRCQuality = 43,
        //
        // Summary:
        //     Gets or Sets the default sample rate conversion quality for samples.
        //     quality (int): The sample rate conversion quality... 0 = linear interpolation,
        //     1 = 8 point sinc interpolation, 2 = 16 point sinc interpolation, 3 = 32 point
        //     sinc interpolation. Other values are also accepted.
        //     This config option determines what sample rate conversion quality a new sample
        //     channel will initially have, following a Un4seen.Bass.Bass.BASS_SampleGetChannel(System.Int32,System.Boolean)
        //     call.  The channel's sample rate conversion quality can subsequently be changed
        //     via the BASS_ATTRIB_SRC attribute (see Un4seen.Bass.Bass.BASS_ChannelSetAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single)).
        //     The default setting is 0 (linear interpolation).
        SampleSRCQuality = 44,
        //
        // Summary:
        //     The buffer length for asynchronous file reading (default setting is 65536
        //     bytes (64KB)).
        //     length (int): The buffer length in bytes. This will be rounded up to the
        //     nearest 4096 byte (4KB) boundary.
        //     This determines the amount of file data that can be read ahead of time with
        //     asynchronous file reading. Changes only affect streams that are created afterwards,
        //     not any that already exist. So it is possible to have streams with differing
        //     buffer lengths by using this config option before creating each of them.
        //     When asynchronous file reading is enabled, the buffer level is available
        //     from Un4seen.Bass.Bass.BASS_StreamGetFilePosition(System.Int32,Un4seen.Bass.BASSStreamFilePosition).
        AsyncFileBufferLength = 45,
        //
        // Summary:
        //     Pre-scan chained OGG files?
        //     prescan (bool): If true, chained OGG files are pre-scanned.
        //     This option is enabled by default, and is equivalent to including the BASS_STREAM_PRESCAN
        //     flag in a Un4seen.Bass.Bass.BASS_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag)
        //     call when opening an OGG file. It can be disabled if seeking and an accurate
        //     length reading are not required from chained OGG files, for faster stream
        //     creation.
        OggPreScan = 47,
        //
        // Summary:
        //     Play the audio from video files using Media Foundation?
        //     video (bool): If true (default) BASS will Accept video files.
        //     This config option is only available on Windows, and only has effect on Windows
        //     Vista and newer.
        BASS_CONFIG_MF_VIDEO = 48,
        //
        // Summary:
        //     The amount of data to check in order to verify/detect the file format of
        //     internet streams.
        //     length (int): The amount of data to check, in bytes... 1000 (min) to 1000000
        //     (max), or 0 = 25% of the BASS_CONFIG_VERIFY setting (with a minimum of 1000
        //     bytes). If the value specified is outside this range, it is automatically
        //     capped.
        //     Of the file formats supported as standard, this setting only affects the
        //     detection of MP3/MP2/MP1 formats, but it may also be used by add-ons (see
        //     the documentation). The verification length excludes any tags that may be
        //     found at the start of the file. The default setting is 0, which means 25%
        //     of the BASS_CONFIG_VERIFY setting.
        //     As well as internet streams, this config setting also applies to "buffered"
        //     user file streams created with Un4seen.Bass.Bass.BASS_StreamCreateFileUser(Un4seen.Bass.BASSStreamSystem,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASS_FILEPROCS,System.IntPtr).
        NetVerificationBytes = 52,
        //
        // Summary:
        //     BASS_AC3 add-on: dynamic range compression option
        //     dynrng (bool): If true dynamic range compression is enbaled (default is false).
        BASS_CONFIG_AC3_DYNRNG = 65537,
        //
        // Summary:
        //     BASSWMA add-on: Prebuffer internet streams on creation, before returning
        //     from BASS_WMA_StreamCreateFile?
        //     prebuf (bool): The Windows Media modules must prebuffer a stream before starting
        //     decoding/playback of it. This option determines when/where to wait for that
        //     to be completed.
        //     The Windows Media modules must prebuffer a stream before starting decoding/playback
        //     of it. This option determines whether the stream creation function (eg. Un4seen.Bass.AddOn.Wma.BassWma.BASS_WMA_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag))
        //     will wait for the prebuffering to complete before returning. If playback
        //     of a stream is attempted before it has prebuffered, it will stall and then
        //     resume once it has finished prebuffering. The prebuffering progress can be
        //     monitored via Un4seen.Bass.Bass.BASS_StreamGetFilePosition(System.Int32,Un4seen.Bass.BASSStreamFilePosition)
        //     (BASS_FILEPOS_WMA_BUFFER).
        //     This option is enabled by default.
        WmaNetPreBuffer = 65793,
        //
        // Summary:
        //     BASSWMA add-on: use BASS file handling.
        //     bassfile (bool): Default is disabled (false).
        //     When enabled (true) BASSWMA uses BASS's file routines when playing local
        //     files. It uses the IStream interface to do that.  This would also allow to
        //     support the "offset" parameter for WMA files with Un4seen.Bass.Bass.BASS_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag).
        //      The downside of enabling this feature is, that it stops playback while encoding
        //     from working.
        WmaBassFileHandling = 65795,
        //
        // Summary:
        //     BASSWMA add-on: enable network seeking?
        //     seek (bool): If true seeking in network files/streams is enabled (default
        //     is false).
        //     If true, it allows seeking before the entire file has been downloaded/cached.
        //     Seeking is slow that way, so it's disabled by default.
        WmaNetSeek = 65796,
        //
        // Summary:
        //     BASSWMA add-on: play audio from WMV (video) files?
        //     playwmv (bool): If true (default) BASSWMA will play the audio from WMV video
        //     files. If false WMV files will not be played.
        WmaVideo = 65797,
        //
        // Summary:
        //     BASSWMA add-on: use a seperate thread to decode the data?
        //     async (bool): If true BASSWMA will decode the data in a seperate thread.
        //     If false (default) the normal file system will be used.
        //     The WM decoder can by synchronous (decodes data on demand) or asynchronous
        //     (decodes in the background).  With the background decoding, BASSWMA buffers
        //     the data that it receives from the decoder for the STREAMPROC to access.
        //     The start of playback/seeking may well be slightly delayed due to there being
        //     no data available immediately.  Internet streams are only supported by the
        //     asynchronous system, but local files can use either, and BASSWMA uses the
        //     synchronous system by default.
        WmaAsync = 65807,

        CDFreeOld = 66048,

        CDRetry = 66049,

        CDAutoSpeed = 66050,

        CDSkipError = 66051,
        //
        // Summary:
        //     BASSCD add-on: The server to use in CDDB requests.
        //     server (string): The CDDB server address, in the form of "user:pass@server:port/path".
        //     The "user:pass@", ":port" and "/path" parts are optional; only the "server"
        //     part is required. If not provided, the port and path default to 80 and "/~cddb/cddb.cgi",
        //     respectively.
        //     A copy is made of the provided server string, so it need not persist beyond
        //     the Un4seen.Bass.Bass.BASS_SetConfigPtr(Un4seen.Bass.BASSConfig,System.IntPtr)
        //     call. The default setting is "freedb.freedb.org". .
        //     The proxy server, as configured via the BASS_CONFIG_NET_PROXY option, is
        //     used when connecting to the CDDB server.
        BASS_CONFIG_CD_CDDB_SERVER = 66052,
        //
        // Summary:
        //     BASSenc add-on: Encoder DSP priority (default -1000)
        //     priority (int): The priorty determines where in the DSP chain the encoding
        //     is performed - all DSP with a higher priority will be present in the encoding.
        //     Changes only affect subsequent encodings, not those that have already been
        //     started. The default priority is -1000.
        BASS_CONFIG_ENCODE_PRIORITY = 66304,
        //
        // Summary:
        //     BASSenc add-on: The maximum queue length (default 10000, 0=no limit)
        //     limit (int): The async encoder queue size limit in milliseconds; 0=unlimited.
        //     When queued encoding is enabled, the queue's buffer will grow as needed to
        //     hold the queued data, up to a limit specified by this config option.  The
        //     default limit is 10 seconds (10000 milliseconds). Changes only apply to new
        //     encoders, not any already existing encoders.
        BASS_CONFIG_ENCODE_QUEUE = 66305,
        //
        // Summary:
        //     BASSenc add-on: ACM codec name to give priority for the formats it supports.
        //     codec (string pointer): The ACM codec name to give priority (e.g. 'l3codecp.acm').
        //     BASSenc does make a copy of the config string, so it can be freed right after
        //     calling it.
        BASS_CONFIG_ENCODE_ACM_LOAD = 66306,
        //
        // Summary:
        //     BASSenc add-on: The time to wait to send data to a cast server (default 5000ms)
        //     timeout (int): The time to wait, in milliseconds.
        //     When an attempt to send data is timed-out, the data is discarded. Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_SetNotify(System.Int32,Un4seen.Bass.AddOn.Enc.ENCODENOTIFYPROC,System.IntPtr)
        //     can be used to receive a notification of when this happens.
        //     The default timeout is 5 seconds (5000 milliseconds). Changes take immediate
        //     effect.
        BASS_CONFIG_ENCODE_CAST_TIMEOUT = 66320,
        //
        // Summary:
        //     BASSenc add-on: Proxy server settings when connecting to Icecast and Shoutcast
        //     (in the form of "[user:pass@]server:port"... null = don't use a proxy but
        //     a direct connection).
        //     proxy (string pointer): The proxy server settings, in the form of "[user:pass@]server:port"...
        //     null = don't use a proxy but make a direct connection (default). If only
        //     the "server:port" part is specified, then that proxy server is used without
        //     any authorization credentials.
        //     BASSenc does not make a copy of the config string, so it must reside in the
        //     heap (not the stack), eg. a global variable. This also means that the proxy
        //     settings can subsequently be changed at that location without having to call
        //     this function again.
        //     This setting affects how the following functions connect to servers: Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastInit(System.Int32,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.Int32,System.Boolean),
        //     Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastGetStats(System.Int32,Un4seen.Bass.AddOn.Enc.BASSEncodeStats,System.String),
        //     Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastSetTitle(System.Int32,System.String,System.String).
        //      When a proxy server is used, it needs to support the HTTP 'CONNECT' method.
        //     The default setting is NULL (do not use a proxy).
        //     Changes take effect from the next internet stream creation call. By default,
        //     BASSenc will not use any proxy settings when connecting to Icecast and Shoutcast.
        BASS_CONFIG_ENCODE_CAST_PROXY = 66321,
        //
        // Summary:
        //     BASSMIDI add-on: Automatically compact all soundfonts following a configuration
        //     change?
        //     compact (bool): If true, all soundfonts are compacted following a MIDI stream
        //     being freed, or a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamSetFonts(System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_FONT[],System.Int32)
        //     call.
        //     The compacting isn't performed immediately upon a MIDI stream being freed
        //     or Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamSetFonts(System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_FONT[],System.Int32)
        //     being called. It's actually done 2 seconds later (in another thread), so
        //     that if another MIDI stream starts using the soundfonts in the meantime,
        //     they aren't needlessly closed and reopened.
        //     Samples that have been preloaded by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_FontLoad(System.Int32,System.Int32,System.Int32)
        //     are not affected by automatic compacting. Other samples that have been preloaded
        //     by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamLoadSamples(System.Int32)
        //     are affected though, so it is probably wise to disable this option when using
        //     that function.
        //     By default, this option is enabled.
        BASS_CONFIG_MIDI_COMPACT = 66560,
        //
        // Summary:
        //     BASSMIDI add-on: The maximum number of samples to play at a time (polyphony).
        //     voices (int): Maximum number of samples to play at a time... 1 (min) - 1000
        //     (max).
        //     This setting determines the maximum number of samples that can play together
        //     in a single MIDI stream. This isn't necessarily the same thing as the maximum
        //     number of notes, due to presets often layering multiple samples. When there
        //     are no voices available to play a new sample, the voice with the lowest volume
        //     will be killed to make way for it.
        //     The more voices that are used, the more CPU that is required. So this option
        //     can be used to restrict that, for example on a less powerful system. The
        //     CPU usage of a MIDI stream can also be restricted via the Un4seen.Bass.BASSAttribute.BASS_ATTRIB_MIDI_CPU
        //     attribute.
        //     Changing this setting only affects subsequently created MIDI streams, not
        //     any that have already been created. The default setting is 128 voices.
        //     Platform-specific
        //     The default setting is 100, except on iOS, where it is 40.
        BASS_CONFIG_MIDI_VOICES = 66561,
        //
        // Summary:
        //     BASSMIDI add-on: Automatically load matching soundfonts?
        //     autofont (bool): If true, BASSMIDI will try to load a soundfont matching
        //     the MIDI file.
        //     This option only applies to local MIDI files, loaded using Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag,System.Int32)
        //     (or Un4seen.Bass.Bass.BASS_StreamCreateFile(System.String,System.Int64,System.Int64,Un4seen.Bass.BASSFlag)
        //     via the plugin system). BASSMIDI won't look for matching soundfonts for MIDI
        //     files loaded from the internet.
        //     By default, this option is enabled.
        BASS_CONFIG_MIDI_AUTOFONT = 66562,
        //
        // Summary:
        //     BASSMIDI add-on: Default soundfont usage
        //     filename (string): Filename of the default soundfont to use (null = no default
        //     soundfont).
        //     When setting the default soundfont, a copy is made of the filename, so it
        //     does not need to persist beyond the Un4seen.Bass.Bass.BASS_SetConfigPtr(Un4seen.Bass.BASSConfig,System.IntPtr)
        //     call. If the specified soundfont cannot be loaded, the default soundfont
        //     setting will remain as it is. Un4seen.Bass.Bass.BASS_GetConfigPtr(Un4seen.Bass.BASSConfig)
        //     can be used to confirm what that is.
        //     On Windows, the default is to use one of the Creative soundfonts (28MBGM.SF2
        //     or CT8MGM.SF2 or CT4MGM.SF2 or CT2MGM.SF2), if present in the windows system
        //     directory.
        BASS_CONFIG_MIDI_DEFFONT = 66563,
        //
        // Summary:
        //     BASSMIDI add-on: The number of MIDI input ports to make available
        //     ports (int): Number of input ports... 0 (min) - 10 (max).
        //     MIDI input ports allow MIDI data to be received from other software, not
        //     only MIDI devices. Once a port has been initialized via Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_InInit(System.Int32,Un4seen.Bass.AddOn.Midi.MIDIINPROC,System.IntPtr),
        //     the ALSA client and port IDs can be retrieved from Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_InGetDeviceInfo(System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_DEVICEINFO),
        //     which other software can use to connect to the port and send data to it.
        //     Prior to initialization, an input port will have a client ID of 0.
        //     The default is for 1 input port to be available. Note: This option is only
        //     available on Linux.
        BASS_CONFIG_MIDI_IN_PORTS = 66564,
        //
        // Summary:
        //     BASSmix add-on: The order of filter used to reduce aliasing (only available/used
        //     pre BASSmix 2.4.7, where BASS_CONFIG_SRC is used).
        //     order (int): The filter order... 2 (min) to 50 (max), and even. If the value
        //     specified is outside this range, it is automatically capped.
        //     The filter order determines how abruptly the level drops at the cutoff frequency,
        //     or the roll-off. The levels rolls off at 6 dB per octave for each order.
        //     For example, a 4th order filter will roll-off at 24 dB per octave. A low
        //     order filter may result in some aliasing persisting, and sounds close to
        //     the cutoff frequency being attenuated. Higher orders reduce those things,
        //     but require more processing.
        //     By default, a 4th order filter is used. Changes only affect channels that
        //     are subsequently plugged into a mixer, not those that are already plugged
        //     in.
        BASS_CONFIG_MIXER_FILTER = 67072,
        //
        // Summary:
        //     BASSmix add-on: The source channel buffer size multiplier.
        //     multiple (int): The buffer size multiplier... 1 (min) to 5 (max). If the
        //     value specified is outside this range, it is automatically capped.
        //     When a source channel has buffering enabled, the mixer will buffer the decoded
        //     data, so that it is available to the Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetData(System.Int32,System.IntPtr,System.Int32)
        //     and Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetLevel(System.Int32)
        //     functions. To reach the source channel's buffer size, the multiplier (multiple)
        //     is applied to the BASS_CONFIG_BUFFER setting at the time of the mixer's creation.
        //     If the source is played at it's default rate, then the buffer only need to
        //     be as big as the mixer's buffer. But if it's played at a faster rate, then
        //     the buffer needs to be bigger for it to contain the data that is currently
        //     being heard from the mixer. For example, playing a channel at 2x its normal
        //     speed would require the buffer to be 2x the normal size (multiple = 2).
        //     Larger buffers obviously require more memory, so the multiplier should not
        //     be set higher than necessary.
        //     The default multiplier is 2x. Changes only affect subsequently setup channel
        //     buffers. An existing channel can have its buffer reinitilized by disabling
        //     and then re-enabling the BASS_MIXER_BUFFER flag using Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelFlags(System.Int32,Un4seen.Bass.BASSFlag,Un4seen.Bass.BASSFlag).
        BASS_CONFIG_MIXER_BUFFER = 67073,
        //
        // Summary:
        //     BASSmix add-on: How far back to keep record of source positions to make available
        //     for Un4seen.Bass.AddOn.Mix.BassMix.BASS_Mixer_ChannelGetPositionEx(System.Int32,Un4seen.Bass.BASSMode,System.Int32).
        //     length (int): The length of time to back, in milliseconds.
        //     If a mixer is not a decoding channel (not using the Un4seen.Bass.BASSFlag.BASS_STREAM_DECODE
        //     flag), this config setting will just be a minimum and the mixer will always
        //     have a position record at least equal to its playback buffer length, as determined
        //     by the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER config option.
        //     The default setting is 2000ms. Changes only affect newly created mixers,
        //     not any that already exist.
        BASS_CONFIG_MIXER_POSEX = 67074,

        SplitBufferLength = 67088,
        //
        // Summary:
        //     BASSaac add-on: play audio from mp4 (video) files?
        //     playmp4 (bool): If true (default) BASSaac will play the audio from mp4 video
        //     files. If false mp4 video files will not be played.
        BASS_CONFIG_MP4_VIDEO = 67328,
        //
        // Summary:
        //     BASSaac add-on: Support MP4 in BASS_AAC_StreamCreateXXX functions?
        //     usemp4 (bool): If true BASSaac supports MP4 in the BASS_AAC_StreamCreateXXX
        //     functions. If false (default) only AAC is supported.
        BASS_CONFIG_AAC_MP4 = 67329,
    }

    public enum TagType
    {
        // Summary:
        //     Unknown tags : not supported tags
        Unknown = -1,
        //
        // Summary:
        //     ID3v1 tags : A pointer to a 128 byte block is returned (see Un4seen.Bass.BASSTag.BASS_TAG_ID3).
        //     See www.id3.org for details of the block's structure.
        ID3 = 0,
        //
        // Summary:
        //     ID3v2 tags : A pointer to a variable length block is returned.
        //     See www.id3.org for details of the block's structure.
        ID3v2 = 1,
        //
        // Summary:
        //     OGG comments : Only available when streaming an OGG file. A pointer to a
        //     series of null-terminated UTF-8 strings is returned, the final string ending
        //     with a double null.
        OGG = 2,
        //
        // Summary:
        //     HTTP headers : Only available when streaming from a HTTP server. A pointer
        //     to a series of null-terminated ANSI strings is returned, the final string
        //     ending with a double null.
        HTTP = 3,
        //
        // Summary:
        //     ICY headers : A pointer to a series of null-terminated ANSI strings is returned,
        //     the final string ending with a double null.
        ICY = 4,
        //
        // Summary:
        //     ICY (Shoutcast) metadata : A single null-terminated ANSI string containing
        //     the current stream title and url (usually omitted). The format of the string
        //     is: StreamTitle='xxx';StreamUrl='xxx';
        META = 5,
        //
        // Summary:
        //     APE (v1 or v2) tags : Only available when streaming an APE file. A pointer
        //     to a series of null-terminated UTF-8 strings is returned, the final string
        //     ending with a double null.
        APE = 6,
        //
        // Summary:
        //     iTunes/MP4 metadata : Only available when streaming a MP4 file. A pointer
        //     to a series of null-terminated UTF-8 strings is returned, the final string
        //     ending with a double null.
        MP4 = 7,
        //
        // Summary:
        //     WMA header tags: WMA tags : Only available when streaming a WMA file. A pointer
        //     to a series of null-terminated UTF-8 strings is returned, the final string
        //     ending with a double null.
        WMA = 8,
        //
        // Summary:
        //     OGG encoder : A single null-terminated UTF-8 string.
        Vendor = 9,
        //
        // Summary:
        //     Lyric3v2 tag : A single ANSI string is returned, containing the Lyrics3v2
        //     information. See www.id3.org/Lyrics3v2 for details of its format.
        Lyrics3v2 = 10,
        //
        // Summary:
        //     WMA mid-stream tag: a single UTF-8 string.
        WmaMeta = 11,
        //
        // Summary:
        //     Apple CoreAudio codec info : Un4seen.Bass.BASS_TAG_CACODEC structure.
        CoreAudioCodec = 11,
        //
        // Summary:
        //     WMA codec: A description of the codec used by the file. 2 null-terminated
        //     UTF-8 strings are returned, with the 1st string being the name of the codec,
        //     and the 2nd containing additional information like what VBR setting was used.
        WmaCodec = 12,
        //
        // Summary:
        //     FLAC cuesheet : Un4seen.Bass.BASSTag.BASS_TAG_FLAC_CUE structure (which includes
        //     Un4seen.Bass.BASS_TAG_FLAC_CUE_TRACK and Un4seen.Bass.BASS_TAG_FLAC_CUE_TRACK_INDEX).
        FlacCue = 12,
        //
        // Summary:
        //     Media Foundation tags : A pointer to a series of null-terminated UTF-8 strings
        //     is returned, the final string ending with a double null.
        MF = 13,
        //
        // Summary:
        //     WAVE format : A pointer to a Un4seen.Bass.WAVEFORMATEXT structure is returned.
        WaveFormat = 14,
        //
        // Summary:
        //     RIFF/WAVE tags : array of null-terminated ANSI strings.
        RIFFInfo = 256,
        //
        // Summary:
        //     BWF/RF64 tags (Broadcast Audio Extension) : A pointer to a variable length
        //     block is returned (see Un4seen.Bass.BASS_TAG_BEXT).
        //     See the EBU specification for details of the block's structure.
        //     When reading BWF tags into a Un4seen.Bass.AddOn.Tags.TAG_INFO structure e.g.
        //     via Un4seen.Bass.AddOn.Tags.BassTags.BASS_TAG_GetFromFile(System.String)
        //     the following mapping is performed if no RIFF_INFO tags are present:
        //     Description = Title (max. 256 chars)
        //     Originator = Artist (max. 32 chars)
        //     OriginatorReference = EncodedBy (max. 32 chars)
        //     OriginationDate = Year (in format 'yyyy-mm-dd hh:mm:ss')
        //     TimeReference = Track
        //     UMID = Copyright (max. 64 chars)
        //     CodingHistory = Comment
        //     However, if RIFF_INFO tags are present the BWF tags are present in the NativeTags.
        BASS_TAG_RIFF_BEXT = 257,
        //
        // Summary:
        //     RIFF/BWF Radio Traffic Extension tags : A pointer to a variable length block
        //     is returned (see Un4seen.Bass.BASS_TAG_CART).
        //     See the EBU specifications for details of the block's structure.
        //     When reading BWF tags into a Un4seen.Bass.AddOn.Tags.TAG_INFO structure e.g.
        //     via Un4seen.Bass.AddOn.Tags.BassTags.BASS_TAG_GetFromFile(System.String)
        //     the following mapping is performed if no RIFF_INFO tags are present:
        //     Title = Title (max. 64 chars)
        //     Artist = Artist (max. 64 chars)
        //     Category = Grouping (max. 64 chars)
        //     Classification = Mood (max. 64 chars)
        //     ProducerAppID = Publisher (max. 64 chars)
        //     ProducerAppVersion = EncodedBy (max. 64 chars)
        //     TagText = Comment
        //     However, if RIFF_INFO tags are present the CART tags are present in the NativeTags.
        BASS_TAG_RIFF_CART = 258,
        //
        // Summary:
        //     RIFF DISP text chunk: a single ANSI string.
        BASS_TAG_RIFF_DISP = 259,
        //
        // Summary:
        //     + index# : Un4seen.Bass.BASSTag.BASS_TAG_APE_BINARY structure.
        BASS_TAG_APE_BINARY = 4096,
        //
        // Summary:
        //     MOD music name : a single ANSI string.
        MusicName = 65536,
        //
        // Summary:
        //     MOD message : a single ANSI string.
        MusicMessage = 65537,
        //
        // Summary:
        //     MOD music order list: BYTE array of pattern numbers played at that order
        //     position.
        //     Pattern number 254 is "+++" (skip order) and 255 is "---" (end song). You
        //     can use Un4seen.Bass.Bass.BASS_ChannelGetLength(System.Int32,Un4seen.Bass.BASSMode)
        //     with BASS_POS_MUSIC_ORDER to get the length of the array.
        MusicOrders = 65538,
        //
        // Summary:
        //     + instrument#, MOD instrument name : ANSI string
        MusicInstrument = 65792,
        //
        // Summary:
        //     + sample#, MOD sample name : ANSI string
        MusicSample = 66304,
        //
        // Summary:
        //     + track#, track text : array of null-terminated ANSI strings
        BASS_TAG_MIDI_TRACK = 69632,
        //
        // Summary:
        //     + index# : Un4seen.Bass.BASSTag.BASS_TAG_FLAC_PICTURE structure.
        BASS_TAG_FLAC_PICTURE = 73728,
        //
        // Summary:
        //     ADX tags: A pointer to the ADX loop structure (see Un4seen.Bass.AddOn.Adx.BASS_ADX_TAG_LOOP).
        BASS_TAG_ADX_LOOP = 73728
    }
}