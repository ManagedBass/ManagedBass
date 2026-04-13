namespace ManagedBass.Asio
{
    /// <summary>
    /// BassAsio add-on: Flags for the <see cref="BassAsio.ChannelGetLevel"/> method to control level measurement behavior.
    /// </summary>
    /// <remarks>
    /// These flags can be combined with the channel handle when requesting level measurements
    /// to modify how the levels are calculated.
    /// </remarks>
    public enum AsioChannelGetLevelFlags
    {
        /// <summary>
        /// Applied to the channel handle when requesting <see cref="BassAsio.ChannelGetLevel"/> to obtain RMS values instead of peak values.
        /// </summary>
        Rms = 0x1000000
    }
}
