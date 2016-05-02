using System;

namespace ManagedBass
{
    /// <summary>
    /// Formats flags of <see cref="RecordInfo.SupportedFormats"/> member to be used with <see cref="RecordInfo" />
    /// </summary>
    [Flags]
    public enum RecordFormatFlags
    {
        /// <summary>
        /// Unknown Format
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 11.025 kHz, Mono, 8-bit
        /// </summary>
        WF1M08 = 1,

        /// <summary>
        /// 11.025 kHz, Stereo, 8-bit
        /// </summary>
        WF1S08 = 2,

        /// <summary>
        /// 11.025 kHz, Mono, 16-bit
        /// </summary>
        WF1M16 = 4,

        /// <summary>
        /// 11.025 kHz, Stereo, 16-bit
        /// </summary>
        WF1S16 = 8,

        /// <summary>
        /// 22.05 kHz, Mono, 8-bit
        /// </summary>
        WF2M08 = 16,

        /// <summary>
        /// 22.05 kHz, Stereo, 8-bit
        /// </summary>
        WF2S08 = 32,

        /// <summary>
        /// 22.05 kHz, Mono, 16-bit
        /// </summary>
        WF2M16 = 64,

        /// <summary>
        /// 22.05 kHz, Stereo, 16-bit
        /// </summary>
        WF2S16 = 128,

        /// <summary>
        /// 44.1 kHz, Mono, 8-bit
        /// </summary>
        WF4M08 = 256,

        /// <summary>
        /// 44.1 kHz, Stereo, 8-bit
        /// </summary>
        WF4S08 = 512,

        /// <summary>
        /// 44.1 kHz, Mono, 16-bit
        /// </summary>
        WF4M16 = 1024,

        /// <summary>
        /// 44.1 kHz, Stereo, 16-bit
        /// </summary>
        WF4S16 = 2048,

        /// <summary>
        /// 48 kHz, Mono, 8-bit
        /// </summary>
        WF48M08 = 4096,

        /// <summary>
        /// 48 kHz, Stereo, 8-bit
        /// </summary>
        WF48S08 = 8192,

        /// <summary>
        /// 48 kHz, Mono, 16-bit
        /// </summary>
        WF48M16 = 16384,

        /// <summary>
        /// 48 kHz, Stereo, 16-bit
        /// </summary>
        WF48S16 = 32768,

        /// <summary>
        /// 96 kHz, Mono, 8-bit
        /// </summary>
        WF96M08 = 65536,

        /// <summary>
        /// 96 kHz, Stereo, 8-bit
        /// </summary>
        WF96S08 = 131072,

        /// <summary>
        /// 96 kHz, Mono, 16-bit
        /// </summary>
        WF96M16 = 262144,

        /// <summary>
        /// 96 kHz, Stereo, 16-bit
        /// </summary>
        WF96S16 = 524288
    }
}
