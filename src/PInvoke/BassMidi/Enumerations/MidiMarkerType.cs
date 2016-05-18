using System;

namespace ManagedBass.Midi
{
	/// <summary>
	/// The Marker type, to be used with <see cref="BassMidi.StreamGetMark" />.
	/// </summary>
    [Flags]
    public enum MidiMarkerType
    {
        Marker,
        CuePoint,
        Lyric,
        Text,
        TimeSignature,
        KeySignature,
        Copyright,
        TrackName,
        InstrumentName,
        Tick = 0x10000 // FLAG: get position in ticks (otherwise bytes)
    }
}