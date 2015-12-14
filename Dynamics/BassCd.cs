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

    [StructLayout(LayoutKind.Explicit)]
    public struct TOCTrack
    {
        [FieldOffset(0)]
        byte res1;
        [FieldOffset(1)]
        byte adrcon;
        [FieldOffset(2)]
        byte track;
        [FieldOffset(3)]
        byte res2;
        [FieldOffset(4)]
        int lba;
        [FieldOffset(4), MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] hmsf;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOC
    {
        short size;
        byte first;
        byte last;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        TOCTrack[] tracks;
    }

    public delegate void CDDataProcedure(int handle, int pos, int type, IntPtr buffer, int length, IntPtr user);

    public static class BassCd
    {
        const string DllName = "basscd.dll";
        static IntPtr _cddbServer;

        static BassCd() { BassManager.Load(DllName); }

        public static int DriveCount
        {
            get
            {
                int Count = 0;
                CDInfo info = new CDInfo();

                for (int i = 0; GetDriveInfo(i, ref info); i++) ++Count;

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

        /// <summary>
        /// The server to use in CDDB requests.
        /// server (string): The CDDB server address, in the form of "user:pass@server:port/path".
        /// The "user:pass@", ":port" and "/path" parts are optional; only the "server" part is required.
        /// If not provided, the port and path default to 80 and "/~cddb/cddb.cgi", respectively.
        /// A copy is made of the provided server string, so it need not persist beyond the Bass.Configure(IntPtr) call.
        /// The default setting is "freedb.freedb.org". 
        /// The proxy server, as configured via the Bass.NetProxy option, is used when connecting to the CDDB server.
        /// </summary>
        public static string CDDBServer
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.CDDBServer)); }
            set
            {
                if (_cddbServer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_cddbServer);
                    _cddbServer = IntPtr.Zero;
                }

                _cddbServer = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.CDDBServer, _cddbServer);
            }
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
        public static extern bool GetDriveInfo(int Drive, ref CDInfo Info);

        public static CDInfo GetDriveInfo(int Drive)
        {
            CDInfo temp = new CDInfo();
            GetDriveInfo(Drive, ref temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreate")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateEx")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateFile")]
        public static extern int CreateStream(string File, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateFileEx")]
        public static extern int CreateStream(string File, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamGetTrack")]
        public static extern int GetStreamTrack(int handle);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamSetTrack")]
        public static extern bool SetStreamTrack(int handle, int track);

        [DllImport(DllName, EntryPoint = "BASS_CD_Door")]
        public static extern bool Door(int drive, CDDoorAction action);

        [DllImport(DllName, EntryPoint = "BASS_CD_DoorIsLocked")]
        public static extern bool DoorIsLocked(int drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_DoorIsOpen")]
        public static extern bool DoorIsOpen(int drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_SetInterface")]
        public static extern int SetInterface(int iface);

        [DllImport(DllName, EntryPoint = "BASS_CD_SetOffset")]
        public static extern bool SetOffset(int Drive, int offset);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetID")]
        public static extern string GetID(int Drive, int id);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetTOC")]
        public static extern bool GetTOC(int Drive, int mode, out TOC toc);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetTracks")]
        public static extern int GetTracks(int Drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetTrackLength")]
        public static extern int GetTrackLength(int Drive, int Track);

        [DllImport(DllName, EntryPoint = "BASS_CD_GetTrackPregap")]
        public static extern int GetTrackPregap(int Drive, int Track);

        [DllImport(DllName, EntryPoint = "BASS_CD_Analog_GetPosition")]
        public static extern int AnalogGetPosition(int drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_Analog_IsActive")]
        public static extern bool AnalogIsActive(int drive);

        [DllImport(DllName, EntryPoint = "BASS_CD_Analog_Play")]
        public static extern bool AnalogPlay(int drive, int track, int pos);

        [DllImport(DllName, EntryPoint = "BASS_CD_Analog_PlayFile")]
        public static extern bool AnalogPlay(string file, int pos);

        [DllImport(DllName, EntryPoint = "BASS_CD_Analog_Stop")]
        public static extern bool AnalogStop(int drive);
    }
}