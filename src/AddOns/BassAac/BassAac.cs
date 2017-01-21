using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Aac
{
    /// <summary>
    /// BassAac is a BASS addon enabling playback of AAC and MP4 streams.
    /// </summary> 
    /// <remarks>
    /// Supports .aac, .adts, .mp4, .m4a, .m4b
    /// </remarks>
    public static class BassAac
    {
        const string DllName = "bass_aac";

        static IntPtr hLib;
        
        /// <summary>
        /// Load this library into Memory.
		/// </summary>
        /// <param name="Folder">Directory to Load from... <see langword="null"/> (default) = Load from Current Directory.</param>
		/// <returns><see langword="true" />, if the library loaded successfully, else <see langword="false" />.</returns>
        /// <remarks>
		/// <para>
		/// An external library is loaded into memory when any of its methods are called for the first time.
		/// This results in the first method call being slower than all subsequent calls.
		/// </para>
		/// <para>
		/// Some BASS libraries and add-ons may introduce new options to the main BASS lib like new parameters.
		/// But, before using these new options the respective library must be already loaded.
		/// This method can be used to make sure, that this library has been loaded.
		/// </para>
		/// </remarks>
        public static bool Load(string Folder = null) => (hLib = DynamicLibrary.Load(DllName, Folder)) != IntPtr.Zero;
		
		/// <summary>
		/// Unloads this library from Memory.
		/// </summary>
		/// <returns><see langword="true" />, if the library unloaded successfully, else <see langword="false" />.</returns>
        public static bool Unload() => DynamicLibrary.Unload(hLib);

        /// <summary>
        /// Play audio from Mp4... default = true.
        /// </summary>
        public static bool PlayAudioFromMp4
        {
            get { return Bass.GetConfigBool(Configuration.PlayAudioFromMp4); }
            set { Bass.Configure(Configuration.PlayAudioFromMp4, value); }
        }

        /// <summary>
        /// Support Mp4 in Aac functions.
        /// </summary>
        public static bool AacSupportMp4
        {
            get { return Bass.GetConfigBool(Configuration.AacSupportMp4); }
            set { Bass.Configure(Configuration.AacSupportMp4, value); }
        }

        /// <summary>
        /// Use this library as a Plugin.
        /// </summary>
        public static readonly Plugin Plugin = new Plugin(DllName);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_AAC_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_AAC_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags);

        /// <summary>Create a stream from file.</summary>
        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_AAC_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        /// <summary>Create a stream from Memory (IntPtr).</summary>
        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_AAC_StreamCreateFile(true, new IntPtr(Memory.ToInt64() + Offset), 0, Length, Flags);
        }

        /// <summary>Create a stream from Memory (byte[]).</summary>
        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return GCPin.CreateStreamHelper(Pointer => CreateStream(Pointer, Offset, Length, Flags), Memory);
        }

        [DllImport(DllName)]
        static extern int BASS_AAC_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user);

        /// <summary>Create a stream using User File Procedures.</summary>
        public static int CreateStream(StreamSystem System, BassFlags Flags, FileProcedures Procedures, IntPtr User = default(IntPtr))
        {
            var h = BASS_AAC_StreamCreateFileUser(System, Flags, Procedures, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedures);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_AAC_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User);

        /// <summary>Create a stream from Url.</summary>
        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            var h = BASS_AAC_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedure);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MP4_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_MP4_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags);

        /// <summary>Create a stream from file.</summary>
        public static int CreateMp4Stream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_MP4_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        /// <summary>Create a stream from Memory (IntPtr).</summary>
        public static int CreateMp4Stream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_MP4_StreamCreateFile(true, new IntPtr(Memory.ToInt64() + Offset), 0, Length, Flags);
        }

        /// <summary>Create a stream from Memory (byte[]).</summary>
        public static int CreateMp4Stream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return GCPin.CreateStreamHelper(Pointer => CreateMp4Stream(Pointer, Offset, Length, Flags), Memory);
        }

        [DllImport(DllName)]
        static extern int BASS_MP4_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user);

        /// <summary>Create a stream using User File Procedures.</summary>
        public static int CreateMp4Stream(StreamSystem System, BassFlags Flags, FileProcedures Procedures, IntPtr User = default(IntPtr))
        {
            var h = BASS_MP4_StreamCreateFileUser(System, Flags, Procedures, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedures);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MP4_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User);

        /// <summary>Create a stream from Url.</summary>
        public static int CreateMp4Stream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            var h = BASS_MP4_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedure);

            return h;
        }
    }
}