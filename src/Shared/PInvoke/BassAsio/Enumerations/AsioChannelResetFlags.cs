#if WINDOWS
using System;

namespace ManagedBass.Asio
{
    /// <summary>
    /// BassAsio attributes to be used when to reset a channel with <see cref="BassAsio.ChannelReset" />.
    /// </summary>
    [Flags]
    public enum AsioChannelResetFlags
    {
        /// <summary>
        /// Disable Channel
        /// </summary>
        Enable = 1,

        /// <summary>
        /// Unjoin Channel
        /// </summary>
        Join = 2,

        /// <summary>
        /// Unpause Channel
        /// </summary>
        Pause = 4,

        /// <summary>
        /// Reset sample format to native format
        /// </summary>
        Format = 8,

        /// <summary>
        /// Reset sample rate to device rate
        /// </summary>
        Rate = 16,

        /// <summary>
        /// Reset Volume to 1.0
        /// </summary>
        Volume = 32
    }
}
#endif