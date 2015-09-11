using ManagedBass.Dynamics;
using System;

namespace ManagedBass
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
            if (!DecodingSource.Info.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

            this.decoder = DecodingSource;

            Handle = BassFx.ReverseCreate(DecodingSource.Handle, (float)DecodingBlockLength, FlagGen(IsDecoder, Resolution));
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