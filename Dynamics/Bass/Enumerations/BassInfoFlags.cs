using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum BASSInfoFlags
    {
        /// <summary>
        /// None of the falgs are set
        /// </summary>
        None = 0,

        /// <summary>
        /// The device supports all sample rates between minrate and maxrate.
        /// </summary>
        ContinuousRate = 16,

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

        /// <summary>
        /// Mono samples are supported by hardware mixing.
        /// </summary>
        Mono = 256,

        /// <summary>
        /// Stereo samples are supported by hardware mixing.
        /// </summary>
        Stereo = 512,

        /// <summary>
        /// 8-bit samples are supported by hardware mixing.
        /// </summary>
        Secondary8Bit = 1024,

        /// <summary>
        /// 16-bit samples are supported by hardware mixing.
        /// </summary>
        Secondary16Bit = 2048,
    }
}