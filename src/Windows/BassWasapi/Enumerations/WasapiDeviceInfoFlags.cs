using System;

namespace ManagedBass.Wasapi
{
    [Flags]
    enum WasapiDeviceInfoFlags
    {
        /// <summary>
        /// Unknown flags. 
        /// e.g. the WASAPI device is not present.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The WASAPI device is enabled (active).
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// The WASAPI device is the default device.
        /// </summary>
        Default = 2,

        /// <summary>
        /// The WASAPI device is initialized.
        /// </summary>
        Initialized = 4,

        /// <summary>
        /// The WASAPI device is a loopback device.
        /// </summary>
        Loopback = 8,

        /// <summary>
        /// The WASAPI device is an Input (capture) device.
        /// </summary>
        Input = 16,

        /// <summary>
        /// The WASAPI device is unplugged.
        /// </summary>
        Unplugged = 32,

        /// <summary>
        /// The WASAPI device is disabled.
        /// </summary>
        Disabled = 64
    }
}