using ManagedBass.Dynamics;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Stream an audio file using .Net file handling
    /// </summary>
    public class ManagedFileChannel : Channel
    {
        FileProcedures procs;
        FileStream Stream;

        public ManagedFileChannel(string FileName, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            procs = new FileProcedures();
            procs.Close = (u) => Stream.Dispose();
            procs.Length = (u) => Stream.Length;
            procs.Read = ReadProc;
            procs.Seek = SeekProc;
            
            Handle = Bass.CreateStream(StreamSystem.Buffer, FlagGen(IsDecoder, Resolution), procs);
        }

        byte[] b;

        int ReadProc(IntPtr buffer, int length, IntPtr user)
        {
            try
            {
                if (b == null || b.Length < length)
                    b = new byte[length];

                int read = Stream.Read(b, 0, length);

                Marshal.Copy(b, 0, buffer, read);

                return read;
            }
            catch { return 0; }
        }

        bool SeekProc(long offset, IntPtr user)
        {
            try
            {
                Stream.Seek(offset, SeekOrigin.Current);
                return true;
            }
            catch { return false; }
        }
    }
}