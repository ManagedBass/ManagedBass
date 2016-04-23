using System;

namespace ManagedBass.Wasapi
{
    [Flags]
    public enum WasapiVolumeTypes
    {
        /// <summary>
        /// Use the device volume.
        /// </summary>
        Device = 0,

        /// <summary>
        /// Logarithmic curve.
        /// </summary>
        LogaritmicCurve = 0,

        /// <summary>
        /// Linear curve.
        /// </summary>
        LinearCurve = 1,

        /// <summary>
        /// Windows' hybrid curve.
        /// </summary>
        WindowsHybridCurve = 2,

        /// <summary>
        /// Use the session volume.
        /// </summary>
        Session = 8
    }
}