using static System.Runtime.InteropServices.Marshal;

namespace ManagedBass
{
    /// <summary>
    /// Streams a MOD music file.
    /// </summary>
    public sealed class Music : Channel
    {
        public Music(string FilePath, int Offset = 0, int Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = Bass.MusicLoad(FilePath, Offset, Length, Flags, Frequency);
        }
        
        public Music(byte[] Memory, int Length, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = Bass.MusicLoad(Memory, 0, Length, Flags, Frequency);
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
