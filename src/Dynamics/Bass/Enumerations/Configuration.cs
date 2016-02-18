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
        
        // TODO: Implement Config
        IOSMixAudio = 34,
        SuppressMP3ErrorCorruptionSilence = 35,
        IncludeDefaultDevice = 36,
        NetReadTimeOut = 37,
        VistaSpeakerAssignment = 38,

        // TODO: Implement Config
        IOSSpeaker = 39,

        // TODO: Implement Config
        MFDisable = 40,
        HandleCount = 41,
        UnicodeDeviceInformation = 42,
        SRCQuality = 43,
        SampleSRCQuality = 44,
        AsyncFileBufferLength = 45,

        // TODO: Implement Config
        IOSNotify = 46,
        OggPreScan = 47,
        MFVideo = 48,

        // TODO: Implement Config
        Airplay = 49,

        // TODO: Implement Config
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
    }
}