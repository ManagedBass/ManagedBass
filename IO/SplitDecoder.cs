using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class SplitDecoder : Channel
    {
        public IDecoder Source { get; set; }

        public SplitDecoder(IDecoder Source, bool IsDecoder = false, Resolution BufferKind = Resolution.Short)
            : base(IsDecoder, BufferKind)
        {
            this.Source = Source;

            var flags = BufferKind.ToBassFlag();
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = BassMix.CreateSplitStream(Source.Handle, flags, null);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public override void Dispose()
        {
            base.Dispose();
            Source = null;
        }
    }
}