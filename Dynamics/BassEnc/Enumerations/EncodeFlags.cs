using System;

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
}