using System;

namespace ManagedBass
{
    /// <summary>
    /// Channel Type flags to be used with <see cref="ChannelInfo" /> (see also <see cref="Bass.ChannelGetInfo(int,out ChannelInfo)" />).
    /// </summary>
    [Flags]
    public enum ChannelType
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
        /// CoreAudio codec stream. Additional information is avaliable via the <see cref="TagType.CoreAudioCodec"/> tag.
        /// </summary>
        CA = 65543,

        /// <summary>
        /// Media Foundation codec stream. Additional information is avaliable via the <see cref="TagType.MF"/> tag.
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
        /// Winamp Input format stream.
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

        /// <summary>
        /// Apple Lossless (ALAC) format stream.
        /// </summary>
        ALAC = 69120,

        /// <summary>
        /// TTA format stream.
        /// </summary>
        TTA = 69376,

        /// <summary>
        /// AC3 format stream.
        /// </summary>
        AC3 = 69632,

        /// <summary>
        /// Video format stream.
        /// </summary>
        Video = 69888,

        /// <summary>
        /// Opus format stream.
        /// </summary>
        OPUS = 70144,

        /// <summary>
        /// Direct Stream Digital (DSD) format stream.
        /// </summary>
        DSD = 71424,

        /// <summary>
        /// ADX format stream.
        /// <para>
        /// ADX is a lossy proprietary audio storage and compression format developed
        /// by CRI Middleware specifically for use in video games, it is derived from ADPCM.
        /// </para>
        /// </summary>
        ADX = 126976,

        /// <summary>
        /// AIX format stream.
        /// Only for ADX of all versions (with AIXP support).
        /// </summary>
        AIX = 126977,

        /// <summary>
        /// BassFx tempo stream.
        /// </summary>
        Tempo = 127488,

        /// <summary>
        /// BassFx reverse stream.
        /// </summary>
        Reverse = 127489,

        /// <summary>
        /// MOD format music.
        /// This can also be used as a flag to test if the channel is any kind of HMUSIC.
        /// </summary>
        MOD = 131072,

        /// <summary>
        /// MTM format music.
        /// </summary>
        MTM = 131073,

        /// <summary>
        /// S3M format music.
        /// </summary>
        S3M = 131074,

        /// <summary>
        /// XM format music.
        /// </summary>
        XM = 131075,

        /// <summary>
        /// IT format music.
        /// </summary>
        IT = 131076,

        /// <summary>
        /// WAV format stream, LOWORD=codec.
        /// </summary>
        Wave = 262144,

        /// <summary>
        /// WAV format stream, PCM 16-bit.
        /// </summary>
        WavePCM = 327681,

        /// <summary>
        /// WAV format stream, FLOAT 32-bit.
        /// </summary>
        WaveFloat = 327683,
    }
}
