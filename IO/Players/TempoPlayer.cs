using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class TempoPlayer : AdvancedPlayable
    {
        Decoder decoder;

        public TempoPlayer(Decoder decoder, Resolution BufferKind = Resolution.Short)
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