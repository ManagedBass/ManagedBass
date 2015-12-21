using System;

namespace ManagedBass.Dynamics
{
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
        /// Enable double buffering, for use by BassWasapi.GetData() and BassWasapi.GetLevel(). 
        /// This requires the BASS "no sound" device to have been initilized, via Bass.Init().
        /// Internally, a BASS stream is used for that, so the usual BASS_DATA_xxx flags are supported.
        /// </summary>
        Buffer = 4,
        
        /// <summary>
        /// Enables the event-driven WASAPI system.
        /// It is only supported when a WASAPIPROC function is provided, ie. not when using BassWasapi.PutData().
        /// When used with shared mode, the user-provided 'buffer' and 'period' lengths are ignored 
        /// and WASAPI decides what buffer to use (BassWasapi.GetInfo() can be used to check that).
        /// </summary>
        EventDriven = 16,
    }
}