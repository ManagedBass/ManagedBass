using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum InputFlags
    {
        /// <summary>
        /// No input flag change.
        /// </summary>
        None = 0,

        /// <summary>
        /// Disable the input. 
        /// This flag can't be used when the device supports only one input at a time.
        /// </summary>
        Off = 65536,

        /// <summary>
        /// Enable the input. 
        /// If the device only allows one input at a time, then any previously enabled input will be disabled by this.
        /// </summary>
        On = 131072,
    }
}