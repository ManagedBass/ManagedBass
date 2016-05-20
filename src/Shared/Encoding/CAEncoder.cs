using System;

#if __MAC__ || __IOS__
namespace ManagedBass.Enc
{
    class CAEncoder : BassEncEncoder
    {
        readonly int _channel;
        readonly Func<int> _starter;

        public CAEncoder(string FileName, int Channel)
        {
            _channel = Channel;
            InputFormat = PCMFormat.FromChannel(Channel);
        }

        public override string OutputFileExtension { get; }
        public override ChannelType OutputType { get; }

        public override int OnStart()
        {
            throw new System.NotImplementedException();
        }

        public override PCMFormat InputFormat { get; }
    }
}
#endif