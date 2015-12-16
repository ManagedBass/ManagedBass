namespace ManagedBass.Dynamics
{
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
}