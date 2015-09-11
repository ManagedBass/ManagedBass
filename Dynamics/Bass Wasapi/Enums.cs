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

    public enum WasapiNotificationType
    {
        // Summary:
        //     The device's status has changed, eg. it has been enabled or disabled. The
        //     new status is available from Un4seen.BassWasapi.BassWasapi.BASS_WASAPI_GetDeviceInfo(System.Int32,Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO).
        Change = 0,
        //
        // Summary:
        //     The device is now the default input device.
        BASS_WASAPI_NOTIFY_DEFOUTPUT = 1,
        //
        // Summary:
        //     The device is now the default output device.
        BASS_WASAPI_NOTIFY_DEFINPUT = 2,
    }

    [Flags]
    public enum WasapiInitFlags
    {
        // Summary:
        //     Init the device (endpoint) in shared mode.
        Shared = 0,
        //
        // Summary:
        //     Init the device (endpoint) in exclusive mode.
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
        //
        // Summary:
        //     32-bit floating-point.
        Float = 0,
        //
        // Summary:
        //     8-bit integer.
        Bit8 = 1,
        //
        // Summary:
        //     16-bit integer.
        Bit16 = 2,
        //
        // Summary:
        //     24-bit integer.
        Bit24 = 3,
        //
        // Summary:
        //     32-bit integer.
        Bit32 = 4,
    }

    public enum WasapiVolumeTypes
    {
        // Summary:
        //     Use the device volume.
        Device = 0,
        //
        // Summary:
        //     Logarithmic curve.
        LogaritmicCurve = 0,
        //
        // Summary:
        //     Linear curve.
        LinearCurve = 1,
        //
        // Summary:
        //     Windows' hybrid curve.
        WindowsHybridCurve = 2,
        //
        // Summary:
        //     Use the session volume.
        Session = 8,
    }

    [Flags]
    public enum WasapiDeviceType
    {
        // Summary:
        //     A network device.
        BASS_WASAPI_TYPE_NETWORKDEVICE = 0,
        //
        // Summary:
        //     A speakers device.
        BASS_WASAPI_TYPE_SPEAKERS = 1,
        //
        // Summary:
        //     A line level device.
        BASS_WASAPI_TYPE_LINELEVEL = 2,
        //
        // Summary:
        //     A headphone device.
        BASS_WASAPI_TYPE_HEADPHONES = 3,
        //
        // Summary:
        //     A microphone device.
        BASS_WASAPI_TYPE_MICROPHONE = 4,
        //
        // Summary:
        //     A headset device.
        BASS_WASAPI_TYPE_HEADSET = 5,
        //
        // Summary:
        //     A handset device.
        BASS_WASAPI_TYPE_HANDSET = 6,
        //
        // Summary:
        //     A digital device.
        BASS_WASAPI_TYPE_DIGITAL = 7,
        //
        // Summary:
        //     A S/PDIF device.
        BASS_WASAPI_TYPE_SPDIF = 8,
        //
        // Summary:
        //     A HDMI device.
        BASS_WASAPI_TYPE_HDMI = 9,
        //
        // Summary:
        //     An unknown device.
        BASS_WASAPI_TYPE_UNKNOWN = 10,
    }
}