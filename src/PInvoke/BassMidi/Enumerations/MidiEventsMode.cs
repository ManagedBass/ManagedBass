using System;

namespace ManagedBass.Midi
{
    [Flags]
    public enum MidiEventsMode
    {
        /// <summary>
        /// Trigger event syncs
        /// </summary>
        Sync = 0x1000000,

        /// <summary>
        /// No running status
        /// </summary>
        NoRunningStatus = 0x2000000
    }
}