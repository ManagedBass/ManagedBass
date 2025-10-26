using System;
using System.ComponentModel;

namespace ManagedBass
{
    /// <summary>
    /// Bass Error Codes returned by <see cref="Bass.LastError" /> and BassAsio.LastError.
    /// </summary>
    public enum Errors
    {
        /// <summary>
        /// Some other mystery error
        /// </summary>
        [Description("Some other mystery error")]
        Unknown = -1,

        /// <summary>
        /// No Error
        /// </summary>
        [Description("No error")]
        OK = 0,

        /// <summary>
        /// Memory Error
        /// </summary>
        [Description("Memory error")]
        Memory = 1,

        /// <summary>
        /// Can't open the file
        /// </summary>
        [Description("Can't open the file")]
        FileOpen = 2,

        /// <summary>
        /// Can't find a free/valid driver
        /// </summary>
        [Description("Can't find a free/valid driver")]
        Driver = 3,

        /// <summary>
        /// The sample Buffer was lost
        /// </summary>
        [Description("The sample buffer was lost")]
        BufferLost = 4,

        /// <summary>
        /// Invalid Handle
        /// </summary>
        [Description("Invalid handle")]
        Handle = 5,

        /// <summary>
        /// Unsupported sample format
        /// </summary>
        [Description("Unsupported sample format")]
        SampleFormat = 6,

        /// <summary>
        /// Invalid playback position
        /// </summary>
        [Description("Invalid playback position")]
        Position = 7,

        /// <summary>
        /// <see cref="Bass.Init"/> has not been successfully called
        /// </summary>
        [Description("Bass.Init has not been successfully called")]
        Init = 8,

        /// <summary>
        /// <see cref="Bass.Start"/> has not been successfully called
        /// </summary>
        [Description("Bass.Start has not been successfully called")]
        Start = 9,

        /// <summary>
        /// SSL/HTTPS support isn't available.
        /// </summary>
        [Obsolete("Use SSL instead.")]
        [Description("SSL/HTTPS support isn't available")]
        SLL = 10,

        /// <summary>
        /// SSL/HTTPS support isn't available.
        /// </summary>
        [Description("SSL/HTTPS support isn't available")]
        SSL = 10,

        /// <summary>
        /// No CD in drive
        /// </summary>
        [Description("No CD in drive")]
        NoCD = 12,

        /// <summary>
        /// Invalid track number
        /// </summary>
        [Description("Invalid track number")]
        CDTrack = 13,

        /// <summary>
        /// Already initialized/paused/whatever
        /// </summary>
        [Description("Already initialized/paused/whatever")]
        Already = 14,

        /// <summary>
        /// Not paused
        /// </summary>
        [Description("Not paused")]
        NotPaused = 16,

        /// <summary>
        /// Not an audio track
        /// </summary>
        [Description("Not an audio track")]
        NotAudioTrack = 17,

        /// <summary>
        /// Can't get a free channel
        /// </summary>
        [Description("Can't get a free channel")]
        NoChannel = 18,

        /// <summary>
        /// An illegal Type was specified
        /// </summary>
        [Description("An illegal type was specified")]
        Type = 19,

        /// <summary>
        /// An illegal parameter was specified
        /// </summary>
        [Description("An illegal parameter was specified")]
        Parameter = 20,

        /// <summary>
        /// No 3D support
        /// </summary>
        [Description("No 3D support")]
        No3D = 21,

        /// <summary>
        /// No EAX support
        /// </summary>
        [Description("No EAX support")]
        NoEAX = 22,

        /// <summary>
        /// Illegal device number
        /// </summary>
        [Description("Illegal device number")]
        Device = 23,

        /// <summary>
        /// Not playing
        /// </summary>
        [Description("Not playing")]
        NotPlaying = 24,

        /// <summary>
        /// Illegal sample rate
        /// </summary>
        [Description("Illegal sample rate")]
        SampleRate = 25,

        /// <summary>
        /// The stream is not a file stream
        /// </summary>
        [Description("The stream is not a file stream")]
        NotFile = 27,

        /// <summary>
        /// No hardware voices available
        /// </summary>
        [Description("No hardware voices available")]
        NoHW = 29,

