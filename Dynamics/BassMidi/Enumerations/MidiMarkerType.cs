﻿using System;

namespace ManagedBass.Dynamics
{
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