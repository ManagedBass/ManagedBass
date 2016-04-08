using System;

namespace ManagedBass.Mix
{
    public sealed class SplitChannel : Channel
    {
        Channel _source;

        public SplitChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            if (!DecodingSource.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

            _source = DecodingSource;

            Handle = BassMix.CreateSplitStream(DecodingSource.Handle, FlagGen(IsDecoder, Resolution), null);
        }

        public override void Dispose()
        {
            base.Dispose();
            _source = null;
        }
    }
}