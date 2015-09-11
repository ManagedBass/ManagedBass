using System;
using System.Runtime.InteropServices;

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
        ANALOG = 65536,
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
        MULTI = 4194304,
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
        SUBCHAN = 67108864,
        //
        // Summary:
        //     The drive can read sub-channel data, and de-interleave it.
        SUBCHANDI = 134217728,
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

    [StructLayout(LayoutKind.Sequential)]
    public struct CDInfo
    {
        IntPtr vendor;
        IntPtr product;
        public IntPtr rev;
        public int letter;
        public CDReadFlags rwflags;
        public bool canopen;
        public bool canlock;
        public int maxspeed;
        public int cache;
        public bool cdtext;

        public string Name { get { return Marshal.PtrToStringAnsi(product); } }

        public string Manufacturer { get { return Marshal.PtrToStringAnsi(vendor); } }

        public int SpeedMultiplier { get { return (int)(maxspeed / 176.4); } }

        public char DriveLetter { get { return char.ToUpper((char)(letter + 63)); } }
    }

    public static class BassCd
    {
        const string DllName = "basscd.dll";

        static BassCd() { BassManager.Load(DllName); }

        public static int DriveCount
        {
            get
            {
                int Count = 0;
                CDInfo info = new CDInfo();

                for (int i = 0; DriveInfo(i, ref info); i++) ++Count;

                return Count;
            }
        }

        /// <summary>
        /// Automatically free an existing stream when creating a new
        /// one on the same drive?
        /// </summary>
        /// <remarks>
        /// Only one stream can exist at a time per CD drive. 
        /// So if a stream using the same drive already exists, stream creation function calls
        /// will fail, unless this config option is enabled to automatically free the
        /// existing stream.
        /// This is enabled by default.
        /// </remarks>
        public static bool FreeOld
        {
            get { return Bass.GetConfigBool(Configuration.CDFreeOld); }
            set { Bass.Configure(Configuration.CDFreeOld, value); }
        }

        /// <summary>
        /// Number of times to retry after a read error.
        /// 0 = don't retry.
        /// The default is 2 retries.
        /// </summary>
        public static int RetryCount
        {
            get { return Bass.GetConfig(Configuration.CDRetry); }
            set { Bass.Configure(Configuration.CDRetry, value); }
        }

        /// <summary>
        /// Automatically reduce the read speed when a read error occurs?
        /// By default, this option is disabled.
        /// If true, the read speed will be halved when a read error occurs, before retrying
        /// (if the RetryCount Setting allows).
        /// </summary>
        public static bool AutoSpeedReduction
        {
            get { return Bass.GetConfigBool(Configuration.CDAutoSpeed); }
            set { Bass.Configure(Configuration.CDAutoSpeed, value); }
        }

        /// <summary>
        /// Skip past read errors?
        /// If true, reading will skip onto the next frame when a read error
        /// occurs, otherwise reading will stop.
        /// When skipping an error, it will be replaced with silence, so that the track
        /// length is unaffected. Before skipping past an error, BassCD will first retry
        /// according to the RetryCount setting.
        /// </summary>
        public static bool SkipError
        {
            get { return Bass.GetConfigBool(Configuration.CDSkipError); }
            set { Bass.Configure(Configuration.CDSkipError, value); }
        }

        [DllImport(DllName, EntryPoint = "BASS_CD_Release")]
        public static extern bool Release(int Drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetSpeed")]
        public static extern int GetSpeed(int Drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_SetSpeed")]
        public static extern bool SetSpeed(int Drive, int Speed);

        [DllImport(DllName, EntryPoint = "BASS_CD_IsReady")]
        public static extern bool IsReady(int Drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetInfo")]
        public static extern bool DriveInfo(int Drive, ref CDInfo Info);

        public static CDInfo DriveInfo(int Drive)
        {
            CDInfo temp = new CDInfo();
            DriveInfo(Drive, ref temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreate")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateFile")]
        public static extern int CreateStream(string File, BassFlags Flags);
    }
}