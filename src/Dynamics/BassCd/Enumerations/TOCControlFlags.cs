using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// The <see cref="TOCTrack.Control" /> flags.
    /// </summary>
    [Flags]
    public enum TOCControlFlags : byte
    {
        /// <summary>
        /// No flag defined.
        /// </summary>
        None = 0,

        /// <summary>
        /// Pre-emphasis of 50/15 µs.
        /// </summary>
        PreEmphasis = 1,

        /// <summary>
        /// Digital copy permitted.
        /// </summary>
        DigitalCopyPermitted = 2,

        /// <summary>
        /// Data track.
        /// </summary>
        DataTrack = 4
    }
}