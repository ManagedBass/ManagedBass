using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassDsd: bassdsd.dll
    /// </summary> 
    /// <remarks>
    /// <para>Supports: *.dsf, *.dff, *.dsd</para>
    /// </remarks>
    public static class BassDsd
    {
        const string DllName = "bassdsd";

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) { Extensions.Load(DllName, Folder); }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags, int freq);

        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags, int freq);

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            return BASS_DSD_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            return BASS_DSD_StreamCreateFile(true, Memory, Offset, Length, Flags, Frequency);
        }

        #region From Array
        static int CreateStreamObj(object Memory, long Offset, long Length, BassFlags Flags, int Frequency)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(short[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(int[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(float[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }
        #endregion

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags, Frequency);
        }

        [DllImport(DllName)]
        static extern int BASS_DSD_StreamCreateFileUser(StreamSystem system, BassFlags flags, IntPtr procs, IntPtr user, int freq);

        public static int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr), int Frequency = 44100)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(procs));

            Marshal.StructureToPtr(procs, ptr, true);

            int handle = BASS_DSD_StreamCreateFileUser(system, flags, ptr, user, Frequency);

            Marshal.FreeHGlobal(ptr);

            return handle;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_DSD_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100);

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100)
        {
            return BASS_DSD_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User, Frequency);
        }
    }
}