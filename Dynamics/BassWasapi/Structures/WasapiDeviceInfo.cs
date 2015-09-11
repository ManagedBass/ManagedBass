using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
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
        public WasapiDeviceType Type { get { return type; } }

        /// <summary>
        /// The minimum update period (in seconds) of the device.
        /// </summary>
        public double MinimumUpdatePeriod { get { return minperiod; } }

        /// <summary>
        /// The default update period (in seconds) of the device.
        /// </summary>
        public double DefaultUpdatePeriod { get { return defperiod; } }

        /// <summary>
        /// The shared-mode format mixers sample rate.
        /// </summary>
        public int MixFrequency { get { return mixfreq; } }

        /// <summary>
        /// The shared-mode format mixers number of channels.
        /// </summary>
        public int MixChannels { get { return mixchans; } }

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        /// <summary>
        /// The ID of the driver being used.
        /// </summary>
        public string ID { get { return Marshal.PtrToStringAnsi(id); } }

        /// <summary>
        /// The device is the system default device.
        /// </summary>
        public bool IsDefault { get { return flags.HasFlag(WasapiDeviceInfoFlags.Default); } }

        public bool IsEnabled { get { return flags.HasFlag(WasapiDeviceInfoFlags.Enabled); } }

        public bool IsInput { get { return flags.HasFlag(WasapiDeviceInfoFlags.Input); } }

        public bool IsLoopback { get { return flags.HasFlag(WasapiDeviceInfoFlags.Loopback); } }

        public bool IsInitialized { get { return flags.HasFlag(WasapiDeviceInfoFlags.Initialized); } }

        public bool IsUnplugged { get { return flags.HasFlag(WasapiDeviceInfoFlags.Unplugged); } }

        public bool IsDisabled { get { return flags.HasFlag(WasapiDeviceInfoFlags.Disabled); } }

        public override string ToString() { return Name; }
    }
}