using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class PushStream : AdvancedPlayable
    {
        public PushStream(BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            Handle = Bass.CreateStream(44100, 2, BufferKind.ToBassFlag(), StreamProcedureType.Push);
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
    }
}
