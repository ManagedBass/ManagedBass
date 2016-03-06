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

        public MemoryChannel(byte[] Memory, int Offset, long Length, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            Handle = Bass.CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, FlagGen(IsDecoder, Resolution));
        }
        
        public override void Dispose()
        {
            base.Dispose();

			if (GCPin != null && GCPin.IsAllocated) GCPin.Free();
        }
    }
}