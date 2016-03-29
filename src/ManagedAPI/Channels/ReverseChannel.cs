using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Playback Direction.
    /// Requires BassFx.
    /// </summary>
    public sealed class ReverseChannel : Channel
    {
        Channel _decoder;

        public ReverseChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short, double DecodingBlockLength = 2)
        {
			_decoder = DecodingSource;

			if (!_decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");
			
			Handle = BassFx.ReverseCreate(_decoder.Handle, (float)DecodingBlockLength, FlagGen(IsDecoder, Resolution));
        }

        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        public override void Dispose()
        {
            base.Dispose();
            _decoder = null;
        }
    }
}