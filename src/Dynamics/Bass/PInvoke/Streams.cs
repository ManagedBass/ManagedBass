using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        [DllImport(DllName, EntryPoint = "BASS_StreamGetFilePosition")]
        public static extern long StreamGetFilePosition(int Handle, FileStreamPosition Mode = FileStreamPosition.Current);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_StreamCreateFile(bool Memory, string File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName)]
        static extern int BASS_StreamCreateFile(bool Memory, IntPtr File, long Offset, long Length, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_StreamCreateFileUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user = default(IntPtr));

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default)
        {
            return BASS_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default)
        {
            return BASS_StreamCreateFile(true, Memory, Offset, Length, Flags);
        }

        #region From Array
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
        #endregion

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr));

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr))
        {
            return BASS_StreamCreateURL(Url, Offset, Flags, Procedure, User);
        }

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_StreamCreate")]
        public extern static int CreateStream(int Frequency, int Channels, BassFlags Flags, StreamProcedureType Procedure, IntPtr User = default(IntPtr));

        #region Stream Put Data
        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutData")]
        public extern static int StreamPutData(int Handle, float[] Buffer, int Length);
        #endregion

        #region Stream Put File Data
        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_StreamPutFileData")]
        public extern static int StreamPutFileData(int Handle, float[] Buffer, int Length);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_StreamFree")]
        public static extern bool StreamFree(int Handle);
    }
}
