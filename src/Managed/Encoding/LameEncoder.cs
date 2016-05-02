#if WINDOWS
namespace ManagedBass.Enc
{
    /// <summary>
    /// The general Lame encoding mode (stereo, joint stereo, mono etc.).
    /// </summary>
    public enum LameMode
    {
        Default,
        Stereo,
        JointStereo,
        ForcedJointStereo,
        Mono,
        DualMono
    }

    /// <summary>
    /// Encoding quality (default is Quality).
    /// </summary>
    public enum LameQuality
    {
        Quality0,
        Quality1,
        Quality2,
        Quality3,
        Quality4,
        Quality5,
        Quality6,
        Quality7,
        Quality8,
        Quality9,
        None,
        Speed,
        Quality
    }

    public class LameMp3Encoder : CommandLineEncoder
    {
        public LameMp3Encoder(int Channel) : base(Channel) { }

        public LameMp3Encoder(PCMFormat Format)
            : base(GetDummyChannel(Format)) { }
        
        public override string OutputFileExtension => "mp3";
        public override ChannelType OutputType => ChannelType.MP3;
        protected override string CommandLineArgs { get; }
        protected override EncodeFlags EncodeFlags { get; }
    }
}
#endif