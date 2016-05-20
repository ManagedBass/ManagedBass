#if WINDOWS || LINUX || __MAC__
namespace ManagedBass.Enc
{
    public abstract class CommandLineEncoder : BassEncEncoder
    {
        protected readonly int _channel;

        protected CommandLineEncoder(int Channel)
        {
            _channel = Channel;

            var info = Bass.ChannelGetInfo(_channel);
            InputFormat = new PCMFormat(info.Frequency, info.Channels, info.Resolution);
        }

        protected abstract string CommandLineArgs { get; }

        protected abstract EncodeFlags EncodeFlags { get; }

        public override int OnStart() => BassEnc.EncodeStart(_channel, CommandLineArgs, EncodeFlags, default(EncodeProcedure));

        public override PCMFormat InputFormat { get; }
    }
}
#endif