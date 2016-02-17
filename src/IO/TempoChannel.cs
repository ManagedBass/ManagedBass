using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Tempo and Pitch
    /// </summary>
    public class TempoChannel : Channel
    {
        Channel decoder;

        public TempoChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
			this.decoder = DecodingSource;

			if (!decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

			Handle = BassFx.TempoCreate(decoder.Handle, FlagGen(IsDecoder, Resolution));
        }

        public double Pitch
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Pitch); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Pitch, value); }
        }

        public double Tempo
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.Tempo); }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.Tempo, value); }
        }

        public override void Dispose()
        {
            base.Dispose();
            decoder = null;
        }
    }
}