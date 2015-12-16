using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum CDReadFlags
    {
        // Summary:
        //     The drive can read CD-R media.
        CDR = 1,
        //
        // Summary:
        //     The drive can read CD-RW media.
        CDRW = 2,
        //
        // Summary:
        //     The drive can read CD-R/RW media where the addressing type is "method 2".
        CDRW2 = 4,
        //
        // Summary:
        //     The drive can read DVD-ROM media.
        DVD = 8,
        //
        // Summary:
        //     The drive can read DVD-R media.
        DVDR = 16,
        //
        // Summary:
        //     The drive can read DVD-RAM media.
        DVDRAM = 32,
        //
        // Summary:
        //     The drive is capable of analog playback.
        Analog = 65536,
        //
        // Summary:
        //     The drive can read in "mode 2 form 1" format.
        M2F1 = 1048576,
        //
        // Summary:
        //     The drive can read in "mode 2 form 2" format.
        M2F2 = 2097152,
        //
        // Summary:
        //     The drive can read multi-session discs.
        MultiSession = 4194304,
        //
        // Summary:
        //     The drive can read CD audio.
        CDDA = 16777216,
        //
        // Summary:
        //     The drive supports "stream is accurate".
        CDDASIA = 33554432,
        //
        // Summary:
        //     The drive can read sub-channel data.
        SubChannel = 67108864,
        //
        // Summary:
        //     The drive can read sub-channel data, and de-interleave it.
        SubChannelDeInterleave = 134217728,
        //
        // Summary:
        //     The drive can provide C2 error info.
        C2 = 268435456,
        //
        // Summary:
        //     The drive can read ISRC numbers.
        ISRC = 536870912,
        //
        // Summary:
        //     The drive can read UPC numbers.
        UPC = 1073741824,
    }

    public enum CDDoorAction
    {
        Close,
        Open,
        Lock,
        Unlock
    }
}