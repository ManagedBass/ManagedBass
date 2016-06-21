namespace ManagedBass
{
    public class FlagCollection
    {
        readonly int _channel;

        public FlagCollection(Channel Channel)
        {
            _channel = Channel.Handle;
        }

        public BassFlags Flags => Bass.ChannelFlags(_channel, 0, 0);

        public bool Has(BassFlags Flag) => Bass.ChannelHasFlag(_channel, Flag);

        public bool Add(BassFlags Flag) => Bass.ChannelAddFlag(_channel, Flag);

        public bool Remove(BassFlags Flag) => Bass.ChannelRemoveFlag(_channel, Flag);
    }
}