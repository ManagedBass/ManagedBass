using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum LevelRetrievalFlags
    {
        /// <summary>
        /// Retrieves mono levels
        /// </summary>
        All = 0,

        /// <summary>
        /// Retrieves mono levels
        /// </summary>
        Mono = 1,

        /// <summary>
        /// Retrieves stereo levels
        /// </summary>
        Stereo = 2,

        /// <summary>
        /// Optional Flag: If set it returns RMS levels instead of peak leavels
        /// </summary>
        RMS = 4,
    }
}