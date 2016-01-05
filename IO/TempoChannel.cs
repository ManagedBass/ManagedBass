using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class TempoChannel : Channel
    {
        IDecoder decoder;

        public TempoChannel(IDecoder decoder, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            this.decoder = decoder;

            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassFx.TempoCreate(decoder.Handle, flags);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
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