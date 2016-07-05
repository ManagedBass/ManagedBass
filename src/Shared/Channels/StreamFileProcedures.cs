using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class StremFileProcedures : FileProcedures
    {
        readonly Stream _stream;

        public StremFileProcedures(Stream InputStream)
        {
            _stream = InputStream;

            if (!_stream.CanRead)
                throw new ArgumentException("Provide a readable stream", nameof(InputStream));

            Close = u => _stream.Dispose();
            Length = u => _stream.Length;
            Read = ReadProc;
            Seek = SeekProc;
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