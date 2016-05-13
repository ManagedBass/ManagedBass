using System;

namespace ManagedBass.Midi
{
    [Flags]
    public enum MidiEventsMode
    {
        /// <summary>
        /// An array of <see cref="MidiEvent" /> structures (Default).
        /// </summary>
        Struct = 0,

        /// <summary>
        /// Raw MIDI event data, as would be sent to a MIDI device. 
		/// Running status is supported.
		/// To overcome the 16 channel limit, the event data's channel information can optionally be overridden by adding the new channel number to this parameter, where +1 = the 1st channel.
        /// </summary>
        Raw = 0x10000,

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