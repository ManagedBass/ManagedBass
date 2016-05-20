namespace ManagedBass.Midi
{
    /// <summary>
    /// Streams MIDI from file or memory. Requires: BassMidi.dll.
    /// </summary>
    public sealed class Midi : Channel
    {
        public Midi(string FilePath, int Offset = 0, int Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = BassMidi.CreateStream(FilePath, Offset, Length, Flags, Frequency);
        }

        public Midi(byte[] Memory, int Offset, int Length, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = BassMidi.CreateStream(Memory, Offset, Length, Flags, Frequency);
        }

        public Midi(int Channels, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = BassMidi.CreateStream(Channels, Flags, Frequency);
        }

        public Midi(MidiEvent[] Events, int ppqn, BassFlags Flags = BassFlags.Default, int Frequency = 0)
        {
            Handle = BassMidi.CreateStream(Events, ppqn, Flags, Frequency);
        }
        
        /// <summary>
        /// Gets the Number of Tracks.
        /// </summary>
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
