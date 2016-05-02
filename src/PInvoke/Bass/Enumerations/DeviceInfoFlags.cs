using System;

namespace ManagedBass
{
    [Flags]
    enum DeviceInfoFlags
    {
        /// <summary>
        /// Bitmask to identify the device Type.
        /// </summary>
        TypeMask = -16777216,

        /// <summary>
        /// The device is not enabled and not initialized.
        /// </summary>
        None = 0,

        /// <summary>
        /// The device is enabled.
        /// It will not be possible to initialize the device if this flag is not present.
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// The device is the system default.
        /// </summary>
        Default = 2,

        /// <summary>
        /// The device is initialized, ie. <see cref="Bass.Init"/> or <see cref="Bass.RecordInit"/> has been called.
        /// </summary>
        Initialized = 4
    }
}
