namespace ManagedBass.Dynamics
{
    public enum PlaybackState
    {
        /// <summary>
        /// The channel is not active, or handle is not a valid channel.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// The channel is playing (or recording).
        /// </summary>
        Playing = 1,

        /// <summary>
        /// Playback of the stream has been stalled due to there not being enough sample
        /// data to continue playing. The playback will automatically resume once there's
        /// sufficient data to do so.
        /// </summary>
        Stalled = 2,

        /// <summary>
        /// The channel is paused.
        /// </summary>
        Paused = 3,
    }
}