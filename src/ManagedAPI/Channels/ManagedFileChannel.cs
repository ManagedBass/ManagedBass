using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Stream an audio file using .Net file handling using <see cref="FileProcedures"/>.
    /// </summary>
    public sealed class ManagedFileChannel : Channel
    {
        readonly FileStream _stream;

        /// <summary>
        /// Gets the path of the Loaded file
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Creates a new Instance of <see cref="ManagedFileChannel"/>.
        /// </summary>
        /// <param name="FileName">Path to the file to load.</param>
        /// <param name="IsDecoder">Whether to create a Decoding Channel.</param>
        /// <param name="Resolution">Channel Resolution to use.</param>
        public ManagedFileChannel(string FileName, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            _stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            this.FileName = FileName;

            var procs = new FileProcedures
            {
                Close = u => _stream.Dispose(),
                Length = u => _stream.Length,
                Read = ReadProc,
                Seek = SeekProc
            };

            Handle = Bass.CreateStream(StreamSystem.Buffer, FlagGen(IsDecoder, Resolution), procs);
        }

        byte[] b;

        int ReadProc(IntPtr buffer, int length, IntPtr user)
        {
            try
            {
                if (b == null || b.Length < length)
                    b = new byte[length];

                var read = _stream.Read(b, 0, length);

                Marshal.Copy(b, 0, buffer, read);

                return read;
            }
            catch { return 0; }
        }

        bool SeekProc(long offset, IntPtr user)
        {
            try
            {
                _stream.Seek(offset, SeekOrigin.Current);
                return true;
            }
            catch { return false; }
        }
    }
}