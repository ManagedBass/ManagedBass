using System;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio using Push system (calling Push() to feed data)
    /// </summary>
    public sealed class PushStream : Channel
    {
        public PushStream(int Frequency = 44100, int Channels = 2, BassFlags Flags = BassFlags.Default)
        {
            Handle = Bass.CreateStream(Frequency, Channels, Flags, StreamProcedureType.Push);
        }

        #region Push
        public bool Push(IntPtr Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(byte[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(float[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(short[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;

        public bool Push(int[] Data, int Length) => Bass.StreamPutData(Handle, Data, Length) != -1;
        #endregion

        /// <summary>
        /// Ends the Push Strem.
        /// </summary>
        public void End() => Bass.StreamPutData(Handle, IntPtr.Zero, (int)StreamProcedureType.End);
    }
}