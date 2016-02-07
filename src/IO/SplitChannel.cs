using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    public class SplitChannel : Channel
    {
        public Channel Source { get; set; }

        public SplitChannel(Channel DecodingSource, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            if (!DecodingSource.Info.IsDecodingChannel)
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