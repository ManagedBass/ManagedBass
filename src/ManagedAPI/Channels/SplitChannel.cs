using System;

namespace ManagedBass.Mix
{
    public class SplitChannel : Channel
    {
        Channel Source;

        public SplitChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            if (!DecodingSource.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

            this.Source = DecodingSource;

            Handle = BassMix.CreateSplitStream(DecodingSource.Handle, FlagGen(IsDecoder, Resolution), null);
        }

        public override void Dispose()
        {
            base.Dispose();
            Source = null;
        }
    }
}