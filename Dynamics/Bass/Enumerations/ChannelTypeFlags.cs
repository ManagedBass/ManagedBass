using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum ChannelTypeFlags
    {
        /// <summary>
        /// Unknown channel format.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Sample channel. (HCHANNEL)
        /// </summary>
        Sample = 1,

        /// <summary>
        /// Recording channel. (HRECORD)
        /// </summary>
        Recording = 2,

        /// <summary>
        /// MO3 format music.
        /// </summary>
        MO3 = 256,

        /// <summary>
        /// User sample stream. 
        /// This can also be used as a flag to test if the channel is any kind of HSTREAM.
        /// </summary>
        Stream = 65536,

        /// <summary>
        /// OGG format stream.
        /// </summary>
        OGG = 65538,

        /// <summary>
        /// MP1 format stream.
        /// </summary>
        MP1 = 65539,

        /// <summary>
        /// MP2 format stream.
        /// </summary>
        MP2 = 65540,

        /// <summary>
        /// MP3 format stream.
        /// </summary>
        MP3 = 65541,

        /// <summary>
        /// WAV format stream.
        /// </summary>
        AIFF = 65542,

        /// <summary>
        /// CoreAudio codec stream. Additional information is avaliable via the Un4seen.Bass.BASS_TAG_CACODEC tag.
        /// </summary>
        CA = 65543,

        /// <summary>
        /// Media Foundation codec stream. Additional information is avaliable via the Un4seen.Bass.BASSTag.BASS_TAG_MF tag.
        /// </summary>
        MF = 65544,

        /// <summary>
        /// Audio-CD, CDA
        /// </summary>
        CD = 66048,

        /// <summary>
        /// WMA format stream.
        /// </summary>
        WMA = 66304,

        /// <summary>
        /// MP3 over WMA format stream.
        /// </summary>
        WMA_MP3 = 66305,

        /// <summary>
        /// Winamp input format stream.
        /// </summary>
        WINAMP = 66560,

        /// <summary>
        /// WavPack Lossless format stream.
        /// </summary>
        WV = 66816,

        /// <summary>
        /// WavPack Hybrid Lossless format stream.
        /// </summary>
        WV_H = 66817,

        /// <summary>
        /// WavPack Lossy format stream.
        /// </summary>
        WV_L = 66818,

        /// <summary>
        /// WavPack Hybrid Lossy format stream.
        /// </summary>
        WV_LH = 66819,

        /// <summary>
        /// Optimfrog format stream.
        /// </summary>
        OFR = 67072,

        /// <summary>
        /// APE format stream.
        /// </summary>
        APE = 67328,

        /// <summary>
        /// BASSmix mixer stream.
        /// </summary>
        Mixer = 67584,

        /// <summary>
        /// BASSmix splitter stream.
        /// </summary>
        Split = 67585,

        /// <summary>
        /// FLAC format stream.
        /// </summary>
        FLAC = 67840,

        /// <summary>
        /// FLAC OGG format stream.
        /// </summary>
        FLAC_OGG = 67841,

        /// <summary>
        /// MPC format stream.
        /// </summary>
        MPC = 68096,

        /// <summary>
        /// AAC format stream.
        /// </summary>
        AAC = 68352,

        /// <summary>
        /// MP4 format stream.
        /// </summary>
        MP4 = 68353,

        /// <summary>
        /// Speex format stream.
        /// </summary>
        SPX = 68608,

        /// <summary>
        /// MIDI sound format stream.
        /// </summary>
        MIDI = 68864,
        //
        // Summary:
        //     Apple Lossless (ALAC) format stream.
        ALAC = 69120,
        //
        // Summary:
        //     TTA format stream.
        TTA = 69376,
        //
        // Summary:
        //     AC3 format stream.
        AC3 = 69632,
        //
        // Summary:
        //     Video format stream.
        Video = 69888,
        //
        // Summary:
        //     Opus format stream.
        OPUS = 70144,
        //
        // Summary:
        //     Direct Stream Digital (DSD) format stream.
        DSD = 71424,
        //
        // Summary:
        //     ADX format stream.
        //     ADX is a lossy proprietary audio storage and compression format developed
        //     by CRI Middleware specifically for use in video games, it is derived from
        //     ADPCM.
        ADX = 126976,
        //
        // Summary:
        //     AIX format stream.
        //     Only for ADX of all versions (with AIXP support).
        AIX = 126977,
        //
        // Summary:
        //     BASS_FX tempo stream.
        Tempo = 127488,
        //
        // Summary:
        //     BASS_FX reverse stream.
        Reverse = 127489,
        //
        // Summary:
        //     MOD format music. This can also be used as a flag to test if the channel
        //     is any kind of HMUSIC.
        MOD = 131072,
        //
        // Summary:
        //     MTM format music.
        MTM = 131073,
        //
        // Summary:
        //     S3M format music.
        S3M = 131074,
        //
        // Summary:
        //     XM format music.
        XM = 131075,
        //
        // Summary:
        //     IT format music.
        IT = 131076,
        //
        // Summary:
        //     WAV format stream, LOWORD=codec.
        WAV = 262144,
        //
        // Summary:
        //     WAV format stream, PCM 16-bit.
        WAV_PCM = 327681,
        //
        // Summary:
        //     WAV format stream, FLOAT 32-bit.
        WAV_FLOAT = 327683,
    }
}