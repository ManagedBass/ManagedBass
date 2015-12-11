using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class ReversePlayer : AdvancedPlayable
    {
        Decoder decoder;

        public ReversePlayer(Decoder decoder, BufferKind BufferKind = BufferKind.Short, double DecodingBlockLength = 2)
            : base(BufferKind)
        {
            this.decoder = decoder;

            Handle = BassFx.ReverseCreate(decoder.Handle, (float)DecodingBlockLength, BufferKind.ToBassFlag());
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