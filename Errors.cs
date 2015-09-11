using ManagedBass.Dynamics;

namespace ManagedBass
{
    public enum Errors
    {
        /// <summary>
        /// Some other mystery error
        /// </summary>     
        Unknown = -1,

        /// <summary>
        /// No Error
        /// </summary>
        OK = 0,

        /// <summary>
        /// Memory Error
        /// </summary>
        Memory = 1,

        //
        // Summary:
        //     Can't open the file
        FileOpen = 2,
        //
        // Summary:
        //     Can't find a free/valid driver
        DriverNotFound = 3,
        //
        // Summary:
        //     The sample buffer was lost
        BufferLost = 4,
        //
        // Summary:
        //     Invalid handle
        InvalidHandle = 5,
        //
        // Summary:
        //     Unsupported sample format
        UnsupportedSampleFormat = 6,
        //
        // Summary:
        //     Invalid playback position
        InvalidPlaybackPosition = 7,
        //
        // Summary:
        //     BASS_Init has not been successfully called
        NotInitialised = 8,
        //
        // Summary:
        //     BASS_Start has not been successfully called
        OutputNotStarted = 9,
        //
        // Summary:
        //     No CD in drive
        NoCDinDrive = 12,
        //
        // Summary:
        //     Invalid track number
        InvalidCDTrack = 13,
        //
        // Summary:
        //     Already initialized/paused/whatever
        Already = 14,
        //
        // Summary:
        //     Not paused
        NotPaused = 16,
        //
        // Summary:
        //     Not an audio track
        NotAudioTrack = 17,
        //
        // Summary:
        //     Can't get a free channel
        NoFreeChannelAvailable = 18,
        //
        // Summary:
        //     An illegal type was specified
        IllegalType = 19,
        //
        // Summary:
        //     An illegal parameter was specified
        IllegalParameter = 20,
        //
        // Summary:
        //     No 3D support
        No3D = 21,
        //
        // Summary:
        //     No EAX support
        NoEAX = 22,
        //
        // Summary:
        //     Illegal device number
        IllegalDevice = 23,
        //
        // Summary:
        //     Not playing
        NotPlaying = 24,
        //
        // Summary:
        //     Illegal sample rate
        IllegalSampleRate = 25,
        //
        // Summary:
        //     The stream is not a file stream
        NotFileStream = 27,
        //
        // Summary:
        //     No hardware voices available
        NoHardwareVoicesAvailable = 29,
        //
        // Summary:
        //     The MOD music has no sequence data
        NoSequenceData = 31,
        //
        // Summary:
        //     No internet connection could be opened
        NoInternetConnection = 32,
        //
        // Summary:
        //     Couldn't create the file
        FileCreate = 33,
        //
        // Summary:
        //     Effects are not available
        EffectsNotAvailable = 34,
        //
        // Summary:
        //     The channel is playing
        Playing = 35,
        //
        // Summary:
        //     Requested data is not available
        DataNotAvailable = 37,
        //
        // Summary:
        //     The channel is a 'decoding channel'
        DecodingChannel = 38,
        //
        // Summary:
        //     A sufficient DirectX version is not installed
        DirectX = 39,
        //
        // Summary:
        //     Connection timedout
        ConnectionTimedout = 40,
        //
        // Summary:
        //     Unsupported file format
        UnsupportedFileFormat = 41,
        //
        // Summary:
        //     Unavailable speaker
        SpeakerUnavailable = 42,
        //
        // Summary:
        //     Invalid BASS version (used by add-ons)
        InvalidVersion = 43,
        //
        // Summary:
        //     Codec is not available/supported
        CodecNotAvailable = 44,
        //
        // Summary:
        //     The channel/file has ended
        Ended = 45,
        //
        // Summary:
        //     The device is busy (eg. in "exclusive" use by another process)
        DeviceBusy = 46,
        //
        // Summary:
        //     BassWma: the file is protected
        WmaLicense = 1000,
        //
        // Summary:
        //     BassWma: WM9 is required
        WM9 = 1001,
        //
        // Summary:
        //     BassWma: access denied (user/pass is invalid)
        WmaAccesDenied = 1002,
        //
        // Summary:
        //     BassWma: no appropriate codec is installed
        WmaCodecNotInstalled = 1003,
        //
        // Summary:
        //     BassWma: individualization is needed
        WmaIndividualisationNeeded = 1004,
        //
        // Summary:
        //     BASSWASAPI: no WASAPI available
        WasapiNotAvailable = 5000
    }

    public class Return<T>
    {
        public Errors ErrorCode { get; set; }

        Return() { }

        public T Value { get; set; }

        public static implicit operator T(Return<T> e) { return e.Value; }

        public static implicit operator Return<T>(T e)
        {
            Return<T> New = new Return<T>();
            New.ErrorCode = Bass.LastError;
            New.Value = e;
            return New;
        }

        public bool Success { get { return ErrorCode == Errors.OK; } }
    }
}