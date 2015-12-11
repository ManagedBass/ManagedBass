using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum WasapiDeviceInfoFlags : uint
    {
        Unknown = 0,
        Enabled = 1,
        Default = 2,
        Initialized = 4,
        Loopback = 8,
        Input = 16,
        Unplugged = 32,
        Disabled = 64,
    }

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

    public enum WasapiFormat
    {
        Unknown = -1,
        
        /// <summary>
        /// 32-bit floating-point.
        /// </summary>
        Float = 0,
        
        /// <summary>
        /// 8-bit integer.
        /// </summary>
        Bit8 = 1,
        
        /// <summary>
        /// 16-bit integer.
        /// </summary>
        Bit16 = 2,
        
        /// <summary>
        /// 24-bit integer.
        /// </summary>
        Bit24 = 3,
        
        /// <summary>
        /// 32-bit integer.
        /// </summary>
        Bit32 = 4,
    }

    public enum WasapiVolumeTypes
    {
        /// <summary>
        /// Use the device volume.
        /// </summary>
        Device = 0,
        
        /// <summary>
        /// Logarithmic curve.
        /// </summary>
        LogaritmicCurve = 0,
        
        /// <summary>
        /// Linear curve.
        /// </summary>
        LinearCurve = 1,
        
        /// <summary>
        /// Windows' hybrid curve.
        /// </summary>
        WindowsHybridCurve = 2,
        
        /// <summary>
        /// Use the session volume.
        /// </summary>
        Session = 8,
    }

    [Flags]
    public enum WasapiDeviceType
    {
        /// <summary>
        /// A network device.
        /// </summary>
        NetworkDevice = 0,
        
        /// <summary>
        /// A speakers device.
        /// </summary>
        Speakers = 1,
        
        /// <summary>
        /// A line level device.
        /// </summary>
        LineLevel = 2,
        
        /// <summary>
        /// A headphone device.
        /// </summary>
        Headphones = 3,
        
        /// <summary>
        /// A microphone device.
        /// </summary>
        Microphone = 4,
        
        /// <summary>
        /// A headset device.
        /// </summary>
        Headset = 5,
        
        /// <summary>
        /// A handset device.
        /// </summary>
        Handset = 6,
        
        /// <summary>
        /// A digital device.
        /// </summary>
        Digital = 7,
        
        /// <summary>
        /// A S/PDIF device.
        /// </summary>
        SPDIF = 8,
        
        /// <summary>
        /// A HDMI device.
        /// </summary>
        HDMI = 9,
        
        /// <summary>
        /// An unknown device.
        /// </summary>
        Unknown = 10,
    }
}