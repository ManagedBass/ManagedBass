namespace ManagedBass.Dynamics
{
    /// <summary>
    /// BassAsio notify values as used in the <see cref="AsioNotifyProcedure" />.
    /// </summary>
    public enum AsioNotify
    {
        /// <summary>
        /// Sample Rate Change
        /// </summary>
        Rate = 1,

        /// <summary>
        /// Reset (Reinitialization) request
        /// </summary>
        Reset = 2
    }
}