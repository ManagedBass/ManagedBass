namespace ManagedBass.Dynamics
{
    // TODO: Check with the new Definition
    public enum WasapiNotificationType
    {
        /// <summary>
        /// The device's status has changed, eg. it has been enabled or disabled. 
        /// The new status is available from BassWasapi.GetDeviceInfo().
        /// </summary>
        Change = 0,

        /// <summary>
        /// The device is now the default input device.
        /// </summary>
        DefaultOutput = 1,

        /// <summary>
        /// The device is now the default output device.
        /// </summary>
        DefaultInput = 2,
    }
}