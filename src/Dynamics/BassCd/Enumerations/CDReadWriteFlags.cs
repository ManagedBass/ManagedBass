using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// The drive's reading &amp; writing capabilities used with <see cref="BassCd.GetInfo(int,out CDInfo)" />.
    /// </summary>
    [Flags]
    public enum CDReadWriteFlags
    {
        /// <summary>
        /// The drive can read CD-R media.
        /// </summary>
        CDR = 1,
        
        /// <summary>
        /// The drive can read CD-RW media.
        /// </summary>
        CDRW = 2,
        
        /// <summary>
        /// The drive can read CD-R/RW media where the addressing Type is "method 2".
        /// </summary>
        CDRW2 = 4,
        
        /// <summary>
        /// The drive can read DVD-ROM media.
        /// </summary>
        DVD = 8,
        
        /// <summary>
        /// The drive can read DVD-R media.
        /// </summary>
        DVDR = 16,
        
        /// <summary>
        /// The drive can read DVD-RAM media.
        /// </summary>
        DVDRAM = 32,
        
        /// <summary>
        /// The drive is capable of analog playback.
        /// </summary>
        Analog = 65536,
        
        /// <summary>
        /// The drive can read in "mode 2 form 1" format.
        /// </summary>
        M2F1 = 1048576,
        
        /// <summary>
        /// The drive can read in "mode 2 form 2" format.
        /// </summary>
        M2F2 = 2097152,
        
        /// <summary>
        /// The drive can read multi-session discs.
        /// </summary>
        MultiSession = 4194304,
        
        /// <summary>
        /// The drive can read CD audio.
        /// </summary>
        CDDA = 16777216,
        
        /// <summary>
        /// The drive supports "stream is accurate".
        /// </summary>
        CDDASIA = 33554432,
        
        /// <summary>
        /// The drive can read sub-channel data.
        /// </summary>
        SubChannel = 67108864,
        
        /// <summary>
        /// The drive can read sub-channel data, and de-interleave it.
        /// </summary>
        SubChannelDeInterleave = 134217728,
        
        /// <summary>
        /// The drive can provide C2 error info.
        /// </summary>
        C2 = 268435456,
        
        /// <summary>
        /// The drive can read ISRC numbers.
        /// </summary>
        ISRC = 536870912,
        
        /// <summary>
        /// The drive can read UPC numbers.
        /// </summary>
        UPC = 1073741824,
    }
}