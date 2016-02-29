using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Used with <see cref="Bass.GetDeviceInfo(int,out DeviceInfo,bool)" /> or <see cref="Bass.RecordGetDeviceInfo(int,out DeviceInfo)" /> to retrieve information on a device.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When a device is disabled/disconnected, it is still retained in the device list, but the IsEnabled is set to <see langword="false" /> flag is removed from it.
    /// If the device is subsequently re-enabled, it may become available again with the same device number, or the system may add a new entry for it.
    /// </para>
    /// <para>
    /// When a new device is connected, it can affect the other devices and result in the system moving them to new device entries.
    /// If an affected device is initialized, it will stop working and will need to be reinitialized using its new device number.
    /// </para>
    /// <para><b>Platform-specific</b></para>
    /// <para>
    /// On Windows, <see cref="DeviceInfo.Driver"/> can reveal the Type of driver being used on systems that support both VxD and WDM drivers (Windows Me/98SE).
    /// Further information can be obtained from the file via the GetFileVersionInfo function.
    /// On Vista and newer, the device's endpoint ID is given rather than its driver filename.
    /// On OSX, driver is the device's UID, and on Linux it is the ALSA device name.
    /// It is unused on other platforms.
    /// The device Type is only available on Windows (Vista and newer) and OSX.
    /// On Windows, DisplayPort devices will have <see cref="DeviceType.HDMI"/> rather than <see cref="DeviceType.DisplayPort"/>.
    /// </para>
    /// <para>
    /// Depending on the <see cref="Bass.UnicodeDeviceInformation" /> config setting, <see cref="DeviceInfo.Name"/> and <see cref="DeviceInfo.Driver"/> can be in ANSI or UTF-8 form on Windows.
    /// They are always in UTF-16 form on Windows CE, and UTF-8 on other platforms.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class DeviceInfo
    {
        IntPtr name;
        IntPtr driver;
        DeviceInfoFlags flags;

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name => Marshal.PtrToStringAnsi(name);

        /// <summary>
        /// The filename of the driver being used... <see langword="null" /> = no driver (ie. "No Sound" device).
        /// <para>On systems that can use both VxD and WDM drivers (Windows Me/98SE), this will reveal which Type of driver is being used.</para>
        /// <para>Further information can be obtained from the file using the GetFileVersionInfo Win32 API function.</para>
        /// </summary>
        public string Driver => Marshal.PtrToStringAnsi(driver);

        /// <summary>
        /// The device is the system default device.
        /// </summary>
        public bool IsDefault => flags.HasFlag(DeviceInfoFlags.Default);

        /// <summary>
        /// The device is enabled and can be used.
        /// </summary>
        public bool IsEnabled => flags.HasFlag(DeviceInfoFlags.Enabled);

        /// <summary>
        /// The device is already initialized.
        /// </summary>
        public bool IsInitialized => flags.HasFlag(DeviceInfoFlags.Initialized);

        /// <summary>
        /// The device's Type.
        /// </summary>
        public DeviceType Type => (DeviceType)(flags & DeviceInfoFlags.TypeMask);
    }
}
