#if WINDOWS || LINUX || __MAC__
namespace ManagedBass.Enc
{
    public abstract class CommandLineEncoder : BassEncEncoder
    {
        protected readonly int _channel;

        protected CommandLineEncoder(int Channel)
        {
            _channel = Channel;
        }

        protected abstract string CommandLineArgs { get; }

        protected abstract EncodeFlags EncodeFlags { get; }

        public override int OnStart() => BassEnc.EncodeStart(_channel, CommandLineArgs, EncodeFlags, default(EncodeProcedure));
    }
}
#endif