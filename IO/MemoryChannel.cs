using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class MemoryChannel : Channel
    {
        GCHandle GCPin;

        public MemoryChannel(byte[] Memory, long Offset, long Length, bool IsDecoder = false, Resolution BufferKind = Resolution.Byte)
            : base(IsDecoder, BufferKind)
        {
            GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (GCPin != null) GCPin.Free();
        }
    }
}