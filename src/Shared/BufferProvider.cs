using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Provides access to a buffer allocated by Bass.
    /// </summary>
    public class BufferProvider
    {
        internal BufferProvider(IntPtr Pointer, int Length)
        {
            this.Pointer = Pointer;
            this.Length = Length;
        }

        /// <summary>
        /// Gets an <see cref="IntPtr"/> to the Buffer.
        /// </summary>
        public IntPtr Pointer { get; }

        /// <summary>
        /// Length of data in buffer in bytes.
        /// </summary>
        /// <remarks>
        /// <p>Length in Shorts = Length in Bytes / 2.</p>
        /// <p>Length in Floats = Length in Bytes / 4.</p>
        /// </remarks>
        public int Length { get; }

        public void Read(byte[] Data, int Offset, int Length) => Marshal.Copy(Pointer, Data, Offset, Length);
        public void Read(short[] Data, int Offset, int Length) => Marshal.Copy(Pointer, Data, Offset, Length / 2);
        public void Read(float[] Data, int Offset, int Length) => Marshal.Copy(Pointer, Data, Offset, Length / 4);

        public void Write(byte[] Data, int Offset, int Length) => Marshal.Copy(Data, Offset, Pointer, Length);
        public void Write(short[] Data, int Offset, int Length) => Marshal.Copy(Data, Offset, Pointer, Length / 2);
        public void Write(float[] Data, int Offset, int Length) => Marshal.Copy(Data, Offset, Pointer, Length / 4);
    }
}