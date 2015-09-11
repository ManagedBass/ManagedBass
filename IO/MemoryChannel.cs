using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Streams an Audio file from Memory
    /// </summary>
    public class MemoryChannel : Channel
    {
        GCHandle GCPin;

        public MemoryChannel(byte[] Memory, long Offset, long Length, bool IsDecoder = false)
        {
            GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Handle = Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, FlagGen(IsDecoder, Resolution.Byte));
        }

        public MemoryChannel(float[] Memory, long Offset, long Length, bool IsDecoder = false)
        {
            GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Handle = Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, FlagGen(IsDecoder, Resolution.Float));
        }

        public override void Dispose()
        {
            base.Dispose();

            if (GCPin != null) GCPin.Free();
        }
    }
}