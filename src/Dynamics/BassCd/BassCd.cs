using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps basscd.dll.
    /// </summary>
    /// <remarks>
    /// <para>Supports: .cda</para>
    /// <para>Not available on OSX</para>
    /// </remarks>
    public static class BassCd
    {
        const string DllName = "basscd";
        static IntPtr _cddbServer, hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        public static int DriveCount
        {
            get
            {
                int i;
                CDInfo info;

                for (i = 0; GetDriveInfo(i, out info); i++) ;

                return i;
            }
        }

        #region Configuration
        /// <summary>
        /// Automatically free an existing stream when creating a new one on the same drive? (enabled by Default)
        /// </summary>
        /// <remarks>
        /// Only one stream can exist at a time per CD drive. 
        /// So if a stream using the same drive already exists, stream creation function calls
        /// will fail, unless this config option is enabled to automatically free the existing stream.
        /// </remarks>
        public static bool FreeOld
        {
            get { return Bass.GetConfigBool(Configuration.CDFreeOld); }
            set { Bass.Configure(Configuration.CDFreeOld, value); }
        }

        /// <summary>
        /// Number of times to retry after a read error.
        /// 0 = don't retry.
        /// default = 2.
        /// </summary>
        public static int RetryCount
        {
            get { return Bass.GetConfig(Configuration.CDRetry); }
            set { Bass.Configure(Configuration.CDRetry, value); }
        }

        /// <summary>
        /// Automatically reduce the read speed when a read error occurs? (default is disabled).
        /// If true, the read speed will be halved when a read error occurs, before retrying (if the <see cref="RetryCount"/> setting allows).
        /// </summary>
        public static bool AutoSpeedReduction
        {
            get { return Bass.GetConfigBool(Configuration.CDAutoSpeed); }
            set { Bass.Configure(Configuration.CDAutoSpeed, value); }
        }

        /// <summary>
        /// Skip past read errors?
        /// If true, reading will skip onto the next frame when a read error occurs, otherwise reading will stop.
        /// When skipping an error, it will be replaced with silence, so that the track Length is unaffected. 
        /// Before skipping past an error, BassCD will first retry according to the <see cref="RetryCount"/> setting.
        /// </summary>
        public static bool SkipError
        {
            get { return Bass.GetConfigBool(Configuration.CDSkipError); }
            set { Bass.Configure(Configuration.CDSkipError, value); }
        }

        /// <summary>
        /// The server to use in CDDB requests.
        /// server (string): The CDDB server address, in the form of "User:pass@server:port/path".
        /// The "User:pass@", ":port" and "/path" parts are optional; only the "server" part is required.
        /// If not provided, the port and path default to 80 and "/~cddb/cddb.cgi", respectively.
        /// The default setting is "freedb.freedb.org". 
        /// The proxy server, as configured via the <see cref="Bass.NetProxy"/> option, is used when connecting to the CDDB server.
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
        #endregion

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
            CDInfo info;
            return GetDriveInfo(Drive, out info) ? info : null;
        }

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreate")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_CD_StreamCreateEx")]
        public static extern int CreateStream(int Drive, int Track, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_CD_StreamCreateFile(string File, BassFlags Flags);

        public static int CreateStream(string File, BassFlags Flags)
        {
            return BASS_CD_StreamCreateFile(File, Flags | BassFlags.Unicode);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_CD_StreamCreateFileEx(string File, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr));

        public static int CreateStream(string File, BassFlags Flags, CDDataProcedure proc, IntPtr user = default(IntPtr))
        {
            return BASS_CD_StreamCreateFileEx(File, Flags | BassFlags.Unicode, proc, user);
        }

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
        public static extern bool GetTOC(int Drive, TOCMode mode, out TOC toc);

        public static TOC GetTOC(int Drive, TOCMode Mode)
        {
            TOC toc;
            return GetTOC(Drive, Mode, out toc) ? toc : null;
        }

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