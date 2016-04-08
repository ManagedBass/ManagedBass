using System;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio using Push system (calling Push() to feed data)
    /// </summary>
    public sealed class PushStream : Channel
    {
        public PushStream(bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.CreateStream(44100, 2, FlagGen(IsDecoder, Resolution), StreamProcedureType.Push);
        }

        #region Push
        public bool Push(IntPtr Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(byte[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(float[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(short[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(int[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;
        #endregion

        public void End() => Bass.StreamPutData(Handle, IntPtr.Zero, (int)StreamProcedureType.End);
    }
}
