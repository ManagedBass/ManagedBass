using System;
using System.Runtime.InteropServices;
namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassAac: bass_aac.dll
    /// </summary>
    public static class BassAac
    {
        const string DllName = "bass_aac.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        #region Create AAC Stream
        [DllImport(DllName)]
        static extern int BASS_AAC_StreamCreateFile(bool mem, IntPtr memory, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_AAC_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        public static int CreateStream(IntPtr memory, long offset, long length, BassFlags flags)
        {
            return BASS_AAC_StreamCreateFile(true, memory, offset, length, flags);
        }

        public static int CreateStream(string file, long offset, long length, BassFlags flags)
        {
            return BASS_AAC_StreamCreateFile(false, file, offset, length, flags);
        }

        [DllImport(DllName, EntryPoint = "BASS_AAC_StreamCreateUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user);
        
        // TODO: Unicode
        [DllImport(DllName, EntryPoint = "BASS_AAC_StreamCreateURL")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPWStr)]string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));
        #endregion

        #region Create MP4 Stream
        [DllImport(DllName)]
        static extern int BASS_MP4_StreamCreateFile(bool mem, IntPtr memory, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_MP4_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        public static int CreateStreamMp4(IntPtr memory, long offset, long length, BassFlags flags)
        {
            return BASS_MP4_StreamCreateFile(true, memory, offset, length, flags);
        }

        public static int CreateStreamMp4(string file, long offset, long length, BassFlags flags)
        {
            return BASS_MP4_StreamCreateFile(false, file, offset, length, flags);
        }

        [DllImport(DllName, EntryPoint = "BASS_MP4_StreamCreateUser")]
        public static extern int CreateStreamMp4(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user);
        #endregion

        /// <summary>
        /// Play audio from mp4 (video) files?
        /// playmp4 (bool): If true (default) BassAac will play the audio from mp4 video files. 
        /// If false mp4 video files will not be played.
        /// </summary>
        public static bool PlayAudioFromMp4
        {
            get { return Bass.GetConfigBool(Configuration.PlayAudioFromMp4); }
            set { Bass.Configure(Configuration.PlayAudioFromMp4, value); }
        }

        /// <summary>
        /// BASSaac add-on: Support MP4 in BASS_AAC_StreamCreateXXX functions?
        /// usemp4 (bool): If true BASSaac supports MP4 in the BASS_AAC_StreamCreateXXX functions. 
        /// If false (default) only AAC is supported.
        /// </summary>
        public static bool SupportMp4
        {
            get { return Bass.GetConfigBool(Configuration.AacSupportMp4); }
            set { Bass.Configure(Configuration.AacSupportMp4, value); }
        }
    }
}
