namespace ManagedBass.Midi
{
    /// <summary>
    /// Streams MIDI from file or memory. Requires: BassMidi.dll.
    /// </summary>
    public sealed class Midi : Channel
    {
        public Midi(string FilePath, int Offset = 0, int Length = 0, int Frequency = 44100, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassMidi.CreateStream(FilePath, Offset, Length, FlagGen(IsDecoder, Resolution), Frequency);
        }

        public Midi(byte[] Memory, int Offset, int Length, int Frequency = 44100, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassMidi.CreateStream(Memory, Offset, Length, FlagGen(IsDecoder, Resolution), Frequency);
        }

        public Midi(int Channels, int Frequency = 44100, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassMidi.CreateStream(Channels, FlagGen(IsDecoder, Resolution), Frequency);
        }

        public Midi(MidiEvent[] Events, int ppqn, int Frequency = 44100, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassMidi.CreateStream(Events, ppqn, FlagGen(IsDecoder, Resolution), Frequency);
        }
        
        public int TrackCount
        {
            get
            {
                var tracks = 0;
                float dummy;

                while (Bass.ChannelGetAttribute(Handle, ChannelAttribute.MidiTrackVolume + tracks, out dummy))
                    tracks++;

                return tracks;
            }
        }
    }
}
