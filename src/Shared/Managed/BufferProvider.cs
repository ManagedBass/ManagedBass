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
        public IntPtr Pointer { get; }

        /// <summary>
        /// Length in Bytes
        /// </summary>
        public int ByteLength { get; }

        /// <summary>
        /// Length in Floats
        /// </summary>
        public int FloatLength => ByteLength / 4;

        /// <summary>
        /// Length in Shorts
        /// </summary>
        public int ShortLength => ByteLength / 2;

        /// <summary>
        /// Length in Ints
        /// </summary>
        public int IntLength => ByteLength / 4;
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="BufferProvider"/>.
        /// </summary>
        /// <param name="Buffer">The Pointer to the Buffer.</param>
        /// <param name="ByteLength">Length of the Buffer.</param>
        public BufferProvider(IntPtr Buffer, int ByteLength)
        {
            Pointer = Buffer;
            this.ByteLength = ByteLength;
        }

        #region Read
        /// <summary>
        /// Reads data from the Buffer.
        /// </summary>
        /// <param name="Data">float[] to read the data into (Must be allocated to an appropriate size by you).</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of floats to read... 0 to read all available.</param>
        public void Read(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = FloatLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        /// <summary>
        /// Reads data from the Buffer.
        /// </summary>
        /// <param name="Data">byte[] to read the data into (Must be allocated to an appropriate size by you).</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of bytes to read... 0 to read all available.</param>
        public void Read(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ByteLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        /// <summary>
        /// Reads data from the Buffer.
        /// </summary>
        /// <param name="Data">short[] to read the data into (Must be allocated to an appropriate size by you).</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of shorts to read... 0 to read all available.</param>
        public void Read(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ShortLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        /// <summary>
        /// Reads data from the Buffer.
        /// </summary>
        /// <param name="Data">int[] to read the data into (Must be allocated to an appropriate size by you).</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of ints to read... 0 to read all available.</param>
        public void Read(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = IntLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }
        #endregion

        #region Write
        /// <summary>
        /// Writes data into the Buffer.
        /// </summary>
        /// <param name="Data">float[] to write the data from.</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of floats to read... 0 to read all available.</param>
        public void Write(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = FloatLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        /// <summary>
        /// Writes data into the Buffer.
        /// </summary>
        /// <param name="Data">byte[] to write the data from.</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of bytes to read... 0 to read all available.</param>
        public void Write(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ByteLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        /// <summary>
        /// Writes data into the Buffer.
        /// </summary>
        /// <param name="Data">short[] to write the data from.</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of shorts to read... 0 to read all available.</param>
        public void Write(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = ShortLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }

        /// <summary>
        /// Writes data into the Buffer.
        /// </summary>
        /// <param name="Data">int[] to write the data from.</param>
        /// <param name="Offset">Zero based index in <paramref name="Data"/> array where coping must start.</param>
        /// <param name="Length">No of ints to read... 0 to read all available.</param>
        public void Write(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0 || Length > ByteLength)
                Length = IntLength;

            Marshal.Copy(Data, 0, Pointer, Length);
        }
        #endregion
    }
}