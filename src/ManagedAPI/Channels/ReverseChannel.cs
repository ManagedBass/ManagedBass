using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Playback Direction.
    /// Requires BassFx.
    /// </summary>
    public class ReverseChannel : Channel
    {
        Channel decoder;

        public ReverseChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short, double DecodingBlockLength = 2)
        {
			this.decoder = DecodingSource;

			if (!decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");
			
			Handle = BassFx.ReverseCreate(decoder.Handle, (float)DecodingBlockLength, FlagGen(IsDecoder, Resolution));
        }

        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        public override void Dispose()
        {
            base.Dispose();
            decoder = null;
        }
    }
}