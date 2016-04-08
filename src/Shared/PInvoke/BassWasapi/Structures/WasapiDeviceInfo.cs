#if WINDOWS
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Wasapi
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WasapiDeviceInfo
    {
        IntPtr name;
        IntPtr id;
        WasapiDeviceType type;
        WasapiDeviceInfoFlags flags;
        float minperiod;
        float defperiod;
        int mixfreq;
        int mixchans;

        /// <summary>
        /// The Type of the devices.
        /// </summary>
        public WasapiDeviceType Type => type;

        /// <summary>
        /// The minimum update period (in seconds) of the device.
        /// </summary>
        public double MinimumUpdatePeriod => minperiod;

        /// <summary>
        /// The default update period (in seconds) of the device.
        /// </summary>
        public double DefaultUpdatePeriod => defperiod;

        /// <summary>
        /// The shared-mode format mixers sample rate.
        /// </summary>
        public int MixFrequency => mixfreq;

        /// <summary>
        /// The shared-mode format mixers number of channels.
        /// </summary>
        public int MixChannels => mixchans;

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name => Marshal.PtrToStringAnsi(name);

        /// <summary>
        /// The ID of the driver being used.
        /// </summary>
        public string ID => Marshal.PtrToStringAnsi(id);

        /// <summary>
        /// The device is the system default device.
        /// </summary>
        public bool IsDefault => flags.HasFlag(WasapiDeviceInfoFlags.Default);

        public bool IsEnabled => flags.HasFlag(WasapiDeviceInfoFlags.Enabled);

        public bool IsInput => flags.HasFlag(WasapiDeviceInfoFlags.Input);

        public bool IsLoopback => flags.HasFlag(WasapiDeviceInfoFlags.Loopback);

        public bool IsInitialized => flags.HasFlag(WasapiDeviceInfoFlags.Initialized);

        public bool IsUnplugged => flags.HasFlag(WasapiDeviceInfoFlags.Unplugged);

        public bool IsDisabled => flags.HasFlag(WasapiDeviceInfoFlags.Disabled);

        public override string ToString() => Name;
    }
}
#endif