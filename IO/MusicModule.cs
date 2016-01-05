using System.Runtime.InteropServices;
using ManagedBass.Dynamics;
using ManagedBass.Effects;

namespace ManagedBass
{
    public class MusicModule : Channel
    {
        public MusicModule(string FilePath, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.LoadMusic(FilePath, 0, 0, flags, 0);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        MusicModule(byte[] Memory, int Length, bool IsDecoder = false)
            : base(IsDecoder, Resolution.Byte)
        {
            var flags = BassFlags.Byte;
            if (IsDecoder) flags |= BassFlags.Decode;

            GCHandle GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.LoadMusic(GCPin.AddrOfPinnedObject(), 0, Length, flags);
            GCPin.Free();

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public MusicModule(float[] Memory, int Length, bool IsDecoder = false)
            : base(IsDecoder, Resolution.Float)
        {
            var flags = BassFlags.Float;
            if (IsDecoder) flags |= BassFlags.Decode;

            GCHandle GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);
            Handle = Bass.LoadMusic(GCPin.AddrOfPinnedObject(), 0, Length, flags);
            GCPin.Free();

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public string Title { get { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicName)); } }

        public string Message { get { return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicMessage)); } }

        public string Instrument(int Index)
        {
            return Marshal.PtrToStringAnsi(Bass.ChannelGetTags(Handle, TagType.MusicInstrument + Index));
        }

        public override void Dispose() { Bass.MusicFree(Handle); }
    }
}
