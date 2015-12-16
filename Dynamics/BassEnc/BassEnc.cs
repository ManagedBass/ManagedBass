using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassEnc
    {
        const string DllName = "bassenc.dll";

        static IntPtr _castProxy;

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

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
        /// The maximum queue length (default 10000, 0=no limit)
        /// limit (int): The async encoder queue size limit in milliseconds; 0=unlimited.
        /// When queued encoding is enabled, the queue's buffer will grow as needed to hold the queued data,
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
        /// (in the form of "[user:pass@]server:port"... null = don't use a proxy but a direct connection).
        /// proxy (string pointer): The proxy server settings, in the form of "[user:pass@]server:port"...
        /// null = don't use a proxy but make a direct connection (default). 
        /// If only the "server:port" part is specified, then that proxy server is used without any authorization credentials.
        /// BASSenc does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
        /// This also means that the proxy settings can subsequently be changed at that location without having to call this function again.
        /// This setting affects how the following functions connect to servers: Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastInit(System.Int32,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.Int32,System.Boolean),
        /// Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastGetStats(System.Int32,Un4seen.Bass.AddOn.Enc.BASSEncodeStats,System.String),
        /// Un4seen.Bass.AddOn.Enc.BassEnc.BASS_Encode_CastSetTitle(System.Int32,System.String,System.String).
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

        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int handle, string id, IntPtr buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetACMFormat")]
        public static extern int GetACMFormat(int handle, IntPtr form, int formlen, string title, ACMFormatFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetChannel")]
        public static extern int EncodeGetChannel(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_GetCount")]
        public static extern long EncodeGetCount(int handle, EncodeCount count);

        [DllImport(DllName, EntryPoint = "BASS_Encode_IsActive")]
        public static extern PlaybackState EncodeIsActive(int handle);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetChannel")]
        public static extern bool EncodeSetChannel(int handle, int channel);

        [DllImport(DllName, EntryPoint = "BASS_Encode_StartACMFile")]
        public static extern int StartACMFile(int handle, IntPtr form, EncodeFlags flags, string filename);

        //public static void Doer()
        //{
        //    var x = new ReverseDecoder(new FileDecoder(@"E:\My Music\English\Akon\Keep Up.mp3", BufferKind.Float),
        //                               BufferKind.Float);
        //    BassEnc.Do(x.Handle, @"E:\My Music\English\Akon\Keep Up Rev.mp3");

        //    float[] y;
        //    while (x.HasData) y = x.ReadFloat((int)x.Seconds2Bytes(2));

        //    Console.ReadKey();
        //}

        //public static void Do(int Channel, string Output)
        //{
        //    int SuggestedFormatLength = GetACMFormat(0, IntPtr.Zero, 0, "", 0);

        //    IntPtr form = Marshal.AllocHGlobal(SuggestedFormatLength);

        //    if (GetACMFormat(Channel, form, SuggestedFormatLength, "", 0) > 0)
        //        StartACMFile(Channel, form, 0, Output);

        //    Marshal.FreeHGlobal(form);
        //}
    }
}