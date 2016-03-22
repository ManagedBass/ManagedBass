using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Wma
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaFileWriter : IAudioFileWriter
    {
        int EncoderHandle;
        
        /// <summary>
        /// The Resolution for the encoded data.
        /// </summary>
        public Resolution Resolution { get; }

        /// <summary>
        /// Creates a new instance of <see cref="WmaFileWriter"/>.
        /// </summary>
        public WmaFileWriter(string FilePath, int NoOfChannels = 2, int SampleRate = 44100, int BitRate = 128000, Resolution Resolution = Resolution.Short)
        {
            this.Resolution = Resolution;

            WMAEncodeFlags flags = WMAEncodeFlags.Default;
            if (Resolution == Resolution.Byte) flags |= WMAEncodeFlags.Byte;
            else if (Resolution == Resolution.Float) flags |= WMAEncodeFlags.Float;

            EncoderHandle = BassWma.EncodeOpenFile(SampleRate, NoOfChannels, flags, BitRate, FilePath);
        }
        
        /// <summary>
        /// Write data from an IntPtr.
        /// </summary>
        /// <param name="Buffer">IntPtr to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public void Write(IntPtr Buffer, int Length) => BassWma.EncodeWrite(EncoderHandle, Buffer, Length);

        void Write(object buffer, int Length)
        {
            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Write(gch.AddrOfPinnedObject(), Length);

            gch.Free();
        }
        
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public void Write(byte[] Buffer, int Length) => Write(Buffer as object, Length);
        
        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        public void Write(short[] Buffer, int Length) => Write(Buffer as object, Length);
        
        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        public void Write(float[] Buffer, int Length) => Write(Buffer as object, Length);

        /// <summary>
        /// Frees all resources used by the writer.
        /// </summary>
        public void Dispose() => BassWma.EncodeClose(EncoderHandle);
    }
}