using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Provides access to a Buffer allocated by Bass.
    /// </summary>
    public class BufferProvider
    {
        #region Fields
        /// <summary>
        /// Pointer to the Unmanaged Buffer
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// Length in Bytes
        /// </summary>
        public int ByteLength { get; private set; }

        /// <summary>
        /// Length in Floats
        /// </summary>
        public int FloatLength { get { return ByteLength / 4; } }

        /// <summary>
        /// Length in Shorts
        /// </summary>
        public int ShortLength { get { return ByteLength / 2; } }

        /// <summary>
        /// Length in Ints
        /// </summary>
        public int IntLength { get { return ByteLength / 4; } }
        #endregion

        public BufferProvider(IntPtr Buffer, int ByteLength)
        {
            this.Pointer = Buffer;
            this.ByteLength = ByteLength;
        }

        #region Read
        public void Read(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = FloatLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public void Read(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ByteLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public void Read(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ShortLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public void Read(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = IntLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }
        #endregion

        #region Write
        public void Write(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = FloatLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ByteLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ShortLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = IntLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }
        #endregion
    }
}