using System;

namespace ManagedBass.Mix
{
    public sealed class Split : Channel
    {
        Channel _source;

        public Split(Channel DecodingSource, BassFlags Flags = BassFlags.Default, int[] ChannelMap = null)
        {
            if (!DecodingSource.Info.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

            _source = DecodingSource;

            Handle = BassMix.CreateSplitStream(DecodingSource.Handle, Flags, ChannelMap);
        }

        public override void Dispose()
        {
            base.Dispose();
            _source = null;
        }
    }
}