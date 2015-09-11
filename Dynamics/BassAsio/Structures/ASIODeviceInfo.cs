using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Used with <see cref="BassAsio.GetDeviceInfo(int,out AsioDeviceInfo)" /> to retrieve information on an asio device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsioDeviceInfo
    {
        IntPtr name;
        IntPtr driver;

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name { get { return Marshal.PtrToStringAnsi(name); } }

        /// <summary>
        /// The filename of the driver being used.
        /// <para>Further information can be obtained from the file using the GetFileVersionInfo Win32 API function.</para>
        /// </summary>
        public string Driver { get { return Marshal.PtrToStringAnsi(driver); } }

        public override string ToString() { return Name; }
    }
}