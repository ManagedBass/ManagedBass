using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Tempo and Pitch
    /// </summary>
    public sealed class TempoChannel : Channel
    {
        Channel _decoder;

        public TempoChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
			_decoder = DecodingSource;

			if (!_decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

			Handle = BassFx.TempoCreate(_decoder.Handle, FlagGen(IsDecoder, Resolution));
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
            _decoder = null;
        }
    }
}