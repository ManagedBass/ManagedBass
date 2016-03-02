using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    /// <summary>
    /// Bass Error Codes returned by <see cref="Bass.LastError" />.
    /// </summary>
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

        /// <summary>
        /// Can't open the file
        /// </summary>
        FileOpen = 2,

        /// <summary>
        /// Can't find a free/valid driver
        /// </summary>
        DriverNotFound = 3,

        /// <summary>
        /// The sample Buffer was lost
        /// </summary>
        BufferLost = 4,

        /// <summary>
        /// Invalid Handle
        /// </summary>
        InvalidHandle = 5,

        /// <summary>
        /// Unsupported sample format
        /// </summary>
        UnsupportedSampleFormat = 6,

        /// <summary>
        /// Invalid playback position
        /// </summary>
        InvalidPlaybackPosition = 7,

        /// <summary>
        /// <see cref="Bass.Init"/> has not been successfully called
        /// </summary>
        NotInitialised = 8,

        /// <summary>
        /// <see cref="Bass.Start"/> has not been successfully called
        /// </summary>
        OutputNotStarted = 9,

        /// <summary>
        /// No CD in drive
        /// </summary>
        NoCDinDrive = 12,

        /// <summary>
        /// Invalid track number
        /// </summary>
        InvalidCDTrack = 13,

        /// <summary>
        /// Already initialized/paused/whatever
        /// </summary>
        Already = 14,

        /// <summary>
        /// Not paused
        /// </summary>
        NotPaused = 16,

        /// <summary>
        /// Not an audio track
        /// </summary>
        NotAudioTrack = 17,

        /// <summary>
        /// Can't get a free channel
        /// </summary>
        NoFreeChannelAvailable = 18,

        /// <summary>
        /// An illegal Type was specified
        /// </summary>
        IllegalType = 19,

        /// <summary>
        /// An illegal parameter was specified
        /// </summary>
        IllegalParameter = 20,

        /// <summary>
        /// No 3D support
        /// </summary>
        No3D = 21,

        /// <summary>
        /// No EAX support
        /// </summary>
        NoEAX = 22,

        /// <summary>
        /// Illegal device number
        /// </summary>
        IllegalDevice = 23,

        /// <summary>
        /// Not playing
        /// </summary>
        NotPlaying = 24,

        /// <summary>
        /// Illegal sample rate
        /// </summary>
        IllegalSampleRate = 25,

        /// <summary>
        /// The stream is not a file stream
        /// </summary>
        NotFileStream = 27,

        /// <summary>
        /// No hardware voices available
        /// </summary>
        NoHardwareVoicesAvailable = 29,

        /// <summary>
        /// The MOD music has no sequence data
        /// </summary>
        NoSequenceData = 31,

        /// <summary>
        /// No internet connection could be opened
        /// </summary>
        NoInternetConnection = 32,

        /// <summary>
        /// Couldn't create the file
        /// </summary>
        FileCreate = 33,

        /// <summary>
        /// Effects are not available
        /// </summary>
        EffectsNotAvailable = 34,

        /// <summary>
        /// The channel is playing
        /// </summary>
        Playing = 35,

        /// <summary>
        /// Requested data is not available
        /// </summary>
        DataNotAvailable = 37,

        /// <summary>
        /// The channel is a 'Decoding Channel'
        /// </summary>
        DecodingChannel = 38,

        /// <summary>
        /// A sufficient DirectX version is not installed
        /// </summary>
        DirectX = 39,

        /// <summary>
        /// Connection timedout
        /// </summary>
        ConnectionTimedout = 40,

        /// <summary>
        /// Unsupported file format
        /// </summary>
        UnsupportedFileFormat = 41,

        /// <summary>
        /// Unavailable speaker
        /// </summary>
        SpeakerUnavailable = 42,

        /// <summary>
        /// Invalid BASS version (used by add-ons)
        /// </summary>
        InvalidVersion = 43,

        /// <summary>
        /// Codec is not available/supported
        /// </summary>
        CodecNotAvailable = 44,

        /// <summary>
        /// The channel/file has ended
        /// </summary>
        Ended = 45,

        /// <summary>
        /// The device is busy (eg. in "exclusive" use by another process)
        /// </summary>
        DeviceBusy = 46,

        #region BassWma
        /// <summary>
        /// BassWma: The file is protected
        /// </summary>
        WmaLicense = 1000,

        /// <summary>
        /// BassWma: WM9 is required
        /// </summary>
        WM9 = 1001,

        /// <summary>
        /// BassWma: Access denied (Username/Password is invalid)
        /// </summary>
        WmaAccesDenied = 1002,

        /// <summary>
        /// BassWma: No appropriate codec is installed
        /// </summary>
        WmaCodecNotInstalled = 1003,

        /// <summary>
        /// BassWma: individualization is needed
        /// </summary>
        WmaIndividualisationNeeded = 1004,
        #endregion

        /// <summary>
        /// BassWASAPI: WASAPI Not available
        /// </summary>
        WasapiNotAvailable = 5000,

        /// <summary>
        /// BassAAC: Non-Streamable due to MP4 atom order ("mdat" before "moov")
        /// </summary>
        Mp4NoStream = 6000
    }

    public class BassException : Exception
    {
        internal BassException(Errors ErrorCode, string ExceptionPoint)
            : base(string.Format("{0} occured at {1}", ErrorCode.ToString(), ExceptionPoint))
        {
            this.ExceptionPoint = ExceptionPoint;
            this.ErrorCode = ErrorCode;
        }

        public string ExceptionPoint { get; }

        public Errors ErrorCode { get; }
    }
}