        /// <summary>
        /// The MOD music has no sequence data
        /// </summary>
        [Description("The MOD music has no sequence data")]
        Empty = 31,

        /// <summary>
        /// No internet connection could be opened
        /// </summary>
        [Description("No internet connection could be opened")]
        NoInternet = 32,

        /// <summary>
        /// Couldn't create the file
        /// </summary>
        [Description("Couldn't create the file")]
        Create = 33,

        /// <summary>
        /// Effects are not available
        /// </summary>
        [Description("Effects are not available")]
        NoFX = 34,

        /// <summary>
        /// The channel is playing
        /// </summary>
        [Description("The channel is playing")]
        Playing = 35,

        /// <summary>
        /// Requested data is not available
        /// </summary>
        [Description("Requested data is not available")]
        NotAvailable = 37,

        /// <summary>
        /// The channel is a 'Decoding Channel'
        /// </summary>
        [Description("The channel is a 'Decoding Channel'")]
        Decode = 38,

        /// <summary>
        /// A sufficient DirectX version is not installed
        /// </summary>
        [Description("A sufficient DirectX version is not installed")]
        DirectX = 39,

        /// <summary>
        /// Connection timed out
        /// </summary>
        [Description("Connection timed out")]
        Timeout = 40,

        /// <summary>
        /// Unsupported file format
        /// </summary>
        [Description("Unsupported file format")]
        FileFormat = 41,

        /// <summary>
        /// Unavailable speaker
        /// </summary>
        [Description("Unavailable speaker")]
        Speaker = 42,

        /// <summary>
        /// Invalid BASS version (used by add-ons)
        /// </summary>
        [Description("Invalid BASS version (used by add-ons)")]
        Version = 43,

        /// <summary>
        /// Codec is not available/supported
        /// </summary>
        [Description("Codec is not available/supported")]
        Codec = 44,

        /// <summary>
        /// The channel/file has ended
        /// </summary>
        [Description("The channel/file has ended")]
        Ended = 45,

        /// <summary>
        /// The device is busy (e.g. in exclusive use by another process)
        /// </summary>
        [Description("The device is busy (e.g. in exclusive use by another process)")]
        Busy = 46,

        /// <summary>
        /// The file cannot be streamed using the buffered file system.
        /// This could be because an MP4 file's "mdat" atom comes before its "moov" atom. 
        /// </summary>
        [Description("The file cannot be streamed using the buffered file system (e.g. 'mdat' before 'moov' in MP4)")]
        Unstreamable = 47,

        #region BassWma
        /// <summary>
        /// BassWma: The file is protected
        /// </summary>
        [Description("BassWma: The file is protected")]
        WmaLicense = 1000,

        /// <summary>
        /// BassWma: WM9 is required
        /// </summary>
        [Description("BassWma: WM9 is required")]
        WM9 = 1001,

        /// <summary>
        /// BassWma: Access denied (Username/Password is invalid)
        /// </summary>
        [Description("BassWma: Access denied (Username/Password is invalid)")]
        WmaAccesDenied = 1002,

        /// <summary>
        /// BassWma: No appropriate codec is installed
        /// </summary>
        [Description("BassWma: No appropriate codec is installed")]
        WmaCodec = 1003,

        /// <summary>
        /// BassWma: individualization is needed
        /// </summary>
        [Description("BassWma: Individualization is needed")]
        WmaIndividual = 1004,
        #endregion

        /// <summary>
        /// BassWASAPI: WASAPI Not available
        /// </summary>
        [Description("BassWASAPI: WASAPI not available")]
        Wasapi = 5000,

        /// <summary>
        /// BassEnc: ACM codec selection cancelled
        /// </summary>
        [Description("BassEnc: ACM codec selection cancelled")]
        AcmCancel = 2000,

        /// <summary>
        /// BassEnc: Access denied (invalid password)
        /// </summary>
        [Description("BassEnc: Access denied (invalid password)")]
        CastDenied = 2100,

        /// <summary>
        /// BassAAC: Non-Streamable due to MP4 atom order ("mdat" before "moov")
        /// </summary>
        [Description("BassAAC: Non-streamable due to MP4 atom order ('mdat' before 'moov')")]
        Mp4NoStream = 6000
    }
}
