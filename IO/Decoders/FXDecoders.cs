using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class ReverseDecoder : Decoder
    {
        Decoder decoder;

        public ReverseDecoder(Decoder decoder, BufferKind BufferKind = BufferKind.Short, double DecodingBlockLength = 2)
            : base(BufferKind)
        {
            this.decoder = decoder;

            Handle = BassFx.ReverseCreate(decoder.Handle, (float)DecodingBlockLength, BassFlags.Decode | BufferKind.ToBassFlag());
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

    public class TempoDecoder : Decoder
    {
        Decoder decoder;

        public TempoDecoder(Decoder decoder, BufferKind BufferKind = BufferKind.Short)
            : base(BufferKind)
        {
            this.decoder = decoder;

            Handle = BassFx.TempoCreate(decoder.Handle, BassFlags.Float | BufferKind.ToBassFlag());
        }

        public double Pitch
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Pitch); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Pitch, value); }
        }

        public double Tempo
        {
            get { return Bass.GetChannelAttribute(Handle, ChannelAttribute.Tempo); }
            set { Bass.SetChannelAttribute(Handle, ChannelAttribute.Tempo, value); }
        }

        public override void Dispose()
        {
            base.Dispose();
            decoder = null;
        }
    }
}