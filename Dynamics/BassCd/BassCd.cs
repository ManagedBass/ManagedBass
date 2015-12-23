using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassCd
    {
        const string DllName = "basscd.dll";
        static IntPtr _cddbServer;

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        public static int DriveCount
        {
            get
            {
                int Count = 0;
                CDInfo info;

                for (int i = 0; GetDriveInfo(i, out info); i++) ++Count;

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
        public static extern bool GetDriveInfo(int Drive, out CDInfo Info);

        public static CDInfo GetDriveInfo(int Drive)
        {
            CDInfo temp;
            GetDriveInfo(Drive, out temp);
            return temp;
        }

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreate")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateEx")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr));

        // Unicode:
        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateFile")]
        public static extern int CreateStream(string File, BassFlags Flags);

        // Unicode:
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