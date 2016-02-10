namespace ManagedBass.Dynamics
{
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