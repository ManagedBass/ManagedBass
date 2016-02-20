using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassEnc: bassenc.dll
    /// </summary>
    public static class BassEnc
    {
        const string DllName = "bassenc";
        static IntPtr _castProxy, hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        [DllImport(DllName)]
        static extern int BASS_Encode_GetVersion();

        public static Version Version => Extensions.GetVersion(BASS_Encode_GetVersion());

        #region Configure
        /// <summary>
        /// Encoder DSP priority (default -1000)
        /// priority (int): The priorty determines where in the DSP chain the encoding is performed. 
        /// All DSP with a higher priority will be present in the encoding.
        /// Changes only affect subsequent encodings, not those that have already been started.
        /// The default priority is -1000.
        /// </summary>
        public static int DSPPriority
        {
            get { return Bass.GetConfig(Configuration.EncodePriority); }
            set { Bass.Configure(Configuration.EncodePriority, value); }
        }

        /// <summary>
        /// The maximum queue Length (default 10000, 0=no limit)
        /// limit (int): The async encoder queue size limit in milliseconds; 0=unlimited.
        /// When queued encoding is enabled, the queue's Buffer will grow as needed to hold the queued data,
        /// up to a limit specified by this config option.  
        /// The default limit is 10 seconds (10000 milliseconds). 
        /// Changes only apply to new encoders, not any already existing encoders.
        /// </summary>
        public static int Queue
        {
            get { return Bass.GetConfig(Configuration.EncodeQueue); }
            set { Bass.Configure(Configuration.EncodeQueue, value); }
        }

        /// <summary>
        /// The time to wait to send data to a cast server (default 5000ms)
        /// timeout (int): The time to wait, in milliseconds.
        /// When an attempt to send data is timed-out, the data is discarded. 
        /// BassEnc.EncodeSetNotify() can be used to receive a notification of when this happens.
        /// The default timeout is 5 seconds (5000 milliseconds).
        /// Changes take immediate effect.
        /// </summary>
        public static int CastTimeout
        {
            get { return Bass.GetConfig(Configuration.EncodeCastTimeout); }
            set { Bass.Configure(Configuration.EncodeCastTimeout, value); }
        }

        /// <summary>
        /// Proxy server settings when connecting to Icecast and Shoutcast
        /// (in the form of "[User:pass@]server:port"... null = don't use a proxy but a direct connection).
        /// proxy (string pointer): The proxy server settings, in the form of "[User:pass@]server:port"...
        /// null = don't use a proxy but make a direct connection (default). 
        /// If only the "server:port" part is specified, then that proxy server is used without any authorization credentials.
        /// BASSenc does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
        /// This also means that the proxy settings can subsequently be changed at that location without having to call this function again.
        /// This setting affects how the following functions connect to servers: BassEnc.EncodeCastInit(),
        /// BassEnc.EncodeCastGetStats(), BassEnc.EncodeCastSetTitle().
        /// When a proxy server is used, it needs to support the HTTP 'CONNECT' method.
        /// The default setting is NULL (do not use a proxy).
        /// Changes take effect from the next internet stream creation call. 
        /// By default, BASSenc will not use any proxy settings when connecting to Icecast and Shoutcast.
        /// </summary>
        public static string CastProxy
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeCastProxy)); }
            set
            {
                if (_castProxy != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_castProxy);

                    _castProxy = IntPtr.Zero;
                }

                _castProxy = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeCastProxy, _castProxy);
            }
        }

        /// <summary>
        /// ACM codec name to give priority for the formats it supports.
        /// codec (string pointer): The ACM codec name to give priority (e.g. 'l3codecp.acm').
        /// BASSenc does make a copy of the config string, so it can be freed right after calling it.
        /// </summary>
        public static string ACMLoad
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeACMLoad)); }
            set
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeACMLoad, ptr);

                Marshal.FreeHGlobal(ptr);
            }
        }
        #endregion

        #region Encoding
        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int handle, string id, IntPtr buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int handle, string id, byte[] buffer, int length);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_GetACMFormat(int handle, IntPtr form, int formlen, string title, int flags);

        public static int GetACMFormat(int handle,
                                       IntPtr form = default(IntPtr),
                                       int formlen = 0,
                                       string title = null,
                                       ACMFormatFlags flags = ACMFormatFlags.Default,
                                       WaveFormatTag encoding = WaveFormatTag.Unknown)
        {
            int ACMflags = BitHelper.MakeLong((short)(flags | ACMFormatFlags.Unicode), (short)encoding);

            return BASS_Encode_GetACMFormat(handle, form, formlen, title, ACMflags);
        }

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetChannel")]
        public static extern int EncodeGetChannel(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetCount")]
        public static extern long EncodeGetCount(int handle, EncodeCount count);

        [DllImport(DllName, EntryPoint = "BASS_Encode_IsActive")]
        public static extern PlaybackState EncodeIsActive(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetChannel")]
        public static extern bool EncodeSetChannel(int handle, int channel);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetNotify")]
        public static extern bool EncodeSetNotify(int handle, EncodeNotifyProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetPaused")]
        public static extern bool EncodeSetPaused(int handle, bool paused);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_Start(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user);

        public static int EncodeStart(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user = default(IntPtr))
        {
            return BASS_Encode_Start(handle, cmdline, flags | EncodeFlags.Unicode, proc, user);
        }

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartACM")]
        public static extern int EncodeStartACM(int handle, IntPtr form, EncodeFlags flags, EncodeProcedure proc, IntPtr user);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartACMFile(int handle, IntPtr form, EncodeFlags flags, string filename);

        public static int EncodeStartACM(int handle, IntPtr form, EncodeFlags flags, string filename)
        {
            return BASS_Encode_StartACMFile(handle, form, flags | EncodeFlags.Unicode, filename);
        }

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartCA")]
        public static extern int EncodeStartCA(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, EncodeProcedureEx proc, IntPtr user);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartCAFile(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, string filename);

        public static int EncodeStartCA(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, string filename)
        {
            return BASS_Encode_StartCAFile(handle, ftype, atype, flags | EncodeFlags.Unicode, bitrate, filename);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartLimit(int handle, string cmdline, BassFlags flags, EncodeProcedure proc, IntPtr user, int limit);

        public static int EncodeStart(int handle, string cmdline, BassFlags flags, EncodeProcedure proc, IntPtr user, int limit)
        {
            return BASS_Encode_StartLimit(handle, cmdline, flags | BassFlags.Unicode, proc, user, limit);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartUser(int handle, string filename, BassFlags flags, EncoderProcedure proc, IntPtr user);

        public static int EncodeStart(int handle, string filename, BassFlags flags, EncoderProcedure proc, IntPtr user = default(IntPtr))
        {
            return BASS_Encode_StartUser(handle, filename, flags | BassFlags.Unicode, proc, user);
        }

        [DllImport(DllName, EntryPoint = "BASS_Encode_Stop")]
        public static extern bool EncodeStop(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_StopEx")]
        public static extern bool EncodeStop(int handle, bool queue);

        #region Encode Write
        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, IntPtr buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, byte[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, short[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, int[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, float[] buffer, int length);
        #endregion
        #endregion

        #region Casting
        [DllImport(DllName, EntryPoint = "BASS_Encode_CastGetStats")]
        public static extern string CastGetStats(int handle, int type, string pass);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastInit")]
        public static extern bool CastInit(int handle,
            string server,
            string pass,
            string content,
            string name,
            string url,
            string genre,
            string desc,
            string headers,
            int bitrate,
            bool pub);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastSendMeta")]
        public static extern bool CastSendMeta(int handle, EncodeMetaDataType type, IntPtr data, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastSetTitle")]
        public static extern bool CastSetTitle(int handle, string title, string url);
        #endregion

        #region Server
        [DllImport(DllName, EntryPoint = "BASS_Encode_ServerInit")]
        public static extern int ServerInit(int handle, string port, int buffer, int burst, EncodeServer flags, EncodeClientProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_Encode_ServerKick")]
        public static extern int ServerKick(int handle, string client);
        #endregion
    }
}