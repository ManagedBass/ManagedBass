using ManagedBass.Dynamics;
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
        public bool Push(IntPtr data, int Length) { return Bass.StreamPutData(Handle, data, Length) != -1; }

        bool PushObj(object data, int Length)
        {
            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);

            bool Result = Bass.StreamPutData(Handle, gch.AddrOfPinnedObject(), Length) != -1;

            gch.Free();

            return Result;
        }

        public bool Push(byte[] data, int Length) { return PushObj(data, Length); }

        public bool Push(float[] data, int Length) { return PushObj(data, Length); }

        public bool Push(short[] data, int Length) { return PushObj(data, Length); }

        public bool Push(int[] data, int Length) { return PushObj(data, Length); }
        #endregion

        public void End() { Bass.StreamPutData(Handle, IntPtr.Zero, (int)StreamProcedureType.End); }
    }
}
