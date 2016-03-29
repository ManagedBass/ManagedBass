using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.Marshal;

namespace ManagedBass
{
    /// <summary>
    /// Streams a MOD music file.
    /// </summary>
    public sealed class MusicModule : Channel
    {
        public MusicModule(string FilePath, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = Bass.MusicLoad(FilePath, 0, 0, FlagGen(IsDecoder, Resolution), 0);
        }

        public MusicModule(byte[] Memory, int Length, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            var gcPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.MusicLoad(gcPin.AddrOfPinnedObject(), 0, Length, FlagGen(IsDecoder, Resolution));
            gcPin.Free();
        }

        public string Title => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicName));

        public string Author => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicAuth));

        public string Message => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicMessage));

        public string Instrument(int Index) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicInstrument + Index));

        public string MusicSampleName(int Index) => PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicSample + Index));

        public byte[] MusicOrders
        {
            get
            {
                var n = (int)Bass.ChannelGetLength(Handle, PositionFlags.MusicOrders);

                var b = new byte[n];

                var ptr = Bass.ChannelGetTags(Handle, TagType.MusicOrders);

                Copy(ptr, b, 0, n);

                return b;
            }
        }

        public int MusicChannelCount
        {
            get
            {
                var channels = 0;
                float dummy;

                while (Bass.ChannelGetAttribute(Handle, ChannelAttribute.MusicVolumeChannel + channels, out dummy))
                    channels++;

                return channels;
            }
        }

        public int InstrumentCount
        {
            get
            {
                var instruments = 0;
                float dummy;

                while (Bass.ChannelGetAttribute(Handle, ChannelAttribute.MusicVolumeInstrument + instruments, out dummy))
                    instruments++;

                return instruments;
            }
        }

        public override void Dispose() => Bass.MusicFree(Handle);
    }
}
