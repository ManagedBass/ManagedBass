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
        //
        // Summary:
        //     Automatically choose another sample format if the specified format is not
        //     supported.  If possible, a higher sample rate than freq will be used, rather
        //     than a lower one.
        AutoFormat = 2,
        //
        // Summary:
        //     Enable double buffering, for use by Un4seen.BassWasapi.BassWasapi.BASS_WASAPI_GetData(System.IntPtr,System.Int32)
        //     and Un4seen.BassWasapi.BassWasapi.BASS_WASAPI_GetLevel(). This requires the
        //     BASS "no sound" device to have been initilized, via Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr).
        //     Internally, a BASS stream is used for that, so the usual BASS_DATA_xxx flags
        //     are supported.
        Buffer = 4,
        //
        // Summary:
        //     Enables the event-driven WASAPI system.
        //     It is only supported when a WASAPIPROC function is provided, ie. not when
        //     using Un4seen.BassWasapi.BassWasapi.BASS_WASAPI_PutData(System.IntPtr,System.Int32).
        //     When used with shared mode, the user-provided 'buffer' and 'period' lengths
        //     are ignored and WASAPI decides what buffer to use (Un4seen.BassWasapi.BassWasapi.BASS_WASAPI_GetInfo(Un4seen.BassWasapi.BASS_WASAPI_INFO)
        //     can be used to check that).
        EventDriven = 16,
    }
}