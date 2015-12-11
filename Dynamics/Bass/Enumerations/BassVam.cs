using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum BASSVam
    {
        /// <summary>
        /// Play the sample in hardware. If no hardware voices are available then the "play" call will fail
        /// </summary>
        Hardware = 1,

        /// <summary>
        /// Play the sample in software (ie. non-accelerated). No other VAM flags may be used together with this flag.        
        /// </summary>
        Software = 2,

        /// <summary>
        /// If there are no free hardware voices, the buffer to be terminated will be the one with the least time left to play.
        /// </summary>
        TerminateTime = 4,

        /// <summary>
        /// If there are no free hardware voices, the buffer to be terminated will be
        /// one that was loaded/created with the BASS_SAMPLE_MUTEMAX flag and is beyond
        /// it's max distance. If there are no buffers that match this criteria, then
        /// the "play" call will fail.
        /// </summary>
        TerminateDistance = 8,

        /// <summary>
        /// If there are no free hardware voices, the buffer to be terminated will be the one with the lowest priority.
        /// </summary>
        TerminatePriority = 16,
    }
}