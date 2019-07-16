namespace ManagedBass.Asio
{
    public enum AsioChannelGetLevelFlags
    {
        /// <summary>
        /// Applied to the channel handle when requesting ChannelGetLevel() to obtain RMS values instead of peak values.
        /// </summary>
        Rms = 0x1000000
    }
}
