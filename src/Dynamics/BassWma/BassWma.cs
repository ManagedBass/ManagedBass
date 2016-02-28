using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps basswma.dll
    /// </summary>
    /// <remarks>
    /// Supports: .wma, .wmv
    /// <para>Not available for Linux and OSX</para>
    /// </remarks>
    public static class BassWma
    {
        const string DllName = "basswma";
        static IntPtr hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        [DllImport(DllName, EntryPoint = "BASS_WMA_GetWMObject")]
        public static extern IntPtr GetWMObject(int handle);

        #region CreateStream
        [DllImport(DllName, EntryPoint="BASS_WMA_StreamCreateFileUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user = default(IntPtr));
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_StreamCreateFile(bool Memory, string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_WMA_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_WMA_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_WMA_StreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        static int CreateStreamObj(object Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags);
        }

        public static int CreateStream(short[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags);
        }

        public static int CreateStream(int[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags);
        }

        public static int CreateStream(float[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }
        #endregion

        #region CreateStream Auth
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_StreamCreateFileAuth(bool mem, IntPtr file, long offset, long length, BassFlags flags, string user, string pass);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_StreamCreateFileAuth(bool mem, string file, long offset, long length, BassFlags flags, string user, string pass);

        public static int CreateStream(string File, long Length, BassFlags Flags, string UserName, string Password)
        {
            return BASS_WMA_StreamCreateFileAuth(false, File, 0, Length, Flags | BassFlags.Unicode, UserName, Password);
        }

        public static int CreateStream(IntPtr Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return BASS_WMA_StreamCreateFileAuth(true, Memory, 0, Length, Flags | BassFlags.Unicode, UserName, Password);
        }

        static int CreateStreamObj(object Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Length, Flags, UserName, Password);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        public static int CreateStream(byte[] Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return CreateStreamObj(Memory, Length, Flags, UserName, Password);
        }

        public static int CreateStream(short[] Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return CreateStreamObj(Memory, Length, Flags, UserName, Password);
        }

        public static int CreateStream(int[] Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return CreateStreamObj(Memory, Length, Flags, UserName, Password);
        }

        public static int CreateStream(float[] Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return CreateStreamObj(Memory, Length, Flags, UserName, Password);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags, string UserName, string Password)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, Length, Flags, UserName, Password);
        }
        #endregion

        #region Encode
        #region Encode Write
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, float[] Buffer, int Length);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpen")]
        public static extern int EncodeOpen(int freq, int chans, WMAEncodeFlags flags, int bitrate, WMEncodeProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenFile")]
        public static extern int EncodeOpenFile(int freq, int chans, WMAEncodeFlags flags, int bitate, string file);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetwork")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, int bitrate, int port, int clients);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetworkMulti")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, int[] bitrate, int port, int clients);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenPublish")]
        public static extern int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int bitrate, string url, string user, string pass);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenPublishMulti")]
        public static extern int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int[] bitrate, string url, string user, string pass);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeSetNotify")]
        public static extern bool EncodeSetNotify(int handle, ClientConnectProcedure proc, IntPtr user);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_WMA_EncodeSetTag(int handle, string tag, string value, int form);

        public static bool EncodeSetTag(int handle, string tag, string value)
        {
            return BASS_WMA_EncodeSetTag(handle, tag, value, 1); // form = 1 (Unicode)
        }

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetClients")]
        public static extern int EncodeGetClients(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetPort")]
        public static extern int EncodeGetPort(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeClose")]
        public static extern bool EncodeClose(int handle);

        [DllImport(DllName)]
        static unsafe extern int* BASS_WMA_EncodeGetRates(int freq, int chans, WMAEncodeFlags Flags);

        public unsafe static int[] EncodeGetRates(int Frequency, int Channels, WMAEncodeFlags Flags)
        {
            var list = new List<int>();

            var rates = BASS_WMA_EncodeGetRates(Frequency, Channels, Flags);

            if (rates != null)
            {
                while (*rates != 0)
                {
                    list.Add(*rates);
                    rates++;
                }
            }

            return list.ToArray();
        }
        #endregion

        #region Configuration
        /// <summary>
        /// Enable network seeking?
        /// seek (bool): If true seeking in network files/streams is enabled (default is false).
        /// If true, it allows seeking before the entire file has been downloaded/cached.
        /// Seeking is slow that way, so it's disabled by default.
        /// </summary>
        public static bool CanSeekNetworkStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetSeek); }
            set { Bass.Configure(Configuration.WmaNetSeek, value); }
        }

        /// <summary>
        /// Play audio from WMV (video) files?
        /// playwmv (bool): If true (default) BASSWMA will play the audio from WMV video files.
        /// If false WMV files will not be played.
        /// </summary>
        public static bool PlayWMVAudio
        {
            get { return Bass.GetConfigBool(Configuration.WmaVideo); }
            set { Bass.Configure(Configuration.WmaVideo, value); }
        }

        /// <summary>
        /// Prebuffer internet streams on creation, before returning from BassWma.StreamCreateFile()?
        /// prebuf (bool): The Windows Media modules must prebuffer a stream before starting decoding/playback of it.
        /// This option determines when/where to wait for that to be completed.
        /// The Windows Media modules must prebuffer a stream before starting decoding/playback of it.
        /// This option determines whether the stream creation function (eg. BassWma.CreateStream())
        /// will wait for the prebuffering to complete before returning.
        /// If playback of a stream is attempted before it has prebuffered,
        /// it will stall and then resume once it has finished prebuffering.
        /// The prebuffering progress can be monitored via Bass.StreamGetFilePosition() (FileStreamPosition.WmaBuffer).
        /// This option is enabled by default.
        /// </summary>
        public static bool PrebufferInternetStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetPreBuffer); }
            set { Bass.Configure(Configuration.WmaNetPreBuffer, value); }
        }

        /// <summary>
        /// Use BASS file handling.
        /// bassfile (bool): Default is disabled (false).
        /// When enabled (true) BASSWMA uses BASS's file routines when playing local files.
        /// It uses the IStream interface to do that.
        /// This would also allow to support the "offset" parameter for WMA files with Bass.CreateStream().
        /// The downside of enabling this feature is, that it stops playback while encoding from working.
        /// </summary>
        public static bool UseBassFileHandling
        {
            get { return Bass.GetConfigBool(Configuration.WmaBassFileHandling); }
            set { Bass.Configure(Configuration.WmaBassFileHandling, value); }
        }

        /// <summary>
        /// Use a seperate thread to decode the data?
        /// async (bool): If true BASSWMA will decode the data in a seperate thread.
        /// If false (default) the normal file system will be used.
        /// The WM decoder can by synchronous (decodes data on demand) or asynchronous (decodes in the background).
        /// With the background decoding, BASSWMA buffers the data that it receives from the decoder for the STREAMPROC to access.
        /// The start of playback/seeking may well be slightly delayed due to there being no data available immediately.
        /// Internet streams are only supported by the asynchronous system,
        /// but local files can use either, and BASSWMA uses the synchronous system by default.
        /// </summary>
        public static bool AsyncDecoding
        {
            get { return Bass.GetConfigBool(Configuration.WmaAsync); }
            set { Bass.Configure(Configuration.WmaAsync, value); }
        }
        #endregion

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern IntPtr BASS_WMA_GetTags(string File, BassFlags Flags);

        public static string[] GetTags(string File, BassFlags Flags) => Extensions.ExtractMultiStringUtf8(BASS_WMA_GetTags(File, Flags | BassFlags.Unicode));
    }
}
