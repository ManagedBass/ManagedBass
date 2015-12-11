using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public enum BufferKind
    {
        /// <summary>
        /// 16-Bit (Default)
        /// </summary>
        Short,

        /// <summary>
        /// 8-Bit
        /// </summary>
        Byte,

        /// <summary>
        /// 32-Bit IEEE Floating Point
        /// </summary>
        Float
    }

    public class BufferProvider
    {
        #region Fields
        /// <summary>
        /// Pointer to the Unmanaged Buffer
        /// </summary>
        public IntPtr Pointer { get; private set; }

        public BufferKind BufferKind { get; private set; }

        /// <summary>
        /// Length in Bytes
        /// </summary>
        public int ByteLength { get; private set; }

        public int FloatLength { get { return ByteLength / 4; } }

        public int ShortLength { get { return ByteLength / 2; } }

        public int IntLength { get { return ByteLength / 4; } }
        #endregion

        public BufferProvider(IntPtr Buffer, int ByteLength, BufferKind BufferKind = BufferKind.Short)
        {
            this.Pointer = Buffer;
            this.ByteLength = ByteLength;
            this.BufferKind = BufferKind;
        }

        #region Read
        public float[] ReadFloat(int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = FloatLength;

            float[] temp = new float[Length];

            Read(temp, Offset, Length);

            return temp;
        }

        public void Read(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = FloatLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public byte[] ReadByte(int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ByteLength;

            byte[] temp = new byte[Length];

            Read(temp, Offset, Length);

            return temp;
        }

        public void Read(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ByteLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public short[] ReadShort(int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ShortLength;

            short[] temp = new short[Length];

            Read(temp, Offset, Length);

            return temp;
        }

        public void Read(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ShortLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }

        public int[] ReadInt(int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = IntLength;

            int[] temp = new int[Length];

            Read(temp, Offset, Length);

            return temp;
        }

        public void Read(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = IntLength;

            Marshal.Copy(Pointer, Data, Offset, Length);
        }
        #endregion

        #region Write
        public void Write(float[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = FloatLength;
            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(byte[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ByteLength;
            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(short[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = ShortLength;
            Marshal.Copy(Data, 0, Pointer, Length);
        }

        public void Write(int[] Data, int Offset = 0, int Length = 0)
        {
            if (Length == 0) Length = IntLength;
            Marshal.Copy(Data, 0, Pointer, Length);
        }
        #endregion
    }
}