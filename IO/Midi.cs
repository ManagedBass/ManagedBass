using ManagedBass.Dynamics;

namespace ManagedBass
{
    class Midi : Channel
    {
        public Midi(string FilePath, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassMidi.CreateStream(FilePath, 0, 0, flags, 0);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }
    }
}
