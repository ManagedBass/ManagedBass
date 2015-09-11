using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class MemoryDecoder : Decoder
    {
        GCHandle GCPin;

        ~MemoryDecoder() { Dispose(); }

        public MemoryDecoder(byte[] Memory, long Offset = 0, long Length = 0, BufferKind BufferKind = BufferKind.Byte)
            : base(BufferKind)
        {
            GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Handle = Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, BassFlags.Decode | BufferKind.ToBassFlag());
        }

        public override void Dispose()
        {
            base.Dispose();
            if (!IsDisposed) GCPin.Free();
        }
    }
}