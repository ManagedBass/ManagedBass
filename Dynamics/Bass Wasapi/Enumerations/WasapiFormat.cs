﻿namespace ManagedBass.Dynamics
{
    public enum WasapiFormat
    {
        Unknown = -1,

        /// <summary>
        /// 32-bit floating-point.
        /// </summary>
        Float = 0,

        /// <summary>
        /// 8-bit integer.
        /// </summary>
        Bit8 = 1,

        /// <summary>
        /// 16-bit integer.
        /// </summary>
        Bit16 = 2,

        /// <summary>
        /// 24-bit integer.
        /// </summary>
        Bit24 = 3,

        /// <summary>
        /// 32-bit integer.
        /// </summary>
        Bit32 = 4,
    }
}