namespace ManagedBass.Dynamics
{
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
        Algorithm3D = 10,
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
        MusicVirtual = 22,
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
        MFVideo = 48,
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
        AC3DynamicRangeCompression = 65537,
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
        CDDBServer = 66052,
        //
        // Summary:
        //     BASSenc add-on: Encoder DSP priority (default -1000)
        //     priority (int): The priorty determines where in the DSP chain the encoding
        //     is performed - all DSP with a higher priority will be present in the encoding.
        //     Changes only affect subsequent encodings, not those that have already been
        //     started. The default priority is -1000.
        EncodePriority = 66304,
        //
        // Summary:
        //     BASSenc add-on: The maximum queue length (default 10000, 0=no limit)
        //     limit (int): The async encoder queue size limit in milliseconds; 0=unlimited.
        //     When queued encoding is enabled, the queue's buffer will grow as needed to
        //     hold the queued data, up to a limit specified by this config option.  The
        //     default limit is 10 seconds (10000 milliseconds). Changes only apply to new
        //     encoders, not any already existing encoders.
        EncodeQueue = 66305,
        //
        // Summary:
        //     BASSenc add-on: ACM codec name to give priority for the formats it supports.
        //     codec (string pointer): The ACM codec name to give priority (e.g. 'l3codecp.acm').
        //     BASSenc does make a copy of the config string, so it can be freed right after
        //     calling it.
        EncodeACMLoad = 66306,
        //
        // Summary:
        //     BASSenc add-on: The time to wait to send data to a cast server (default 5000ms)
        //     timeout (int): The time to wait, in milliseconds.
        //     When an attempt to send data is timed-out, the data is discarded. Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_SetNotify(System.Int32,Un4seen.Bass.AddOn.Enc.ENCODENOTIFYPROC,System.IntPtr)
        //     can be used to receive a notification of when this happens.
        //     The default timeout is 5 seconds (5000 milliseconds). Changes take immediate
        //     effect.
        EncodeCastTimeout = 66320,
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
        EncodeCastProxy = 66321,
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
        MidiCompact = 66560,
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
        MidiVoices = 66561,
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
        MidiAutoFont = 66562,
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
        MidiDefaultFont = 66563,
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
        MidiInputPorts = 66564,
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
        MixerFilter = 67072,
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
        MixerBuffer = 67073,
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
        MixerPositionEx = 67074,

        SplitBufferLength = 67088,
        //
        // Summary:
        //     BASSaac add-on: play audio from mp4 (video) files?
        //     playmp4 (bool): If true (default) BASSaac will play the audio from mp4 video
        //     files. If false mp4 video files will not be played.
        Mp4Video = 67328,
        //
        // Summary:
        //     BASSaac add-on: Support MP4 in BASS_AAC_StreamCreateXXX functions?
        //     usemp4 (bool): If true BASSaac supports MP4 in the BASS_AAC_StreamCreateXXX
        //     functions. If false (default) only AAC is supported.
        AacSupportMp4 = 67329,
    }
}