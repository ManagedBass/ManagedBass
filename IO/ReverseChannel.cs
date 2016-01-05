using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class ReverseChannel : Channel
    {
        IDecoder decoder;

        public ReverseChannel(IDecoder decoder, bool IsDecoder = false, Resolution BufferKind = Resolution.Short, double DecodingBlockLength = 2)
            : base(IsDecoder, BufferKind)
        {
            this.decoder = decoder;

            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassFx.ReverseCreate(decoder.Handle, (float)DecodingBlockLength, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public bool Reverse
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        public override void Dispose()
        {
            base.Dispose();
            decoder = null;
        }
    }
}