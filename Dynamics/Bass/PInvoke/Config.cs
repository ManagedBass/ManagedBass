using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        static IntPtr _netAgent, _netProxy;

        [DllImport(DllName, EntryPoint = "BASS_SetConfig")]
        internal extern static bool Configure(Configuration option, bool newvalue);

        [DllImport(DllName, EntryPoint = "BASS_SetConfig")]
        internal extern static bool Configure(Configuration option, int newvalue);

        [DllImport(DllName, EntryPoint = "BASS_SetConfigPtr")]
        internal extern static bool Configure(Configuration option, IntPtr newvalue);

        [DllImport(DllName, EntryPoint = "BASS_GetConfig")]
        internal extern static int GetConfig(Configuration option);

        [DllImport(DllName, EntryPoint = "BASS_GetConfigPtr")]
        internal extern static IntPtr GetConfigPtr(Configuration option);

        internal static bool GetConfigBool(Configuration option) { return GetConfig(option) == 1; }

        /// <summary>
        /// The buffer length in milliseconds. The minimum length is 1ms
        /// above the update period (See <see cref="UpdatePeriod"/>),
        /// the maximum is 5000 milliseconds. If the length specified is outside this
        /// range, it is automatically capped.
        /// The default buffer length is 500 milliseconds. Increasing the length, decreases
        /// the chance of the sound possibly breaking-up on slower computers, but also
        /// increases the latency for DSP/FX.
        /// Small buffer lengths are only required if the sound is going to be changing
        /// in real-time, for example, in a soft-synth. If you need to use a small buffer,
        /// then the minbuf member of BASS_INFO should be used to get the recommended
        /// minimum buffer length supported by the device and it's drivers. Even at this
        /// default length, it's still possible that the sound could break up on some
        /// systems, it's also possible that smaller buffers may be fine. So when using
        /// small buffers, you should have an option in your software for the user to
        /// finetune the length used, for optimal performance.
        /// Using this config option only affects the HMUSIC/HSTREAM channels that you
        /// create afterwards, not the ones that have already been created. So you can
        /// have channels with differing buffer lengths by using this config option each
        /// time before creating them.
        /// If automatic updating is disabled, make sure you call Bass.Update()
        /// frequently enough to keep the buffers updated.
        /// </summary>
        public static int PlaybackBufferLength
        {
            get { return GetConfig(Configuration.PlaybackBufferLength); }
            set { Configure(Configuration.PlaybackBufferLength, value); }
        }

        /// <summary>
        /// The update period of HSTREAM and HMUSIC channel playback buffers in milliseconds.
        /// 0 = disable automatic updating. The minimum period is 5ms, the maximum is 100ms. If the period
        /// specified is outside this range, it is automatically capped.
        /// The update period is the amount of time between updates of the playback buffers
        /// of HSTREAM/HMUSIC channels. Shorter update periods allow smaller buffers
        /// to be set with the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER option, but
        /// as the rate of updates increases, so the overhead of setting up the updates
        /// becomes a greater part of the CPU usage. The update period only affects HSTREAM
        /// and HMUSIC channels, it does not affect samples. Nor does it have any effect
        /// on decoding channels, as they are not played.
        /// BASS creates one or more threads (determined by Un4seen.Bass.BASSConfig.BASS_CONFIG_UPDATETHREADS)
        /// specifically to perform the updating, except when automatic updating is disabled
        /// (period=0) - then you must regularly call Bass.Update()
        /// or Bass.ChannelUpdate() instead.
        /// This allows you to synchronize BASS's CPU usage with your program's. For
        /// example, in a game loop you could call Un4seen.Bass.Bass.BASS_Update(System.Int32)
        /// once per frame, which keeps all the processing in sync so that the frame
        /// rate is as smooth as possible. Bass.Update() should be called at least around
        /// 8 times per second, even more often if the Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER
        /// option is used to set smaller buffers.
        /// The update period can be altered at any time, including during playback.
        /// The default period is 100ms.
        /// </summary>
        public static int UpdatePeriod
        {
            get { return GetConfig(Configuration.UpdatePeriod); }
            set { Configure(Configuration.UpdatePeriod, value); }
        }

        /// <summary>
        /// Global sample volume.
        /// volume (int): Sample global volume level... 0 (silent) - 10000 (full).
        /// This config option allows you to have control over the volume levels of all the samples, 
        /// which is useful for setup options (eg. separate music and fx volume controls).
        /// A channel's final volume = channel volume * global volume / max volume. 
        /// So, for example, if a stream channel's volume is 0.5 and the global stream volume is 8000, 
        /// then effectively the stream's volume level is 0.4 (0.5 * 8000 / 10000 = 0.4).
        /// </summary>
        public static int GlobalSampleVolume
        {
            get { return GetConfig(Configuration.GlobalSampleVolume); }
            set { Configure(Configuration.GlobalSampleVolume, value); }
        }

        /// <summary>
        /// Global stream volume.
        /// volume (int): Stream global volume level... 0 (silent) - 10000 (full).
        /// This config option allows you to have control over the volume levels of all streams, 
        /// which is useful for setup options (eg. separate music and fx volume controls).
        /// A channel's final volume = channel volume * global volume / max volume. 
        /// So, for example, if a stream channel's volume is 0.5 and the global stream volume is 8000, 
        /// then effectively the stream's volume level is 0.4 (0.5 * 8000 / 10000 = 0.4).
        /// </summary>
        public static int GlobalStreamVolume
        {
            get { return GetConfig(Configuration.GlobalStreamVolume); }
            set { Configure(Configuration.GlobalStreamVolume, value); }
        }

        /// <summary>
        /// Global music volume.
        /// volume (int): MOD music global volume level... 0 (silent) - 10000 (full).
        /// This config option allows you to have control over the volume levels of all the MOD musics, 
        /// which is useful for setup options (eg. separate music and fx volume controls).
        /// A channel's final volume = channel volume * global volume / max volume.
        /// So, for example, if a stream channel's volume is 0.5 and the global stream volume is 8000, 
        /// then effectively the stream's volume level is 0.4 (0.5 * 8000 / 10000 = 0.4).
        /// </summary>
        public static int GlobalMusicVolume
        {
            get { return GetConfig(Configuration.GlobalMusicVolume); }
            set { Configure(Configuration.GlobalMusicVolume, value); }
        }

        /// <summary>
        /// Volume translation curve.
        /// logvol (bool): Volume curve... false = linear, true = logarithmic.
        /// DirectSound uses logarithmic volume and panning curves, which can be awkward to work with. 
        /// For example, with a logarithmic curve, the audible difference between 10000 and 9000,
        /// is not the same as between 9000 and 8000. 
        /// With a linear "curve" the audible difference is spread equally across the whole range of values, 
        /// so in the previous example the audible difference between 10000 and 9000,
        /// and between 9000 and 8000 would be identical.
        /// When using the linear curve, the volume range is from 0% (silent) to 100% (full). 
        /// When using the logarithmic curve, the volume range is from -100 dB (effectively silent) to 0 dB (full).
        /// For example, a volume level of 0.5 is 50% linear or -50 dB logarithmic.
        /// The linear curve is used by default.
        /// </summary>
        public static bool LogarithmicVolumeCurve
        {
            get { return GetConfigBool(Configuration.LogarithmicVolumeCurve); }
            set { Configure(Configuration.LogarithmicVolumeCurve, value); }
        }

        /// <summary>
        /// Panning translation curve.
        /// logpan (bool): Panning curve... false = linear, true = logarithmic.
        /// The panning curve affects panning in exactly the same way as the volume curve (LogarithmicVolumeCurve) affects the volume.
        /// The linear curve is used by default.
        /// </summary>
        public static bool LogarithmicPanningCurve
        {
            get { return GetConfigBool(Configuration.LogarithmicPanCurve); }
            set { Configure(Configuration.LogarithmicPanCurve, value); }
        }

        /// <summary>
        /// Pass 32-bit floating-point sample data to all DSP functions?
        /// floatdsp (bool): If true, 32-bit floating-point sample data is passed 
        /// to all DSPProcedure callback functions.
        /// Normally DSP functions receive sample data in whatever format the channel is using, ie. it can be 8, 16 or 32-bit. 
        /// But using this config option, BASS will convert 8/16-bit sample data to 32-bit floating-point before passing 
        /// it to DSP functions, and then convert it back after all the DSP functions are done. 
        /// As well as simplifying the DSP code (no need for 8/16-bit processing), 
        /// this also means that there is no degradation of quality as sample data passes through a chain of DSP.
        /// This config option also applies to effects set via Un4seen.Bass.Bass.BASS_ChannelSetFX(System.Int32,Un4seen.Bass.BASSFXType,System.Int32),
        /// except for DX8 effects when using the "With FX flag" DX8 effect implementation.
        /// Changing the setting while there are DSP or FX set could cause problems, so should be avoided.
        /// Platform-specific: On Android and Windows CE, 8.24 bit fixed-point is used instead of floating-point. 
        /// Floating-point DX8 effect processing requires DirectX 9 (or above) on Windows.
        /// </summary>
        public static bool FloatingPointDSP
        {
            get { return GetConfigBool(Configuration.FloatDSP); }
            set { Configure(Configuration.FloatDSP, value); }
        }

        /// <summary>
        /// The number of threads to use for updating playback buffers.
        /// threads (int): The number of threads to use... 0 = disable automatic updating.
        /// The number of update threads determines how many HSTREAM/HMUSIC channel playback
        /// buffers can be updated in parallel;
        /// each thread can process one channel at a time. 
        /// The default is to use a single thread, but additional threads can be used to take advantage of multiple CPU cores.
        /// There is generally nothing much to be gained by creating more threads than there are CPU cores,
        /// but one benefit of using multiple threads even with a single CPU core is that
        /// a slow updating channel need not delay the updating of other channels.
        /// When automatic updating is disabled (threads = 0), Bass.Update() or Bass.ChannelUpdate() should be used instead.
        /// The number of update threads can be changed at any time, including during playback.
        /// Platform-specific: The number of update threads is limited to 1 on Windows CE platforms.
        /// </summary>
        public static int UpdateThreads
        {
            get { return GetConfig(Configuration.UpdateThreads); }
            set { Configure(Configuration.UpdateThreads, value); }
        }

        /// <summary>
        /// The buffer length for asynchronous file reading (default setting is 65536 bytes (64KB)).
        /// length (int): The buffer length in bytes.
        /// This will be rounded up to the nearest 4096 byte (4KB) boundary.
        /// This determines the amount of file data that can be read ahead of time with asynchronous file reading.
        /// Changes only affect streams that are created afterwards, not any that already exist. 
        /// So it is possible to have streams with differing buffer lengths 
        /// by using this config option before creating each of them.
        /// When asynchronous file reading is enabled, the buffer level is available from Bass.StreamGetFilePosition().
        /// </summary>
        public static int AsyncFileBufferLength
        {
            get { return GetConfig(Configuration.AsyncFileBufferLength); }
            set { Configure(Configuration.AsyncFileBufferLength, value); }
        }

        /// <summary>
        /// Gets the total number of HSTREAM/HSAMPLE/HMUSIC/HRECORD handles.
        /// none: only used with Bass.GetConfig().
        /// The handle count may not only include the app-created stuff but also internal stuff, 
        /// eg. BassWasapi.Init() will create a stream when the Configuration.WasapiBuffer flag is used.
        /// </summary>
        public static int HandleCount { get { return GetConfig(Configuration.HandleCount); } }

        /// <summary>
        /// Time to wait for a server to respond to a connection request.
        /// timeout (int): The time to wait, in milliseconds.
        /// The default timeout is 5 seconds (5000 milliseconds).
        /// </summary>
        public static int NetTimeOut
        {
            get { return GetConfig(Configuration.NetTimeOut); }
            set { Configure(Configuration.NetTimeOut, value); }
        }

        /// <summary>
        /// The time to wait for a server to deliver more data for an internet stream.
        /// timeout (int): The time to wait in milliseconds (default=0, infinite).
        /// When the timeout is hit, the connection with the server will be closed. 
        /// The default setting is 0, no timeout.
        /// </summary>
        public static int NetReadTimeOut
        {
            get { return GetConfig(Configuration.NetReadTimeOut); }
            set { Configure(Configuration.NetReadTimeOut, value); }
        }

        /// <summary>
        /// The internet download buffer length.
        /// length (int): The buffer length, in milliseconds.
        /// Increasing the buffer length decreases the chance of the stream stalling,
        /// but also increases the time taken by Bass.CreateStream(url)
        /// to create the stream, as it has to pre-buffer more data (adjustable via the Configuration.NetPreBuffer option).
        /// Aside from the pre-buffering, this setting has no effect on streams without either the BASSFlag.BASS_STREAM_BLOCK
        /// or BASSFlag.BASS_STREAM_RESTRATE flags.
        /// When streaming in blocks, this option determines the download buffer length.
        /// The effective buffer length can actually be a bit more than that specified,
        /// including data that has been read from the buffer by the decoder but not yet decoded.
        /// This config option also determines the buffering used by "buffered" user file streams created with Bass.StreamCreateFileUser().
        /// The default buffer length is 5 seconds (5000 milliseconds). 
        /// The net buffer length should be larger than the length of the playback buffer (Un4seen.Bass.BASSConfig.BASS_CONFIG_BUFFER),
        /// otherwise the stream is likely to briefly stall soon after starting playback.
        /// Using this config option only affects streams created afterwards, not any that have already been created.
        /// </summary>
        public static int NetBufferLength
        {
            get { return GetConfig(Configuration.NetBufferLength); }
            set { Configure(Configuration.NetBufferLength, value); }
        }

        /// <summary>
        /// Prevent channels being played when the output is paused?
        /// noplay (bool): If true, channels can't be played while the output is paused.
        /// When the output is paused using Bass.Pause(), and this config option is enabled,
        /// channels can't be played until the output is resumed using Bass.Start(). 
        /// Attempts to play a channel will give a BASS_ERROR_START error.
        /// By default, this config option is enabled.
        /// </summary>
        public static int PauseNoPlay
        {
            get { return GetConfig(Configuration.PauseNoPlay); }
            set { Configure(Configuration.PauseNoPlay, value); }
        }

        /// <summary>
        /// Amount to pre-buffer when opening internet streams.
        /// prebuf (int): Amount (percentage) to pre-buffer.
        /// This setting determines what percentage of the buffer length (NetBufferLength)
        /// should be filled by Bass.CreateStream(URL).
        /// The default is 75%. 
        /// Setting this lower (eg. 0) is useful if you want to display a "buffering progress" (using Bass.StreamGetFilePosition())
        /// when opening internet streams, but note that this setting is just a minimum. 
        /// BASS will always pre-download a certain amount to verify the stream.
        /// As well as internet streams, this config setting also applies to "buffered" user file streams created with Bass.StreamCreateFileUser().
        /// </summary>
        public static int NetPreBuffer
        {
            get { return GetConfig(Configuration.NetPreBuffer); }
            set { Configure(Configuration.NetPreBuffer, value); }
        }

        /// <summary>
        /// Use passive mode in FTP connections?
        /// passive (bool): If true, passive mode is used, otherwise normal/active mode is used.
        /// Changes take effect from the next internet stream creation call. 
        /// By default, passive mode is enabled.
        /// </summary>
        public static bool NetPassive
        {
            get { return GetConfigBool(Configuration.NetPassive); }
            set { Configure(Configuration.NetPassive, value); }
        }

        /// <summary>
        /// Process URLs in PLS, M3U, WPL or ASX playlists?
        /// netlists (int): When to process URLs in PLS, M3U, WPL or ASX playlists...
        /// 0 = never,
        /// 1 = in Bass.CreateStream(URL) only,
        /// 2 = in Bass.CreateStream(File) and Bass.StreamCreateFileUser() too.
        /// When enabled, BASS will process PLS, M3U, WPL and ASX playlists, 
        /// going through each entry until it finds a URL that it can play.
        /// By default, playlist procesing is disabled.
        /// </summary>
        public static int NetPlaylist
        {
            get { return GetConfig(Configuration.NetPlaylist); }
            set { Configure(Configuration.NetPlaylist, value); }
        }

        /// <summary>
        /// The "User-Agent" request header sent to servers.
        /// agent (string pointer): The "User-Agent" header.
        /// BASS does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
        /// This also means that the agent setting can subsequently be changed at that location without having to call this function again.
        /// Changes take effect from the next internet stream creation call.
        /// </summary>
        public static string NetAgent
        {
            get { return Marshal.PtrToStringAnsi(GetConfigPtr(Configuration.NetAgent)); }
            set
            {
                if (_netAgent != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_netAgent);
                    _netAgent = IntPtr.Zero;
                }

                _netAgent = Marshal.StringToHGlobalAnsi(value);

                Configure(Configuration.NetAgent, _netAgent);
            }
        }

        /// <summary>
        /// Proxy server settings (in the form of "user:pass@server:port"... null = don't use a proxy).
        /// proxy (string pointer): The proxy server settings, in the form of "user:pass@server:port"...
        /// NULL = don't use a proxy. "" (empty string) = use the OS's default proxy settings.
        /// If only the "user:pass@" part is specified, then those authorization credentials are used with the default proxy server.
        /// If only the "server:port" part is specified, then that proxy server is used without any authorization credentials.
        /// BASS does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
        /// This also means that the proxy settings can subsequently be changed at that location without having to call this function again.
        /// Changes take effect from the next internet stream creation call.
        /// </summary>
        public static string NetProxy
        {
            get { return Marshal.PtrToStringAnsi(GetConfigPtr(Configuration.NetProxy)); }
            set
            {
                if (_netProxy != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_netProxy);
                    _netProxy = IntPtr.Zero;
                }

                _netProxy = Marshal.StringToHGlobalAnsi(value);

                Configure(Configuration.NetProxy, _netProxy);
            }
        }

        /// <summary>
        /// The maximum number of virtual channels to use in the rendering of IT files.
        /// number (int): The number of virtual channels... 1 (min) to 512 (max).
        /// If the value specified is outside this range, it is automatically capped.
        /// This setting only affects IT files, as the other MOD music formats do not have virtual channels. 
        /// The default setting is 64. 
        /// Changes only apply to subsequently loaded files, not any that are already loaded.
        /// </summary>
        public static int MusicVirtial
        {
            get { return GetConfig(Configuration.MusicVirtual); }
            set { Configure(Configuration.MusicVirtual, value); }
        }

        /// <summary>
        /// The amount of data to check in order to verify/detect the file format.
        /// length (int): The amount of data to check, in bytes... 1000 (min) to 100000 (max). 
        /// If the value specified is outside this range, it is automatically capped.
        /// Of the file formats supported as standard, this setting only affects the detection of MP3/MP2/MP1 formats,
        /// but it may also be used by add-ons (see the documentation). 
        /// For internet (and "buffered" user file) streams, a quarter of the length is used, up to a minimum of 1000 bytes.
        /// The verification length excludes any tags that may be at the start of the file.
        /// The default length is 16000 bytes.
        /// For internet (and "buffered" user file) streams, the BASS_CONFIG_VERIFY_NET setting determines how much data is checked.
        /// </summary>
        public static int FileVerificationBytes
        {
            get { return GetConfig(Configuration.FileVerificationBytes); }
            set { Configure(Configuration.FileVerificationBytes, value); }
        }

        /// <summary>
        /// The amount of data to check in order to verify/detect the file format of internet streams.
        /// length (int): The amount of data to check, in bytes... 1000 (min) to 1000000 (max),
        /// or 0 = 25% of the BASS_CONFIG_VERIFY setting (with a minimum of 1000 bytes). 
        /// If the value specified is outside this range, it is automatically capped.
        /// Of the file formats supported as standard, this setting only affects the detection of MP3/MP2/MP1 formats, 
        /// but it may also be used by add-ons (see the documentation). 
        /// The verification length excludes any tags that may be found at the start of the file. 
        /// The default setting is 0, which means 25% of the BASS_CONFIG_VERIFY setting.
        /// As well as internet streams, this config setting also applies to "buffered" user file streams 
        /// created with Bass.StreamCreateFileUser().
        /// </summary>
        public static int NetVerificationBytes
        {
            get { return GetConfig(Configuration.NetVerificationBytes); }
            set { Configure(Configuration.NetVerificationBytes, value); }
        }

        /// <summary>
        /// Linux, Android and CE only: The output device buffer length.
        /// length (int): The buffer length in milliseconds.
        /// The device buffer is where the final mix of all playing channels is placed, ready for the device to play. 
        /// Its length affects the latency of things like starting and stopping playback of a channel, 
        /// so you will probably want to avoid setting it unnecessarily high, 
        /// but setting it too short could result in breaks in the output.
        /// When using a large device buffer, the ChannelAttribute.NoBuffer attribute could be used to skip the channel buffering stage,
        /// to avoid further increasing latency for real-time generated sound and/or DSP/FX changes.
        /// Changes to this config setting only affect subsequently initialized devices, not any that are already initialized.
        /// This config option is only available on Linux, Android and Windows CE. 
        /// The device's buffer is determined automatically on other platforms.
        /// Platform-specific: On Linux, the driver may choose to use a different buffer length
        /// if it decides that the specified length is too short or long. 
        /// The buffer length actually being used can be obtained with BassInfo, like this: latency + minbuf / 2.
        /// </summary>
        public static int DeviceBufferLength
        {
            get { return GetConfig(Configuration.DeviceBufferLength); }
            set { Configure(Configuration.DeviceBufferLength, value); }
        }

        /// <summary>
        /// Enable true play position mode on Windows Vista and newer?
        /// truepos (bool): If enabled, DirectSound's 'true play position' mode is enabled on Windows Vista and newer (default is true).
        /// Unless this option is enabled, the reported playback position will advance in 10ms steps on Windows Vista and newer. 
        /// As well as affecting the precision of Bass.ChannelGetPosition(), this also affects the timing of non-mixtime syncs.
        /// When this option is enabled, it allows finer position reporting but it also increases latency.
        /// The default setting is enabled. 
        /// Changes only affect channels that are created afterwards, not any that already exist. 
        /// The latency and minbuf values in the BassInfo structure reflect the setting at the time of the device's Bass.Init() call.
        /// </summary>
        public static bool TruePlayPosition
        {
            get { return GetConfigBool(Configuration.TruePlayPosition); }
            set { Configure(Configuration.TruePlayPosition, value); }
        }

        /// <summary>
        /// Suppress silencing for corrupted MP3 frames.
        /// errors (bool): Suppress error correction silences? (default is false).
        /// When BASS is detecting some corruption in an MP3 file's Huffman coding,
        /// it silences the frame to avoid any unpleasent noises that can result from corruption.
        /// Set this parameter to true in order to suppress this behavior.
        /// This applies only to the regular BASS version and NOT the "mp3-free" version.
        /// </summary>
        public static bool SuppressMP3ErrorCorruptionSilence
        {
            get { return GetConfigBool(Configuration.SuppressMP3ErrorCorruptionSilence); }
            set { Configure(Configuration.SuppressMP3ErrorCorruptionSilence, value); }
        }

        /// <summary>
        /// Windows-only: Include a "Default" entry in the output device list?
        /// default (bool): If true, a 'Default' device will be included in the device list (default is false).
        /// BASS does not usually include a "Default" entry in its device list, 
        /// as that would ultimately map to one of the other devices and be a duplicate entry.
        /// When the default device is requested in a Bass.Init() call (with device = -1),
        /// BASS will check the default device at that time, and initialize it. 
        /// But Windows 7 has the ability to automatically switch the default output to the new default device whenever it changes,
        /// and in order for that to happen, the default device (rather than a specific device) needs to be used. 
        /// That is where this option comes in.
        /// When enabled, the "Default" device will also become the default device to Bass.Init() (with device = -1). 
        /// When the "Default" device is used, the Bass.SetVolume() and Bass.GetVolume() functions work a bit differently to usual;
        /// they deal with the "session" volume, which only affects the current process's output on the device, rather than the device's volume.
        /// This option can only be set before Bass.GetDeviceInfo() or Bass.Init() has been called.
        /// Platform-specific: This config option is only available on Windows. 
        /// It is available on all Windows versions (not including CE), but only Windows 7 has the default output switching feature.
        /// </summary>
        public static bool IncludeDefaultDevice
        {
            get { return GetConfigBool(Configuration.IncludeDefaultDevice); }
            set { Configure(Configuration.IncludeDefaultDevice, value); }
        }

        /// <summary>
        /// Enable speaker assignment with panning/balance control on Windows Vista and newer?
        /// enable (bool): If true, speaker assignment with panning/balance control is enabled on Windows Vista and newer.
        /// Panning/balance control via the ChannelAttributes.Pan attribute is not available 
        /// when speaker assignment is used on Windows due to the way that the speaker assignment needs to be implemented there. 
        /// The situation is improved with Windows Vista, and speaker assignment can generally 
        /// be done in a way that does permit panning/balance control to be used at the same time,
        /// but there may still be some drivers that it does not work properly with, 
        /// so it is disabled by default and can be enabled via this config option.
        /// Changes only affect channels that are created afterwards, not any that already exist.
        /// Platform-specific: This config option is only available on Windows. 
        /// It is available on all Windows versions (not including CE), but only has effect on Windows Vista and newer. 
        /// Speaker assignment with panning/balance control is always possible on other platforms,
        /// where BASS generates the final mix.
        /// </summary>
        public static bool EnableSpeakerAssignment
        {
            get { return GetConfigBool(Configuration.EnableSpeakerAssignment); }
            set { Configure(Configuration.EnableSpeakerAssignment, value); }
        }

        /// <summary>
        /// Gets or Sets the Unicode character set in device information.
        /// utf8 (bool): If true, device information will be in UTF-8 form.
        /// Otherwise it will be ANSI.
        /// This config option determines what character set is used in the 
        /// DeviceInfo structure and by the Bass.RecordGetInputName() function. 
        /// The default setting is ANSI, and it can only be changed before 
        /// Bass.GetDeviceInfo() or Bass.Init() or Bass.RecordGetDeviceInfo() or Bass.RecordInit() has been called.
        /// Platform-specific: This config option is only available on Windows.
        /// </summary>
        public static bool UnicodeDeviceInformation
        {
            get { return GetConfigBool(Configuration.UnicodeDeviceInformation); }
            set { Configure(Configuration.UnicodeDeviceInformation, value); }
        }

        /// <summary>
        /// Gets or Sets the default sample rate conversion quality.
        /// quality (int): The sample rate conversion quality... 
        /// 0 = linear interpolation,
        /// 1 = 8 point sinc interpolation,
        /// 2 = 16 point sinc interpolation,
        /// 3 = 32 point sinc interpolation. 
        /// Other values are also accepted.
        /// This config option determines what sample rate conversion 
        /// quality new channels will initially have, except for sample channels (HCHANNEL),
        /// which use the BASS_CONFIG_SRC_SAMPLE setting.  
        /// A channel's sample rate conversion quality can subsequently
        /// be changed via the BASS_ATTRIB_SRC attribute (see Bass.ChannelSetAttribute()).
        /// The default setting is 1 (8 point sinc interpolation).
        /// </summary>
        public static int SRCQuality
        {
            get { return GetConfig(Configuration.SRCQuality); }
            set { Configure(Configuration.SRCQuality, value); }
        }

        /// <summary>
        /// Gets or Sets the default sample rate conversion quality for samples.
        /// quality (int): The sample rate conversion quality...
        /// 0 = linear interpolation,
        /// 1 = 8 point sinc interpolation,
        /// 2 = 16 point sinc interpolation,
        /// 3 = 32 point sinc interpolation. 
        /// Other values are also accepted.
        /// This config option determines what sample rate conversion quality a new sample
        /// channel will initially have, following a Bass.SampleGetChannel() call.
        /// The channel's sample rate conversion quality can subsequently be changed 
        /// via the BASS_ATTRIB_SRC attribute (see Bass.ChannelSetAttribute()).
        /// The default setting is 0 (linear interpolation).
        /// </summary>
        public static int SampleSRCQuality
        {
            get { return GetConfig(Configuration.SampleSRCQuality); }
            set { Configure(Configuration.SampleSRCQuality, value); }
        }

        /// <summary>
        /// Pre-scan chained OGG files?
        /// prescan (bool): If true, chained OGG files are pre-scanned.
        /// This option is enabled by default, and is equivalent to including the BASS_STREAM_PRESCAN flag
        /// in a Bass.StreamCreateFile() call when opening an OGG file.
        /// It can be disabled if seeking and an accurate length reading are not required from chained OGG files,
        /// for faster stream creation.
        /// </summary>
        public static bool OggPreScan
        {
            get { return GetConfigBool(Configuration.OggPreScan); }
            set { Configure(Configuration.OggPreScan, value); }
        }

        /// <summary>
        /// Play the audio from video files using Media Foundation?
        /// video (bool): If true (default) BASS will Accept video files.
        /// This config option is only available on Windows, and only has effect on Windows Vista and newer.
        /// </summary>
        public static bool MFVideo
        {
            get { return GetConfigBool(Configuration.MFVideo); }
            set { Configure(Configuration.MFVideo, value); }
        }
    }
}