namespace ManagedBass.Enc
{
    public class PCMEncoder : BassEncEncoder
    {
        public string FileName { get; }

        readonly int _channel;

        public PCMEncoder(string FileName, int Channel)
        {
            this.FileName = FileName;
            _channel = Channel;

            InputFormat = PCMFormat.FromChannel(Channel);
        }

        public PCMEncoder(string FileName, PCMFormat Format)
            : this(FileName, GetDummyChannel(Format)) { }

        public override string OutputFileExtension => "wav";
        public override ChannelType OutputType => ChannelType.Wave;

        public override int OnStart() => BassEnc.EncodeStart(_channel, FileName, EncodeFlags.PCM, default(EncodeProcedure));

        public override PCMFormat InputFormat { get; }
    }
}