using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    /// <summary>
    /// Writes audio to a file, encoding it using ACM
    /// </summary>
    public class ACMEncodedFileWriter : ACMFileEncoder, IAudioFileWriter
    {
        /// <summary>
        /// Creates a new instance of <see cref="ACMEncodedFileWriter"/>.
        /// </summary>
        public ACMEncodedFileWriter(string FileName, WaveFormatTag encoding,
            int Channels = 2, int SampleRate = 44100, Resolution Resolution = Resolution.Short, EncodeFlags flags = EncodeFlags.Default)
            : base(FileName, Bass.CreateStream(SampleRate, Channels, Resolution.ToBassFlag() | BassFlags.Decode, StreamProcedureType.Push),
                   EncodeFlags.Default, encoding)
        {
            this.Resolution = Resolution;
        }

        /// <summary>
        /// Write data from an IntPtr.
        /// </summary>
        /// <param name="Buffer">IntPtr to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public void Write(IntPtr Buffer, int Length)
        {
            // Push Data to the encoder
            Bass.StreamPutData(Channel, Buffer, Length);

            // Retrieve data from encoder to encode the data
            Bass.ChannelGetData(Channel, Buffer, Length);
        }

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
        /// The Resolution for the encoded data.
        /// </summary>
        public Resolution Resolution { get; }
    }
}