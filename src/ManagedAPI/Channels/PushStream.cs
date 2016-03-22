using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio using Push system (calling Push() to feed data)
    /// </summary>
    public class PushStream : Channel
    {
        public PushStream(bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.CreateStream(44100, 2, FlagGen(IsDecoder, Resolution), StreamProcedureType.Push);
        }

        #region Push
        public bool Push(IntPtr data, int Length) => Bass.StreamPutData(Handle, data, Length) != -1;

        public bool Push(byte[] data, int Length) => Bass.StreamPutData(Handle, data, Length) != -1;

        public bool Push(float[] data, int Length) => Bass.StreamPutData(Handle, data, Length) != -1;

        public bool Push(short[] data, int Length) => Bass.StreamPutData(Handle, data, Length) != -1;

        public bool Push(int[] data, int Length) => Bass.StreamPutData(Handle, data, Length) != -1;
        #endregion

        public void End() => Bass.StreamPutData(Handle, IntPtr.Zero, (int)StreamProcedureType.End);
    }
}
