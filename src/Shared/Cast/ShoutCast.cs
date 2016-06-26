namespace ManagedBass.Enc
{
    public sealed class ShoutCast : Cast
    {
        public ShoutCast(int Encoder, ChannelType OutputType, int Bitrate = 0) : base(Encoder, OutputType, Bitrate) { }
        
        protected override string _server => string.IsNullOrEmpty(SID) ? $"{ServerAddress}:{ServerPort}" : $"{ServerAddress}:{ServerPort},{SID}";
        
        public string SID { get; set; }
    }
}