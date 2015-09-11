using System;
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

    public static class BassWma
    {
        const string DllName = "basswma.dll";

        static BassWma() { BassManager.Load(DllName); }

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenFile")]
        public static extern int EncodeOpenFile(int freq, int chans, WMAEncodeFlags flags, int bitate, string file);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeClose")]
        public static extern bool EncodeClose(int handle);

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
    }
}