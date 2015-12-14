using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum EncodeFlags
    {
        /// <summary>
        /// Cmdline is Unicode (16-bit characters).
        /// </summary>
        Unicode = -2147483648,
        
        /// <summary>
        /// Default option, incl. wave header, little-endian and no FP conversion.
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// Do NOT send a WAV header to the encoder.
        /// </summary>
        NoHeader = 1,
        
        /// <summary>
        /// Convert floating-point sample data to 8-bit integer.
        /// </summary>
        ConvertFloatTo8BitInt = 2,
        
        /// <summary>
        /// Convert floating-point sample data to 16-bit integer.
        /// </summary>
        ConvertFloatTo16BitInt = 4,
        
        /// <summary>
        /// Convert floating-point sample data to 24-bit integer.
        /// </summary>
        ConvertFloatTo24Bit = 6,
        
        /// <summary>
        /// Convert floating-point sample data to 32-bit integer.
        /// </summary>
        ConvertFloatTo32Bit = 8,
        
        /// <summary>
        /// Big-Endian sample data.
        /// </summary>
        BigEndian = 16,
        
        /// <summary>
        /// Start the encoder paused.
        /// </summary>
        Pause = 32,
        
        /// <summary>
        /// Write PCM sample data (no encoder).
        /// </summary>
        PCM = 64,
        
        /// <summary>
        /// Write RF64 WAV header (no encoder).
        /// </summary>
        RF64 = 128,
        
        /// <summary>
        /// Convert to mono (if not already).
        /// </summary>
        Mono = 256,
        //
        // Summary:
        //     Queue data to feed encoder asynchronously.
        //     The queue buffer will grow as needed to fit the data, but its size can be
        //     limited by the Un4seen.Bass.BASSConfig.BASS_CONFIG_ENCODE_QUEUE config option
        //     (0 = no limit); the default is 10000ms.  If the queue reaches the size limit
        //     and data is lost, the Un4seen.Bass.AddOn.Enc.BASSEncodeNotify.BASS_ENCODE_NOTIFY_QUEUE_FULL
        //     notification will be triggered.
        Queue = 512,
        //
        // Summary:
        //     Send the sample format information to the encoder in WAVEFORMATEXTENSIBLE
        //     form instead of WAVEFORMATEX form.
        //     This flag is ignored if the BASS_ENCODE_NOHEAD flag is used.
        WaveFormatExtensible = 1024,
        //
        // Summary:
        //     Don't limit the data rate (to real-time speed) when sending to a Shoutcast
        //     or Icecast server.
        //     With this option you might disable the rate limiting during casting (as it'll
        //     be limited by the playback rate anyway if the source channel is being played).
        UnlimitedCastDataRate = 4096,
        //
        // Summary:
        //     Limit data rate to real-time.
        //     Limit the data rate to real-time speed, by introducing a delay when the rate
        //     is too high. With BASS 2.4.6 or above, this flag is ignored when the encoder
        //     is fed in a playback buffer update cycle (including Un4seen.Bass.Bass.BASS_Update(System.Int32)
        //     and Un4seen.Bass.Bass.BASS_ChannelUpdate(System.Int32,System.Int32) calls),
        //     to avoid possibly causing playback buffer underruns.  Except for in those
        //     instances, this flag is applied automatically when the encoder is feeding
        //     a Shoutcast or Icecast server.
        Limit = 8192,
        
        /// <summary>
        /// Send an AIFF header to the encoder instead of a WAVE header.
        /// </summary>
        AIFF = 16384,
        
        /// <summary>
        /// Free the encoder when the channel is freed.
        /// </summary>
        AutoFree = 262144,
    }

    [Flags]
    public enum ACMFormatFlags
    {
        /// <summary>
        /// Unicode (16-bit characters) option.
        /// </summary>
        Unicode = -2147483648,

        /// <summary>
        /// No ACM.
        /// </summary>
        NoACM = 0,

        /// <summary>
        /// Use the format as default selection.
        /// </summary>
        Default = 1,

        /// <summary>
        /// Only list formats with same sample rate as the source channel.
        /// </summary>
        SameSampleRate = 2,

        /// <summary>
        /// Only list formats with same number of channels (eg. mono/stereo).
        /// </summary>
        SameChannels = 4,

        /// <summary>
        /// Suggest a format (HIWORD=format tag - use one of the WAVEFormatTag flags).
        /// </summary>
        Suggest = 8,
    }

    /// <remarks>See the MMREG.H file for more codec numbers.</remarks>
    public enum WAVEFormatTag
    {
        /// <summary>
        /// Extensible Format (user defined)
        /// </summary>
        Extensible = -2,
        
        /// <summary>
        /// Unknown Format
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// PCM format (8 or 16 bit), Microsoft Corporation
        /// </summary>
        PCM = 1,
        
        /// <summary>
        /// AD PCM Format, Microsoft Corporation
        /// </summary>
        ADPCM = 2,
        
        /// <summary>
        /// IEEE PCM Float format (32 bit)
        /// </summary>
        IEEE_Float = 3,
        
        /// <summary>
        /// AC2, Dolby Laboratories
        /// </summary>
        Dolby_AC2 = 48,
        
        /// <summary>
        /// GSM 6.10, Microsoft Corporation
        /// </summary>
        GSM610 = 49,
        
        /// <summary>
        /// MSN Audio, Microsoft Corporation
        /// </summary>
        MSNAudio = 50,
        
        /// <summary>
        /// MPEG format
        /// </summary>
        MPEG = 80,
        
        /// <summary>
        /// ISO/MPEG Layer3 Format
        /// </summary>
        MPEGLAYER3 = 85,
        
        /// <summary>
        /// AC3 Digital, Sonic Foundry
        /// </summary>
        DOLBY_AC3_SPDIF = 146,
        
        /// <summary>
        /// Raw AAC
        /// </summary>
        RAW_AAC1 = 255,
        
        /// <summary>
        /// Microsoft Corporation
        /// </summary>
        MSAUDIO1 = 352,
        
        /// <summary>
        /// Windows Media Audio. This format is valid for versions 2 through 9
        /// </summary>
        WMA = 353,
        
        /// <summary>
        /// Windows Media Audio 9 Professional
        /// </summary>
        WMA_PRO = 354,
        
        /// <summary>
        /// Windows Media Audio 9 Lossless
        /// </summary>
        WMA_LOSSLESS = 355,
        
        /// <summary>
        /// Windows Media SPDIF Digital Audio
        /// </summary>
        WMA_SPDIF = 356,
        
        /// <summary>
        /// ADTS AAC Audio
        /// </summary>
        MPEG_ADTS_AAC = 5632,
        
        /// <summary>
        /// Raw AAC
        /// </summary>
        MPEG_RAW_AAC = 5633,
        
        /// <summary>
        /// MPEG-4 audio transport stream with a synchronization layer (LOAS) and a multiplex layer (LATM)
        /// </summary>
        MPEG_LOAS = 5634,
        
        /// <summary>
        /// High-Efficiency Advanced Audio Coding (HE-AAC) stream
        /// </summary>
        MPEG_HEAAC = 5648,
    }

    public enum EncodeCount
    {
        In, // sent to encoder
        Out, // received from encoder
        Cast, // sent to cast server
        Queue, // queued
        QueueLimit, // queue limit
        QueueFail // failed to queue
    }

    public static class BassEnc
    {
        const string DllName = "bassenc.dll";

        static IntPtr _castProxy;

        static BassEnc() { BassManager.Load(DllName); }

        #region Configure
        /// <summary>
        /// Encoder DSP priority (default -1000)
        /// priority (int): The priorty determines where in the DSP chain the encoding is performed. 
        /// All DSP with a higher priority will be present in the encoding.
        /// Changes only affect subsequent encodings, not those that have already been started.
        /// The default priority is -1000.
        /// </summary>
        public static int DSPPriority
        {
            get { return Bass.GetConfig(Configuration.EncodePriority); }
            set { Bass.Configure(Configuration.EncodePriority, value); }
        }

        /// <summary>
        /// The maximum queue length (default 10000, 0=no limit)
        /// limit (int): The async encoder queue size limit in milliseconds; 0=unlimited.
        /// When queued encoding is enabled, the queue's buffer will grow as needed to hold the queued data,
        /// up to a limit specified by this config option.  
        /// The default limit is 10 seconds (10000 milliseconds). 
        /// Changes only apply to new encoders, not any already existing encoders.
        /// </summary>
        public static int Queue
        {
            get { return Bass.GetConfig(Configuration.EncodeQueue); }
            set { Bass.Configure(Configuration.EncodeQueue, value); }
        }

        /// <summary>
        /// The time to wait to send data to a cast server (default 5000ms)
        /// timeout (int): The time to wait, in milliseconds.
        /// When an attempt to send data is timed-out, the data is discarded. 
        /// BassEnc.EncodeSetNotify() can be used to receive a notification of when this happens.
        /// The default timeout is 5 seconds (5000 milliseconds).
        /// Changes take immediate effect.
        /// </summary>
        public static int CastTimeout
        {
            get { return Bass.GetConfig(Configuration.EncodeCastTimeout); }
            set { Bass.Configure(Configuration.EncodeCastTimeout, value); }
        }

        /// <summary>
        /// Proxy server settings when connecting to Icecast and Shoutcast
        /// (in the form of "[user:pass@]server:port"... null = don't use a proxy but a direct connection).
        /// proxy (string pointer): The proxy server settings, in the form of "[user:pass@]server:port"...
        /// null = don't use a proxy but make a direct connection (default). 
        /// If only the "server:port" part is specified, then that proxy server is used without any authorization credentials.
        /// BASSenc does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
        /// This also means that the proxy settings can subsequently be changed at that location without having to call this function again.
        /// This setting affects how the following functions connect to servers: Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastInit(System.Int32,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.Int32,System.Boolean),
        /// Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastGetStats(System.Int32,Un4seen.Bass.AddOn.Enc.BASSEncodeStats,System.String),
        /// Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastSetTitle(System.Int32,System.String,System.String).
        /// When a proxy server is used, it needs to support the HTTP 'CONNECT' method.
        /// The default setting is NULL (do not use a proxy).
        /// Changes take effect from the next internet stream creation call. 
        /// By default, BASSenc will not use any proxy settings when connecting to Icecast and Shoutcast.
        /// </summary>
        public static string CastProxy
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeCastProxy)); }
            set
            {
                if (_castProxy != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_castProxy);

                    _castProxy = IntPtr.Zero;
                }

                _castProxy = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeCastProxy, _castProxy);
            }
        }

        /// <summary>
        /// ACM codec name to give priority for the formats it supports.
        /// codec (string pointer): The ACM codec name to give priority (e.g. 'l3codecp.acm').
        /// BASSenc does make a copy of the config string, so it can be freed right after calling it.
        /// </summary>
        public static string ACMLoad
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeACMLoad)); }
            set
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeACMLoad, ptr);

                Marshal.FreeHGlobal(ptr);
            }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int handle, string id, IntPtr buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetACMFormat")]
        public static extern int GetACMFormat(int handle, IntPtr form, int formlen, string title, ACMFormatFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetChannel")]
        public static extern int EncodeGetChannel(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetCount")]
        public static extern long EncodeGetCount(int handle, EncodeCount count);

        [DllImport(DllName, EntryPoint = "BASS_Encode_IsActive")]
        public static extern PlaybackState EncodeIsActive(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetChannel")]
        public static extern bool EncodeSetChannel(int handle, int channel);

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartACMFile")]
        public static extern int StartACMFile(int handle, IntPtr form, EncodeFlags flags, string filename);

        //public static void Doer()
        //{
        //    var x = new ReverseDecoder(new FileDecoder(@"E:\My Music\English\Akon\Keep Up.mp3", BufferKind.Float),
        //                               BufferKind.Float);
        //    BassEnc.Do(x.Handle, @"E:\My Music\English\Akon\Keep Up Rev.mp3");

        //    float[] y;
        //    while (x.HasData) y = x.ReadFloat((int)x.Seconds2Bytes(2));

        //    Console.ReadKey();
        //}

        //public static void Do(int Channel, string Output)
        //{
        //    int SuggestedFormatLength = GetACMFormat(0, IntPtr.Zero, 0, "", 0);

        //    IntPtr form = Marshal.AllocHGlobal(SuggestedFormatLength);

        //    if (GetACMFormat(Channel, form, SuggestedFormatLength, "", 0) > 0)
        //        StartACMFile(Channel, form, 0, Output);

        //    Marshal.FreeHGlobal(form);
        //}
    }
}