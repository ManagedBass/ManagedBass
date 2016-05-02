using System;

namespace ManagedBass.Midi
{
    [Flags]
    enum MidiEventsMode
    {
        Struct = 0, // MidiEvent structures
        Raw = 0x10000, // raw MIDI event data
        Sync = 0x1000000, // FLAG: trigger event syncs
        NoRunningStatus = 0x2000000 // FLAG: no running status
    }
}