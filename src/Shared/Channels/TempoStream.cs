using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Tempo and Pitch
    /// </summary>
    public sealed class TempoStream : Channel
    {
        Channel _decoder;

        public TempoStream(Channel DecodingSource, BassFlags Flags = BassFlags.Default)
        {
			_decoder = DecodingSource;

			if (!_decoder.Info.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

			Handle = BassFx.TempoCreate(_decoder.Handle, Flags);
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