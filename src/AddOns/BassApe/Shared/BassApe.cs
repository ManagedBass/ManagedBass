using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Ape
{
    /// <summary>
    /// BassApe is a BASS addon enabling the playback of Monkey's Audio encoded files.
    /// </summary> 
    /// <remarks>
    /// Supports .ape, .ap1
    /// </remarks>
    public static class BassApe
    {
#if __IOS__
        const string DllName = "__Internal";
#else
        const string DllName = "bass_ape";
#endif
        
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
        /// Use this library as a Plugin.
        /// </summary>
        public static readonly Plugin Plugin = new Plugin(DllName);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_APE_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags);

        [DllImport(DllName)]
        static extern int BASS_APE_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags);

        /// <summary>Create a stream from file.</summary>
        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_APE_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        /// <summary>Create a stream from Memory (IntPtr).</summary>
        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_APE_StreamCreateFile(true, new IntPtr(Memory.ToInt64() + Offset), 0, Length, Flags);
        }

        /// <summary>Create a stream from Memory (byte[]).</summary>
        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            return GCPin.CreateStreamHelper(Pointer => CreateStream(Pointer, Offset, Length, Flags), Memory);
        }

        [DllImport(DllName)]
        static extern int BASS_APE_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user);

        /// <summary>Create a stream using User File Procedures.</summary>
        public static int CreateStream(StreamSystem System, BassFlags Flags, FileProcedures Procedures, IntPtr User = default(IntPtr))
        {
            var h = BASS_APE_StreamCreateFileUser(System, Flags, Procedures, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedures);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_APE_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User);

        /// <summary>Create a stream from Url.</summary>
        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            var h = BASS_APE_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User);

            if (h != 0)
                ChannelReferences.Add(h, 0, Procedure);

            return h;
        }
    }
}