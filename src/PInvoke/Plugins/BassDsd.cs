



using System;
using System.Runtime.InteropServices;

namespace ManagedBass{
    	/// <summary>
    /// Wraps BassDsd
    /// </summary> 
    /// <remarks>
    /// Supports .dsf, .dff, .dsd
    /// </remarks>
	    public static partial class BassDsd
    {
#if __IOS__
        const string DllName = "__internal";
#else
        const string DllName = "bassdsd";
#endif

#if __ANDROID__ || WINDOWS || LINUX || __MAC__
        static IntPtr hLib;
        
        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = DynamicLibrary.Load(DllName, Folder);

        public static void Unload() => DynamicLibrary.Unload(hLib);
#endif

        public static readonly Plugin Plugin = new Plugin(DllName);		
        

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags, int Frequency = 0);

        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags, int Frequency = 0);

		/// <summary>Create a stream from file.</summary>
        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            return BASS_DSD_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency);
        }

		/// <summary>Create a stream from Memory (IntPtr).</summary>
        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            return BASS_DSD_StreamCreateFile(true, new IntPtr(Memory.ToInt64() + Offset), 0, Length, Flags, Frequency);
        }

		/// <summary>Create a stream from Memory (byte[]).</summary>
        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 0)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            var Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }
        
        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user, int Frequency = 0);

		/// <summary>Create a stream using User File Procedures.</summary>
        public static int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr), int Frequency = 0)
        {
            var h = BASS_DSD_StreamCreateFileUser(system, flags, procs, user, Frequency);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, procs);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User, int Frequency = 0);

		/// <summary>Create a stream from Url.</summary>
        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 0)
        {
            var h = BASS_DSD_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User, Frequency);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, Procedure);

            return h;
        }
    }
}
