using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        /// <summary>
        /// Retrieves the decoding/download/end position of a file stream.
        /// </summary>
        /// <param name="Handle">The stream's handle.</param>
        /// <param name="Mode">The file position to retrieve. One of <see cref="FileStreamPosition" /> values.</param>
        /// <returns>If succesful, then the requested file position is returned, else -1 is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>ID3 tags (both v1 and v2) and WAVE headers, as well as any other rubbish at the start of the file, are excluded from the calculations of this function.</para>
        /// <para>This is useful for average bitrate calculations, but it means that the <see cref="FileStreamPosition.Current"/> position may not be the actual file position - the <see cref="FileStreamPosition.Start"/> position can be added to it to get the actual file position.</para>
        /// <para>
        /// When streaming a file from the internet or a "buffered" user file stream, the entire file is downloaded even if the audio data ends before that, in case there are tags to be read.
        /// This means that the <see cref="FileStreamPosition.Download"/> position may go beyond the <see cref="FileStreamPosition.End"/> position.
        /// </para>
        /// <para>
        /// It's unwise to use this function (with mode = <see cref="FileStreamPosition.Current"/>) for syncing purposes because it returns the position that's being decoded, not the position that's being heard.
        /// Use <see cref="ChannelGetPosition" /> for syncing instead.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.InvalidHandle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotFileStream">The stream is not a file stream.</exception>
        /// <exception cref="Errors.NotAvailable">The requested file position/status is not available.</exception>
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
            return BASS_StreamCreateFile(true, new IntPtr(Memory.ToInt32() + Offset), 0, Length, Flags);
        }
        
        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
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
