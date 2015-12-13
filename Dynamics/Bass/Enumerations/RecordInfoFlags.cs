﻿using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum RecordInfoFlags
    {
        /// <summary>
        /// None of the flags is set
        /// </summary>
        None = 0,

        /// <summary>
        /// The device's drivers do NOT have DirectSound support, so it is being emulated.
        /// Updated drivers should be installed.
        /// </summary>
        EmulatedDrivers = 32,

        /// <summary>
        /// The device driver has been certified by Microsoft. 
        /// This flag is always set on WDM drivers.
        /// </summary>
        Certified = 64,
    }
}