using System;

namespace ManagedBass.Mix
{
    public sealed class Splitter : Channel
    {
        Channel _source;

        public Splitter(Channel DecodingSource, BassFlags Flags = BassFlags.Default, int[] ChannelMap = null)
        {
            if (!DecodingSource.Info.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");

            _source = DecodingSource;

            Handle = BassMix.CreateSplitStream(DecodingSource.Handle, Flags, ChannelMap);
        }

        public void Reset() => BassMix.SplitStreamReset(Handle);

        public void Reset(int Offset) => BassMix.SplitStreamReset(Handle, Offset);

        public int AvailableData => BassMix.SplitStreamGetAvailable(Handle);

        public override void Dispose()
        {
            base.Dispose();
            _source = null;
        }
    }
}