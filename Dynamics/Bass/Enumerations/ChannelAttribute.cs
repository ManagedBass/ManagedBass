namespace ManagedBass.Dynamics
{
    public enum ChannelAttribute
    {
        /// <summary>
        /// The sample rate of a channel.
        /// freq: The sample rate... 0 = original rate (when the channel was created).
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels. 
        /// It is still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix add-on,
        /// and anything else that wants to use it.
        /// It is not possible to change the sample rate of a channel if the "with FX
        /// flag" DX8 effect implementation enabled on it, unless DirectX 9 or above is installed.
        /// Increasing the sample rate of a stream or MOD music increases its CPU usage,
        /// and reduces the length of its playback buffer in terms of time. 
        /// If you intend to raise the sample rate above the original rate, then you may also need
        /// to increase the buffer length via the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        /// config option to avoid break-ups in the sound.
        /// 
        /// Platform-specific
        /// On Windows, the sample rate will get rounded down to a whole number during playback.
        /// </summary>
        Frequency = 1,

        /// <summary>
        /// The volume level of a channel.
        /// volume: The volume level... 0 (silent) to 1 (full). This can go above 1.0 on decoding channels.
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels. 
        /// It is still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix add-on, and anything else that wants to use it.
        /// When using Un4seen.Bass.Bass.BASS_ChannelSlideAttribute(System.Int32,Un4seen.Bass.BASSAttribute,System.Single,System.Int32)
        /// to slide this attribute, a negative volume value can be used to fade-out and then stop the channel.
        /// </summary>
        Volume = 2,

        /// <summary>
        /// The panning/balance position of a channel.
        /// pan: The pan position... -1 (full left) to +1 (full right), 0 = centre.
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels. 
        /// It is still adjustable though, so that it can be used by the Un4seen.Bass.AddOn.Mix add-on,
        /// and anything else that wants to use it.
        /// It is not possible to set the pan position of a 3D channel. 
        /// It is also not possible to set the pan position when using speaker assignment, but if needed,
        /// it can be done via a Un4seen.Bass.DSPPROC instead (not on mono channels).
        /// 
        /// Platform-specific
        /// On Windows, this attribute has no effect when speaker assignment is used,
        /// except on Windows Vista and newer with the Un4seen.Bass.BASSConfig.BASS_CONFIG_VISTA_SPEAKERS
        /// config option enabled. Balance control could be implemented via a Un4seen.Bass.DSPPROC instead
        /// </summary>
        Pan = 3,

        /// <summary>
        /// The wet (reverb) / dry (no reverb) mix ratio on a sample, stream, or MOD music channel with 3D functionality.
        /// mix: The wet / dry ratio... 0 (full dry) to 1 (full wet), -1 = automatically
        /// calculate the mix based on the distance (the default).
        /// Obviously, EAX functions have no effect if the output device does not support EAX.
        /// Un4seen.Bass.Bass.BASS_GetInfo(Un4seen.Bass.BASS_INFO) can be used to check that.
        /// EAX only affects 3D channels, but EAX functions do not require Un4seen.Bass.Bass.BASS_Apply3D() to apply the changes.
        /// Un4seen.Bass.BASSErrorDescription BASS_ERROR_NOEAXThe channel does not have EAX support.
        /// EAX only applies to 3D channels that are mixed by the hardware/drivers.
        /// Un4seen.Bass.Bass.BASS_ChannelGetInfo(System.Int32,Un4seen.Bass.BASS_CHANNELINFO)
        /// can be used to check if a channel is being mixed by the hardware.
        /// EAX is only supported on Windows.
        /// </summary>
        EaxMix = 4,
        
        /// <summary>
        /// Non-Windows: Disable playback buffering?
        /// nobuffer: Disable playback buffering... 0 = no, else yes..
        /// A playing channel is normally asked to render data to its playback buffer in advance, 
        /// via automatic buffer updates or the Bass.Update() and Bass.ChannelUpdate() functions,
        /// ready for mixing with other channels to produce the final mix that is given to the output device.
        /// When this attribute is switched on (the default is off), that buffering is skipped and 
        /// the channel will only be asked to produce data as it is needed during the generation of the final mix. 
        /// This allows the lowest latency to be achieved, but also imposes tighter timing requirements
        /// on the channel to produce its data and apply any DSP/FX (and run mixtime syncs) that are set on it; 
        /// if too long is taken, there will be a break in the output, affecting all channels that are playing on the same device.
        /// The channel's data is still placed in its playback buffer when this attribute is on,
        /// which allows Bass.ChannelGetData() and Bass.ChannelGetLevel() to be used, although there is 
        /// likely to be less data available to them due to the buffer being less full.
        /// This attribute can be changed mid-playback. 
        /// If switched on, any already buffered data will still be played, so that there is no break in sound.
        /// This attribute is not available on Windows, as BASS does not generate the final mix.
        /// </summary>
        NoBuffer = 5,

        /// <summary>
        /// The CPU usage of a channel.
        /// cpu: The CPU usage (in percentage).
        /// This attribute gives the percentage of CPU that the channel is using, 
        /// including the time taken by decoding and DSP processing, and any FX that are 
        /// not using the "with FX flag" DX8 effect implementation. 
        /// It does not include the time taken to add the channel's data to the final output mix during playback.
        /// The processing of some add-on stream formats may also not be entirely included,
        /// if they use additional decoding threads; see the add-on documentation for details.
        /// Like Bass.GetCPU(), this function does not strictly tell the CPU usage, but rather how timely the processing is.
        /// For example, if it takes 10ms to generate 100ms of data, that would be 10%.
        /// If the reported usage exceeds 100%, that means the channel's data is taking longer to generate than to play. 
        /// The duration of the data is based on the channel's current sample rate (BASSAttribute.Frequency).
        /// A channel's CPU usage is updated whenever it generates data. 
        /// That could be during a playback buffer update cycle, or a Bass.Update() call, or a Bass.ChannelUpdate() call.
        /// For a decoding channel, it would be in a Bass.ChannelGetData() or Bass.ChannelGetLevel() call.
        /// This attribute is read-only, so cannot be modified via Bass.ChannelSetAttribute().
        /// </summary>
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

        /// <summary>
        /// The scanned info of a channel.
        /// </summary>
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

        /// <summary>
        /// BASS_FX Tempo: The Tempo in percents (-95%..0..+5000%).
        /// </summary>
        Tempo = 65536,

        /// <summary>
        /// BASS_FX Tempo: The Pitch in semitones (-60..0..+60).
        /// </summary>
        Pitch = 65537,

        /// <summary>
        /// BASS_FX Tempo: The Samplerate in Hz, but calculates by the same % as BASS_ATTRIB_TEMPO.
        /// </summary>
        TempoFrequency = 65538,

        /// <summary>
        /// BASS_FX Tempo Option: Use FIR low-pass (anti-alias) filter (gain speed, lose quality)? true=1 (default), false=0.
        /// See BassFx.TempoCreate() for details.
        /// On iOS, Android, WinCE and Linux ARM platforms this is by default disabled for lower CPU usage.
        /// </summary>
        TempoUseAAFilter = 65552,

        /// <summary>
        /// BASS_FX Tempo Option: FIR low-pass (anti-alias) filter length in taps (8 .. 128 taps, default = 32, should be %4).
        /// See BassFx.TempoCreate() for details.
        /// </summary>
        TempoAAFilterLength = 65553,

        /// <summary>
        /// BASS_FX Tempo Option: Use quicker tempo change algorithm (gain speed, lose quality)? true=1, false=0 (default).
        /// See BassFx.TempoCreate() for details.
        /// </summary>
        TempoUseQuickAlgorithm = 65554,

        /// <summary>
        /// BASS_FX Tempo Option: Tempo Sequence in milliseconds (default = 82).
        /// See BassFx.TempoCreate() for details.
        /// </summary>
        TempoSequenceMilliseconds = 65555,

        /// <summary>
        /// BASS_FX Tempo Option: SeekWindow in milliseconds (default = 14).
        /// See BassFx.TempoCreate() for details.
        /// </summary>
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

        /// <summary>
        /// Playback direction (-1 = Reverse or 1 = Forward).
        /// </summary>
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
}