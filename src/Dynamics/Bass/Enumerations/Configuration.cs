namespace ManagedBass.Dynamics
{
    enum Configuration
    {
        PlaybackBufferLength = 0,
        UpdatePeriod = 1,        
        GlobalSampleVolume = 4,
        GlobalStreamVolume = 5,
        GlobalMusicVolume = 6,
        LogarithmicVolumeCurve = 7,
        LogarithmicPanCurve = 8,
        FloatDSP = 9,
        Algorithm3D = 10,
        NetTimeOut = 11,
        NetBufferLength = 12,
        PauseNoPlay = 13,
        NetPreBuffer = 15,
        NetAgent = 16,
        NetProxy = 17,
        NetPassive = 18,
        RecordingBufferLength = 19,
        NetPlaylist = 21,
        MusicVirtual = 22,
        FileVerificationBytes = 23,
        UpdateThreads = 24,
        DeviceBufferLength = 27,
        TruePlayPosition = 30,
        IOSMixAudio = 34,
        SuppressMP3ErrorCorruptionSilence = 35,
        IncludeDefaultDevice = 36,
        NetReadTimeOut = 37,
        VistaSpeakerAssignment = 38,

        // TODO: Implement Config
        IOSSpeaker = 39,
        MFDisable = 40,
        HandleCount = 41,
        UnicodeDeviceInformation = 42,
        SRCQuality = 43,
        SampleSRCQuality = 44,
        AsyncFileBufferLength = 45,
        IOSNotify = 46,
        OggPreScan = 47,
        MFVideo = 48,
        Airplay = 49,
        DevNonStop = 50,

        // TODO: Implement Config
        IOSNoCategory = 51,
        NetVerificationBytes = 52,
        AC3DynamicRangeCompression = 65537,
        WmaNetPreBuffer = 65793,
        WmaBassFileHandling = 65795,
        WmaNetSeek = 65796,
        WmaVideo = 65797,
        WmaAsync = 65807,
        CDFreeOld = 66048,
        CDRetry = 66049,
        CDAutoSpeed = 66050,
        CDSkipError = 66051,
        CDDBServer = 66052,
        EncodePriority = 66304,
        EncodeQueue = 66305,
        EncodeACMLoad = 66306,
        EncodeCastTimeout = 66320,
        EncodeCastProxy = 66321,
        MidiCompact = 66560,
        MidiVoices = 66561,
        MidiAutoFont = 66562,
        MidiDefaultFont = 66563,
        MidiInputPorts = 66564,
        MixerBufferLength = 67073,
        MixerPositionEx = 67074,
        SplitBufferLength = 67088,
        PlayAudioFromMp4 = 67328,
        AacSupportMp4 = 67329,

        // TODO: Implement Config
        /// <summary>
        /// BASSdsd add-on: the default sample rate when converting to PCM.
        /// <para>freq (int): the sample rate.</para>
        /// <para>This setting determines what sample rate is used by default when converting to PCM. The rate actually used may be different if the specified rate is not valid for a particular DSD rate, in which case it will be rounded up (or down if there are none higher) to the nearest valid rate; the valid rates are 1/8, 1/16, 1/32, etc. of the DSD rate down to a minimum of 44100 Hz.</para>
        /// <para>The default setting is 88200 Hz. Changes only affect subsequently created streams, not any that already exist.</para>
        /// </summary>
        BASS_CONFIG_DSD_FREQ = 67584,

        // TODO: Implement Config
        /// <summary>
        /// BASSWinamp add-on: Winamp input timeout.
        /// <para>timeout (int): The time (in milliseconds) to wait until timing out, because the plugin is not using the output system.</para>
        /// </summary>
        BASS_CONFIG_WINAMP_INPUT_TIMEOUT = 67584,

        // TODO: Implement Config
        /// <summary>
        /// BASSdsd add-on: the default gain applied when converting to PCM.
        /// <para>gain (int): the gain in decibels.</para>
        /// <para>This setting determines what gain is applied by default when converting to PCM. Changes only affect subsequently created streams, not any that already exist. An existing stream's gain can be changed via the <see cref="F:Un4seen.Bass.BASSAttribute.BASS_ATTRIB_DSD_GAIN" /> attribute.</para>
        /// <para>The default setting is 6dB.</para>
        /// </summary>
        BASS_CONFIG_DSD_GAIN = 67585
    }
}