namespace ManagedBass.Enc
{
    public sealed class IceCast : Cast
    {
        public IceCast(int Encoder, ChannelType OutputType, int Bitrate = 0) : base(Encoder, OutputType, Bitrate) { }
        
        protected override string _server => $"{ServerAddress}:{ServerPort}{MountPoint}";
        protected override string _headers => Quality == null ? null : $"ice-bitrate: {Quality}\r\n";
        
        public string MountPoint { get; set; }
        
        public string Quality { get; set; }
    }
}