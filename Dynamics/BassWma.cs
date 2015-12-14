using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum WMAEncodeFlags
    {
        Unicode = -2147483648,
        Default = 0,
        Byte = 1,
        Float = 256
    }

    public delegate void WMEncodeProcedure(int handle, int type, IntPtr buffer, int length, IntPtr user);

    public delegate void ClientConnectProcedure(int handle, bool connect, string ip, IntPtr user);

    public static class BassWma
    {
        // TODO: BASS_WMA_StreamCreateFileAuth
        // TODO: BASS_WMA_StreamCreateFileUser
        // TODO: BASS_WMA_GetTags
        // TODO: BASS_WMA_EncodeGetRates

        const string DllName = "basswma.dll";

        static BassWma() { BassManager.Load(DllName); }

        #region Streams
        [DllImport(DllName, EntryPoint = "BASS_WMA_GetWMObject")]
        public static extern IntPtr GetWMObject(int handle);

        [DllImport(DllName)]
        static extern int BASS_WMA_StreamCreateFile(bool Memory, [MarshalAs(UnmanagedType.LPWStr)]string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_WMA_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        public static int CreateStream(string File, long Offset, long Length, BassFlags Flags)
        {
            return BASS_WMA_StreamCreateFile(false, File, Offset, Length, Flags);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags)
        {
            return BASS_WMA_StreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            return Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }
        #endregion

        #region Encode
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpen")]
        public static extern int EncodeOpen(int freq, int chans, WMAEncodeFlags flags, int bitrate, WMEncodeProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenFile")]
        public static extern int EncodeOpenFile(int freq, int chans, WMAEncodeFlags flags, int bitate, string file);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetwork")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, int bitrate, int port, int clients);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetworkMuli")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, [MarshalAs(UnmanagedType.LPArray)] int[] bitrate, int port, int clients);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenPublish")]
        public static extern int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int bitrate, string url, string user, string pass);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenPublishMulti")]
        public static extern int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, [MarshalAs(UnmanagedType.LPArray)] int[] bitrate, string url, string user, string pass);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeSetNotify")]
        public static extern bool EncodeSetNotify(int handle, ClientConnectProcedure proc, IntPtr user);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeSetTag")]
        public static extern bool EncodeSetTag(int handle, string tag, string value, int form);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetClients")]
        public static extern int EncodeGetClients(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetPort")]
        public static extern int EncodeGetPort(int handle);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeClose")]
        public static extern bool EncodeClose(int handle);
        #endregion

        #region Configuration
        public static bool CanSeekNetworkStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetSeek); }
            set { Bass.Configure(Configuration.WmaNetSeek, value); }
        }

        public static bool PlayWMVAudio
        {
            get { return Bass.GetConfigBool(Configuration.WmaVideo); }
            set { Bass.Configure(Configuration.WmaVideo, value); }
        }

        public static bool PrebufferInternetStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetPreBuffer); }
            set { Bass.Configure(Configuration.WmaNetPreBuffer, value); }
        }

        public static bool UseBassFileHandling
        {
            get { return Bass.GetConfigBool(Configuration.WmaBassFileHandling); }
            set { Bass.Configure(Configuration.WmaBassFileHandling, value); }
        }

        public static bool AsyncDecoding
        {
            get { return Bass.GetConfigBool(Configuration.WmaAsync); }
            set { Bass.Configure(Configuration.WmaAsync, value); }
        }
        #endregion
    }
}