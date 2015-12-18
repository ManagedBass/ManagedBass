using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class SplitDecoder : Decoder
    {
        public Decoder Source { get; set; }

        public SplitDecoder(Decoder Source, Resolution BufferKind = Resolution.Short)
            : base(BufferKind)
        {
            this.Source = Source;

            Handle = BassMix.CreateSplitStream(Source.Handle, BassFlags.Decode | BufferKind.ToBassFlag(), null);
        }

        public override void Dispose()
        {
            base.Dispose();
            Source = null;
        }
    }
}