using System;

namespace ManagedBass.Wasapi
{
    /// <summary>
	/// BassWasapi initialization flags to be used with <see cref="BassWasapi.Init" />.
	/// </summary>
	[Flags]
    public enum WasapiInitFlags
    {
        /// <summary>
        /// Init the device (endpoint) in shared mode.
        /// </summary>
        Shared = 0,

        /// <summary>
        /// Init the device (endpoint) in exclusive mode.
        /// </summary>
        Exclusive = 1,
        
        /// <summary>
        /// Automatically choose another sample format if the specified format is not supported.
        /// If possible, a higher sample rate than freq will be used, rather than a lower one.
        /// </summary>
        AutoFormat = 2,
        
        /// <summary>
        /// Enable double buffering, for use by <see cref="BassWasapi.GetData(IntPtr,int)"/> and <see cref="BassWasapi.GetLevel()"/>. 
        /// This requires the BASS "no sound" device to have been initilized, via <see cref="Bass.Init"/>.
        /// Internally, a BASS stream is used for that, so the usual <see cref="DataFlags"/> flags are supported.
        /// </summary>
        Buffer = 4,
        
        /// <summary>
        /// Enables the event-driven WASAPI system.
        /// It is only supported when a <see cref="WasapiProcedure"/> function is provided, ie. not when using <see cref="BassWasapi.PutData(IntPtr,int)"/>.
        /// When used with shared mode, the User-provided 'Buffer' and 'period' lengths are ignored 
        /// and WASAPI decides what Buffer to use (<see cref="BassWasapi.Info"/> can be used to check that).
        /// </summary>
        EventDriven = 16
    }
}