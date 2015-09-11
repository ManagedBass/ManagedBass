using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum BASSEncode
    {
        // Summary:
        //     Cmdline is Unicode (16-bit characters).
        BASS_UNICODE = -2147483648,
        //
        // Summary:
        //     Default option, incl. wave header, little-endian and no FP conversion.
        BASS_ENCODE_DEFAULT = 0,
        //
        // Summary:
        //     Do NOT send a WAV header to the encoder.
        BASS_ENCODE_NOHEAD = 1,
        //
        // Summary:
        //     Convert floating-point sample data to 8-bit integer.
        BASS_ENCODE_FP_8BIT = 2,
        //
        // Summary:
        //     Convert floating-point sample data to 16-bit integer.
        BASS_ENCODE_FP_16BIT = 4,
        //
        // Summary:
        //     Convert floating-point sample data to 24-bit integer.
        BASS_ENCODE_FP_24BIT = 6,
        //
        // Summary:
        //     Convert floating-point sample data to 32-bit integer.
        BASS_ENCODE_FP_32BIT = 8,
        //
        // Summary:
        //     Big-Endian sample data.
        BASS_ENCODE_BIGEND = 16,
        //
        // Summary:
        //     Start the encoder paused.
        BASS_ENCODE_PAUSE = 32,
        //
        // Summary:
        //     Write PCM sample data (no encoder).
        BASS_ENCODE_PCM = 64,
        //
        // Summary:
        //     Write RF64 WAV header (no encoder).
        BASS_ENCODE_RF64 = 128,
        //
        // Summary:
        //     Convert to mono (if not already).
        BASS_ENCODE_MONO = 256,
        //
        // Summary:
        //     Queue data to feed encoder asynchronously.
        //     The queue buffer will grow as needed to fit the data, but its size can be
        //     limited by the Un4seen.Bass.BASSConfig.BASS_CONFIG_ENCODE_QUEUE config option
        //     (0 = no limit); the default is 10000ms.  If the queue reaches the size limit
        //     and data is lost, the Un4seen.Bass.AddOn.Enc.BASSEncodeNotify.BASS_ENCODE_NOTIFY_QUEUE_FULL
        //     notification will be triggered.
        BASS_ENCODE_QUEUE = 512,
        //
        // Summary:
        //     Send the sample format information to the encoder in WAVEFORMATEXTENSIBLE
        //     form instead of WAVEFORMATEX form.
        //     This flag is ignored if the BASS_ENCODE_NOHEAD flag is used.
        BASS_ENCODE_WFEXT = 1024,
        //
        // Summary:
        //     Don't limit the data rate (to real-time speed) when sending to a Shoutcast
        //     or Icecast server.
        //     With this option you might disable the rate limiting during casting (as it'll
        //     be limited by the playback rate anyway if the source channel is being played).
        BASS_ENCODE_CAST_NOLIMIT = 4096,
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
        BASS_ENCODE_LIMIT = 8192,
        //
        // Summary:
        //     Send an AIFF header to the encoder instead of a WAVE header.
        BASS_ENCODE_AIFF = 16384,
        //
        // Summary:
        //     Free the encoder when the channel is freed.
        BASS_ENCODE_AUTOFREE = 262144,
    }

    [Flags]
    public enum BASSACMFormat
    {
        // Summary:
        //     Unicode (16-bit characters) option.
        BASS_UNICODE = -2147483648,
        //
        // Summary:
        //     No ACM.
        BASS_ACM_NONE = 0,
        //
        // Summary:
        //     Use the format as default selection.
        BASS_ACM_DEFAULT = 1,
        //
        // Summary:
        //     Only list formats with same sample rate as the source channel.
        BASS_ACM_RATE = 2,
        //
        // Summary:
        //     Only list formats with same number of channels (eg. mono/stereo).
        BASS_ACM_CHANS = 4,
        //
        // Summary:
        //     Suggest a format (HIWORD=format tag - use one of the Un4seen.Bass.WAVEFormatTag
        //     flags).
        BASS_ACM_SUGGEST = 8,
    }

    // See the MMREG.H file for more codec numbers.
    public enum WAVEFormatTag
    {
        // Summary:
        //     Extensible Format (user defined)
        EXTENSIBLE = -2,
        //
        // Summary:
        //     Unknown Format
        UNKNOWN = 0,
        //
        // Summary:
        //     PCM format (8 or 16 bit), Microsoft Corporation
        PCM = 1,
        //
        // Summary:
        //     AD PCM Format, Microsoft Corporation
        ADPCM = 2,
        //
        // Summary:
        //     IEEE PCM Float format (32 bit)
        IEEE_FLOAT = 3,
        //
        // Summary:
        //     AC2, Dolby Laboratories
        DOLBY_AC2 = 48,
        //
        // Summary:
        //     GSM 6.10, Microsoft Corporation
        GSM610 = 49,
        //
        // Summary:
        //     MSN Audio, Microsoft Corporation
        MSNAUDIO = 50,
        //
        // Summary:
        //     MPEG format
        MPEG = 80,
        //
        // Summary:
        //     ISO/MPEG Layer3 Format
        MPEGLAYER3 = 85,
        //
        // Summary:
        //     AC3 Digital, Sonic Foundry
        DOLBY_AC3_SPDIF = 146,
        //
        // Summary:
        //     Raw AAC
        RAW_AAC1 = 255,
        //
        // Summary:
        //     Microsoft Corporation
        MSAUDIO1 = 352,
        //
        // Summary:
        //     Windows Media Audio. This format is valid for versions 2 through 9
        WMA = 353,
        //
        // Summary:
        //     Windows Media Audio 9 Professional
        WMA_PRO = 354,
        //
        // Summary:
        //     Windows Media Audio 9 Lossless
        WMA_LOSSLESS = 355,
        //
        // Summary:
        //     Windows Media SPDIF Digital Audio
        WMA_SPDIF = 356,
        //
        // Summary:
        //     ADTS AAC Audio
        MPEG_ADTS_AAC = 5632,
        //
        // Summary:
        //     Raw AAC
        MPEG_RAW_AAC = 5633,
        //
        // Summary:
        //     MPEG-4 audio transport stream with a synchronization layer (LOAS) and a multiplex
        //     layer (LATM)
        MPEG_LOAS = 5634,
        //
        // Summary:
        //     High-Efficiency Advanced Audio Coding (HE-AAC) stream
        MPEG_HEAAC = 5648,
    }

    static class BassEnc
    {
        const string DllName = "bassenc.dll";

        static BassEnc() { BassManager.Load(DllName); }

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetACMFormat")]
        public static extern int GetACMFormat(int handle, IntPtr form, int formlen, string title, int flags);

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartACMFile")]
        public static extern int StartACMFile(int handle, IntPtr form, BASSEncode flags, string filename);
                
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