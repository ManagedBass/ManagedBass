using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Creates a Channel from a <see cref="Stream"/> using <see cref="FileProcedures"/>.
    /// </summary>
    public class StreamChannel : Channel
    {
        readonly Stream _stream;
        
        /// <summary>
        /// Creates a new Instance of <see cref="StreamChannel"/>.
        /// </summary>
        /// <param name="InputStream">Stream to load.</param>
        /// <param name="IsDecoder">Whether to create a Decoding Channel.</param>
        /// <param name="Resolution">Channel Resolution to use.</param>
        public StreamChannel(Stream InputStream, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            _stream = InputStream;
            
            if (!_stream.CanRead)
                throw new ArgumentException("Provide a readable stream", nameof(InputStream));

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
            if (!_stream.CanSeek)
                return false;

            try
            {
                _stream.Seek(offset, SeekOrigin.Current);
                return true;
            }
            catch { return false; }
        }
    }
}