using System;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassAsio: bassasio.dll
    /// </summary>
    /// <remarks>
    /// Only available on Windows
    /// </remarks>
    public static class BassAsio
    {
        const string DllName = "bassasio";
        static IntPtr hLib;

        static BassAsio() { SetUnicode(); }

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        #region AddDevice
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_ASIO_AddDevice(Guid clsid, string driver, string name);

        /// <summary>
		/// Adds a driver to the device list.
		/// </summary>
		/// <param name="ClsID">The driver's class ID.</param>
		/// <param name="Driver">The filename of the driver.</param>
		/// <param name="Name">An optional description of the driver.</param>
		/// <returns>
        /// If successful, the new device number is returned (which might be used in a subsequent <see cref="Init" /> call), else -1 is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
		/// <remarks>
		/// <para>A list of installed ASIO drivers is kept in the Windows registry, which is where BassAsio gets its device list from, 
		/// but it is also possible to add unregistered drivers (eg. private drivers) to the list via this function. 
		/// If successful, the returned device number can be used in a <see cref="Init" /> call to use the driver.</para>
		/// <para>The <paramref name="Driver"/> and <patamref name="Name"/> strings are expected to be in Unicode from.</para>
		/// </remarks>
        /// <exception cref="Errors.FileOpen">The <paramref name="Driver" /> file does not exist.</exception>
        public static int AddDevice(Guid ClsID, string Driver, string Name) => Checked(BASS_ASIO_AddDevice(ClsID, Driver, Name));
        #endregion

        #region CheckRate
        [DllImport(DllName)]
        static extern bool BASS_ASIO_CheckRate(double rate);

        /// <summary>
		/// Checks if a sample rate is supported by the device.
		/// </summary>
		/// <param name="Rate">The sample rate to check.</param>
		/// <returns>If the sample rate is supported, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">The sample rate is not supported by the device/drivers.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static bool CheckRate(double Rate) => Checked(BASS_ASIO_CheckRate(Rate));
        #endregion

        #region ControlPanel
        [DllImport(DllName)]
        static extern bool BASS_ASIO_ControlPanel();

        /// <summary>
		/// Displays the current Asio driver's control panel.
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static bool ControlPanel() => Checked(BASS_ASIO_ControlPanel());
        #endregion

        #region LastError
        [DllImport(DllName)]
        static extern Errors BASS_ASIO_ErrorGetCode();

        /// <summary>
		/// Retrieves the error code for the most recent BassAsio function call in the current thread.
		/// </summary>
		/// <returns>
        /// If no error occured during the last BassAsio function call then <see cref="Errors.OK"/> is returned, else one of the <see cref="Errors" /> values is returned. 
		/// See the function description for an explanation of what the error code means.
        /// </returns>
		/// <remarks>Error codes are stored for each thread. So if you happen to call 2 or more BassAsio functions at the same time, they will not interfere with eachother's error codes.</remarks>
        public static Errors LastError => BASS_ASIO_ErrorGetCode();
        #endregion

        #region Free
        [DllImport(DllName)]
        static extern bool BASS_ASIO_Free();

        /// <summary>
		/// Releases the Asio device/driver.
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>Make sure to free each Asio device you have initialized with <see cref="Init" />, <see cref="CurrentDevice" /> is used to switch the current device.</remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init"/> has not been successfully called.</exception>
        public static bool Free() => Checked(BASS_ASIO_Free());
        #endregion

        #region Future
        [DllImport(DllName)]
        static extern bool BASS_ASIO_Future(AsioFuture selector, IntPtr param);

        [Obsolete("Use AsioFuture enum overload.")]
        public static bool Future(int Selector, IntPtr Param) => Checked(BASS_ASIO_Future((AsioFuture)Selector, Param));

        /// <summary>
		/// Provides access to the driver's 'future' function.
		/// </summary>
		/// <param name="Selector">Operation code.</param>
		/// <param name="Param">Pointer to the operation's parameters, if applicable.</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>This method is a general purpose extension method serving various purposes.</remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">The <paramref name="Selector" /> is not supported by the driver.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem.</exception>
        public static bool Future(AsioFuture Selector, IntPtr Param) => Checked(BASS_ASIO_Future(Selector, Param));
        #endregion

        [DllImport(DllName)]
        static extern float BASS_ASIO_GetCPU();

        public static double CPUUsage => BASS_ASIO_GetCPU();

        #region Current Device
        [DllImport(DllName)]
        static extern int BASS_ASIO_GetDevice();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetDevice(int device);

        public static int CurrentDevice
        {
            get { return BASS_ASIO_GetDevice(); }
            set { if (!BASS_ASIO_SetDevice(value)) throw new Exception("Could not set device"); }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetDeviceInfo")]
        public static extern bool GetDeviceInfo(int device, out AsioDeviceInfo info);

        public static AsioDeviceInfo GetDeviceInfo(int device)
        {
            AsioDeviceInfo info;
            GetDeviceInfo(device, out info);
            return info;
        }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetInfo")]
        public static extern bool GetInfo(out AsioInfo info);

        public static int DeviceCount
        {
            get
            {
                int i;
                AsioDeviceInfo info;

                for (i = 0; GetDeviceInfo(i, out info); ++i) ;

                return i;
            }
        }

        public static AsioInfo Info
        {
            get
            {
                AsioInfo info;
                GetInfo(out info);
                return info;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetLatency")]
        public static extern int GetLatency(bool input);

        #region Rate
        [DllImport(DllName)]
        static extern double BASS_ASIO_GetRate();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetRate(double rate);

        public static double Rate
        {
            get { return BASS_ASIO_GetRate(); }
            set { if (!BASS_ASIO_SetRate(value)) throw new Exception("Could not set rate");}
        }
        #endregion

        [DllImport(DllName)]
        static extern int BASS_ASIO_GetVersion();

        public static Version Version => Extensions.GetVersion(BASS_ASIO_GetVersion());

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Init")]
        public static extern bool Init(int device, AsioInitFlags flags);

        [DllImport(DllName)]
        static extern bool BASS_ASIO_IsStarted();

        public static bool IsStarted => BASS_ASIO_IsStarted();

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Monitor")]
        public static extern bool Monitor(int input, int output, int gain, int state, int pan);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetDSD")]
        public static extern bool SetDSD(bool dsd);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetNotify")]
        public static extern bool SetNotify(AsioNotifyProcedure proc, IntPtr User = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ASIO_SetUnicode")]
        static extern bool SetUnicode(bool Unicode = true);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Start")]
        public static extern bool Start(int buflen = 0, int threads = 0);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Stop")]
        public static extern bool Stop();

        #region Channels
        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnable")]
        public static extern bool ChannelEnable(bool input, int channel, AsioProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelEnableMirror")]
        public static extern bool ChannelEnableMirror(int channel, bool input2, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetFormat")]
        public static extern AsioSampleFormat ChannelGetFormat(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetInfo")]
        public static extern bool ChannelGetInfo(bool input, int channel, out AsioChannelInfo info);

        public static AsioChannelInfo ChannelGetInfo(bool input, int channel)
        {
            AsioChannelInfo info;
            ChannelGetInfo(input, channel, out info);
            return info;
        }

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetLevel(bool input, int channel);

        public static double ChannelGetLevel(bool input, int channel) => BASS_ASIO_ChannelGetLevel(input, channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelGetRate")]
        public static extern double ChannelGetRate(bool input, int channel);

        [DllImport(DllName)]
        static extern float BASS_ASIO_ChannelGetVolume(bool input, int channel);

        public static double ChannelGetVolume(bool input, int channel) => BASS_ASIO_ChannelGetVolume(input, channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelIsActive")]
        public static extern AsioChannelActive ChannelIsActive(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelJoin")]
        public static extern bool ChannelJoin(bool input, int channel, int channel2);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelPause")]
        public static extern bool ChannelPause(bool input, int channel);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelReset")]
        public static extern bool ChannelReset(bool input, int channel, AsioChannelResetFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetFormat")]
        public static extern bool ChannelSetFormat(bool input, int channel, AsioSampleFormat format);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_ChannelSetRate")]
        public static extern bool ChannelSetRate(bool input, int channel, double rate);

        [DllImport(DllName)]
        static extern bool BASS_ASIO_ChannelSetVolume(bool input, int channel, float volume);

        public static bool ChannelSetVolume(bool input, int channel, double volume)
        {
            return BASS_ASIO_ChannelSetVolume(input, channel, (float)volume);
        }
        #endregion
    }
}
