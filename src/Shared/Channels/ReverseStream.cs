using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Playback Direction.
    /// Requires BassFx.
    /// </summary>
    public sealed class ReverseStream : Channel
    {
        Channel _decoder;

        public ReverseStream(Channel DecodingSource, double DecodingBlockLength = 2, BassFlags Flags = BassFlags.Default)
        {
			_decoder = DecodingSource;

			if (!_decoder.Info.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");
			
			Handle = BassFx.ReverseCreate(_decoder.Handle, (float)DecodingBlockLength, Flags);
        }

        /// <summary>
        /// Gets or Sets the Playback Position.
        /// </summary>
        public bool Reverse
        {
            get { return Bass.ChannelGetAttribute(Handle, ChannelAttribute.ReverseDirection) < 0; }
            set { Bass.ChannelSetAttribute(Handle, ChannelAttribute.ReverseDirection, value ? -1 : 1); }
        }

        /// <summary>
        /// Frees all resources used by this instance.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _decoder = null;
        }
    }
}