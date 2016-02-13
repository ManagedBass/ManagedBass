using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        public const int NoSoundDevice = 0, DefaultDevice = -1;

        [DllImport(DllName)]
        static extern bool BASS_Init(int Device, int Frequency, DeviceInitFlags Flags, IntPtr Win = default(IntPtr), IntPtr ClsID = default(IntPtr));

        /// <summary>
        /// Initializes an output device.
        /// </summary>
        /// <param name="Device">The device to use... -1 = default device, 0 = no sound, 1 = first real output device.
        /// <see cref="GetDeviceInfo(int,out DeviceInfo)" /> or <see cref="DeviceCount" /> can be used to get the total number of devices.
        /// </param>
        /// <param name="Frequency">Output sample rate.</param>
        /// <param name="Flags">Any combination of <see cref="DeviceInitFlags"/>.</param>
        /// <param name="Win">The application's main window... <see cref="IntPtr.Zero" /> = the desktop window (use this for console applications).</param>
        /// <param name="ClsID">Class identifier of the object to create, that will be used to initialize DirectSound... <see langword="null" /> = use default</param>
        /// <returns>If the device was successfully initialized, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.IllegalDevice">The device number specified is invalid.</exception>
        /// <exception cref="Errors.Already">The device has already been initialized. You must call <see cref="Free" /> before you can initialize it again.</exception>
        /// <exception cref="Errors.DriverNotFound">There is no available device driver... the device may already be in use.</exception>
        /// <exception cref="Errors.UnsupportedSampleFormat">The specified format is not supported by the device. Try changing the <paramref name="Frequency" /> and <paramref name="Flags" /> parameters.</exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.No3D">The device has no 3D support.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        /// <remarks>
        /// <para>This function must be successfully called before using any sample, stream or MOD music functions. The recording functions may be used without having called this function.</para>
        /// <para>Playback is not possible with the "no sound" device, but it does allow the use of "decoding channels", eg. to decode files.</para>
        /// <para>When specifying a class identifier (<paramref name="ClsID"/>), after successful initialization, you can use <see cref="GetDSoundObject" /> to retrieve the DirectSound object, and through that access any special interfaces that the object may provide.</para>
        /// <para>
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions. <see cref="CurrentDevice" /> is used to switch the current device.
        /// When successful, <see cref="Init"/> automatically sets the current thread's device to the one that was just initialized.
        /// </para>
        /// <para>
        /// When using the default device (device = -1), <see cref="CurrentDevice" /> can be used to find out which device it was mapped to.
        /// On Windows, it'll always be the first device.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// On Linux, a 'Default' device is hardcoded to device number 1, which uses the default output set in the ALSA config; that could map directly to one of the other devices or it could use ALSA plugins.
        /// If the <see cref="IncludeDefaultDevice" /> config option has been enbled, a "Default" device is also available on Windows, who's output will follow default device changes on Windows 7.
        /// In both cases, the "Default" device will also be the default device (device = -1).
        /// </para>
        /// <para>
        /// The sample format specified in the <paramref name="Frequency" /> and <paramref name="Flags" /> parameters has no effect on the device output on iOS or OSX, and not on Windows unless VxD drivers are used (on Windows 98/95);
        /// with WDM drivers (on Windows XP/2000/Me/98SE), the output format is automatically set depending on the format of what is played and what the device supports, while on Vista and above, the output format is determined by the user's choice in the Sound control panel.
        /// On Linux the output device will use the specified format if possible, but will otherwise use a format as close to it as possible.
        /// If the <see cref="DeviceInitFlags.Frequency"/> flag is specified on iOS or OSX, then the device's output rate will be set to the freq parameter (if possible).
        /// The <see cref="DeviceInitFlags.Frequency"/> flag has no effect on other platforms.
        /// <see cref="GetInfo" /> can be used to check what the output format actually is.
        /// </para>
        /// <para>
        /// The <paramref name="Win" /> and <paramref name="ClsID" /> parameters are only used on Windows and are ignored on other platforms.
        /// That applies to the <see cref="DeviceInitFlags.CPSpeakers"/> and <see cref="DeviceInitFlags.ForcedSpeakerAssignment"/> flags too, as the number of available speakers is always accurately detected on the other platforms.
        /// The <see cref="DeviceInitFlags.Latency"/> flag is also ignored on Linux/OSX/Android/Windows CE, as latency information is available without it.
        /// The latency is also available without it on iOS, but not immediately following this function call unless the flag is used.
        /// </para>
        /// <para>
        /// The <see cref="DeviceInitFlags.DMix"/> flag is only available on Linux, and allows multiple applications to share the device (if they all use 'dmix').
        /// It may also be possible for multiple applications to use exclusive access if the device is capable of hardware mixing.
        /// If exclusive access initialization fails, the <see cref="DeviceInitFlags.DMix"/> flag will automatically be tried;
        /// if that happens, it can be detected via <see cref="GetInfo" /> and the <see cref="BassInfo.InitFlags"/>.
        /// </para>
        /// <para>On Linux and Windows CE, the length of the device's buffer can be set via the <see cref="PlaybackBufferLength" /> config option.</para>
        /// </remarks>
        public static Return<bool> Init(int Device = DefaultDevice, int Frequency = 44100, DeviceInitFlags Flags = DeviceInitFlags.Default, IntPtr Win = default(IntPtr), IntPtr ClsID = default(IntPtr))
        {
            return BASS_Init(Device, Frequency, Flags, Win, ClsID);
        }

        [DllImport(DllName, EntryPoint = "BASS_Start")]
        public static extern bool Start();

        [DllImport(DllName, EntryPoint = "BASS_Pause")]
        public static extern bool Pause();

        [DllImport(DllName, EntryPoint = "BASS_Stop")]
        public static extern bool Stop();

        #region Free
        [DllImport(DllName)]
        static extern bool BASS_Free();

        public static Return<bool> Free(int Device) { return BASS_SetDevice(Device) && BASS_Free(); }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ChannelGetDevice")]
        public static extern int ChannelGetDevice(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_ChannelSetDevice")]
        public static extern bool ChannelSetDevice(int Handle, int Device);

        public static int DeviceCount
        {
            get
            {
                int i;
                DeviceInfo info;

                for (i = 0; GetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        #region Current Device Volume
        [DllImport(DllName)]
        extern static float BASS_GetVolume();

        [DllImport(DllName)]
        extern static bool BASS_SetVolume(float volume);

        public static double Volume
        {
            get { return BASS_GetVolume(); }
            set { BASS_SetVolume((float)value); }
        }
        #endregion

        #region Current Device
        [DllImport(DllName)]
        extern static int BASS_GetDevice();

        [DllImport(DllName)]
        extern static bool BASS_SetDevice(int Device);

        public static int CurrentDevice
        {
            get { return BASS_GetDevice(); }
            set { if (!BASS_SetDevice(value)) throw new Exception("Could not Set Device"); }
        }
        #endregion

        #region Get Device Info
        [DllImport(DllName, EntryPoint = "BASS_GetDeviceInfo")]
        public static extern bool GetDeviceInfo(int Device, out DeviceInfo Info);

        public static DeviceInfo GetDeviceInfo(int Device)
        {
            DeviceInfo temp;
            GetDeviceInfo(Device, out temp);
            return temp;
        }
        #endregion
    }
}