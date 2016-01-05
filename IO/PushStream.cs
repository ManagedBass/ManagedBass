using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class PushStream : Channel
    {
        public PushStream(Resolution BufferKind = Resolution.Short)
            : base(false, BufferKind)
        {
            Handle = Bass.CreateStream(44100, 2, BufferKind.ToBassFlag(), StreamProcedureType.Push);

            Player = new BassPlayer(Handle, this);
        }

        #region Push
        bool Push(object data, int Length)
        {
            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);

            bool Result = Bass.StreamPutData(Handle, gch.AddrOfPinnedObject(), Length) != -1;

            gch.Free();

            return Result;
        }

        public bool Push(byte[] data, int Length) { return Push(data as object, Length); }

        public bool Push(float[] data, int Length) { return Push(data as object, Length); }

        public bool Push(short[] data, int Length) { return Push(data as object, Length); }

        public bool Push(int[] data, int Length) { return Push(data as object, Length); }
        #endregion

        public void End() { Bass.StreamPutData(Handle, IntPtr.Zero, (int)StreamProcedureType.End); }
    }
}
