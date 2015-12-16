﻿using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum ACMFormatFlags
    {
        /// <summary>
        /// Unicode (16-bit characters) option.
        /// </summary>
        Unicode = -2147483648,

        /// <summary>
        /// No ACM.
        /// </summary>
        NoACM = 0,

        /// <summary>
        /// Use the format as default selection.
        /// </summary>
        Default = 1,

        /// <summary>
        /// Only list formats with same sample rate as the source channel.
        /// </summary>
        SameSampleRate = 2,

        /// <summary>
        /// Only list formats with same number of channels (eg. mono/stereo).
        /// </summary>
        SameChannels = 4,

        /// <summary>
        /// Suggest a format (HIWORD=format tag - use one of the WAVEFormatTag flags).
        /// </summary>
        Suggest = 8,
    }
}