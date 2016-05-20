#if WINDOWS
namespace ManagedBass.Enc
{
    public class LameMp3Encoder : CommandLineEncoder
    {
        string _inputFile;

        public string OutputFile { get; set; }

        public LameMp3Encoder(int Channel) : base(Channel) { }

        public LameMp3Encoder(PCMFormat Format)
            : base(GetDummyChannel(Format)) { }
        
        public override string OutputFileExtension => "mp3";
        public override ChannelType OutputType => ChannelType.MP3;

        protected override string CommandLineArgs => "lame " + 
                                                     $"{(_inputFile == null ? "-" : $"\"{_inputFile}\"")}" +
                                                     $"{(OutputFile == null ? "-" : $"\"{OutputFile}\"")}";

        protected override EncodeFlags EncodeFlags => EncodeFlags.NoHeader | EncodeFlags.ConvertFloatTo32Bit;
    }
}
#endif