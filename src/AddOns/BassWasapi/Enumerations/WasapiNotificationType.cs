namespace ManagedBass.Wasapi
{
    /// <summary>
    /// BassWasapi Notification type to be used with <see cref="BassWasapi.SetNotify"/>.
    /// </summary>
    public enum WasapiNotificationType
    {
        /// <summary>
        /// The device has been enabled.
        /// </summary>
        Enabled,

        /// <summary>
        /// The device has been disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// The device is now the default Input device.
        /// </summary>
        DefaultOutput,

        /// <summary>
        /// The device is now the default output device.
        /// </summary>
        DefaultInput
    }
}