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
    public static partial class BassAsio
    {
        const string DllName = "bassasio";
        static IntPtr hLib;

        static BassAsio() { Unicode = true; }

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        #region AddDevice
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_ASIO_AddDevice(Guid clsid, string driver, string name);

        [DllImport(DllName, EntryPoint = "BASS_ASIO_AddDevice")]
        static extern int BASS_ASIO_AddDeviceAnsi(Guid clsid, string driver, string name);

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
        public static int AddDevice(Guid ClsID, string Driver, string Name) 
        {
            return Unicode ? Checked(BASS_ASIO_AddDevice(ClsID, Driver, Name))
                           : Checked(BASS_ASIO_AddDeviceAnsi(ClsID, Driver, Name));
        }
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

        #region CPUUsage
        [DllImport(DllName)]
        static extern float BASS_ASIO_GetCPU();

        /// <summary>
		/// Retrieves the current CPU usage of BASSASIO.
		/// </summary>
		/// <returns>The BASSASIO CPU usage as a percentage of total CPU time.</returns>
		/// <remarks>This function includes the time taken by the <see cref="AsioProcedure" /> callback functions.</remarks>
        public static double CPUUsage => BASS_ASIO_GetCPU();
        #endregion

        #region Current Device
        [DllImport(DllName)]
        static extern int BASS_ASIO_GetDevice();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetDevice(int device);

        /// <summary>
		/// Gets or Sets the Asio device to use for subsequent calls in the current thread... 0 = first device.
		/// </summary>
		/// <remarks>
        /// <para>
        /// As in BASS, simultaneously using multiple devices is supported in the BASSASIO API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used needs to be set via this function prior to calling the function.
        /// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
        /// </para>
        /// <para>
        /// The device context setting is used by any function that may result in a <see cref="Errors.NotInitialised"/> error (except this function), which is the majority of them.
        /// When one if those functions is called, it will check the current thread's device setting, and if no device is selected (or the selected device is not initialized), BassAsio will automatically select the lowest device that is initialized. 
		/// This means that when using a single device, there is no need to use this function - BassAsio will automatically use the device that's initialized.
        /// Even if you free the device, and initialize another, BassAsio will automatically switch to the one that is initialized.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised">The device has not been initialized or there are no initialised devices.</exception>
        /// <exception cref="Errors.IllegalDevice">The device number specified is invalid.</exception>
        public static int CurrentDevice
        {
            get { return Checked(BASS_ASIO_GetDevice()); }
            set { if (!Checked(BASS_ASIO_SetDevice(value))) throw new Exception("Could not set device"); }
        }
        #endregion

        #region GetDeviceInfo
        /// <summary>
		/// Retrieves information on an Asio device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <param name="Info">An instance of the <see cref="AsioDeviceInfo" /> structure to store the information at.</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// This function can be used to enumerate the available Asio devices for a setup dialog. 
        /// This function does not throw <see cref="BassException"/>.
		/// </remarks>
        /// <exception cref="Errors.IllegalDevice">The <paramref name="Device"/> number specified is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetDeviceInfo")]
        public static extern bool GetDeviceInfo(int Device, out AsioDeviceInfo Info);
        
		/// <summary>
		/// Retrieves information on an Asio device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <returns>An instance of the <see cref="AsioDeviceInfo" /> structure is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// This function can be used to enumerate the available Asio devices for a setup dialog.
		/// </remarks>
        /// <exception cref="Errors.IllegalDevice">The <paramref name="Device"/> number specified is invalid.</exception>
        public static AsioDeviceInfo GetDeviceInfo(int Device)
        {
            AsioDeviceInfo info;
            Checked(GetDeviceInfo(Device, out info));
            return info;
        }
        
		/// <summary>
		/// Returns the total number of available Asio devices.
		/// </summary>
		/// <returns>Number of ASIO devices available.</returns>
		/// <remarks>Uses <see cref="GetDeviceInfo(int, out AsioDeviceInfo)" /> internally.</remarks>        
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
        #endregion

        #region GetInfo
		/// <summary>
		/// Retrieves information on the Asio device being used.
		/// </summary>
		/// <param name="Info">An instance of the <see cref="BassInfo" /> structure to store the information at.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
        /// As in BASS, simultaneously using multiple devices is supported in the BASSASIO API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used needs to be set via <see cref="CurrentDevice" /> prior to calling the function.
        /// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
        /// This function does not throw <see cref="BassException"/>.
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        [DllImport(DllName, EntryPoint = "BASS_ASIO_GetInfo")]
        public static extern bool GetInfo(out AsioInfo Info);
        
		/// <summary>
		/// Retrieves information on the Asio device being used.
		/// </summary>
		/// <returns>An instance of the <see cref="AsioInfo" /> structure.</returns>
		/// <remarks>
        /// As in BASS, simultaneously using multiple devices is supported in the BASSASIO API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used needs to be set via <see cref="CurrentDevice" /> prior to calling the function.
        /// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        public static AsioInfo Info
        {
            get
            {
                AsioInfo info;
                GetInfo(out info);
                return info;
            }
        }
        #endregion

        #region GetLatency
        [DllImport(DllName)]
        static extern int BASS_ASIO_GetLatency(bool input);
        
        /// <summary>
		/// Retrieves the latency of input or output channels of the current Asio device
		/// </summary>
		/// <param name="Input">Get the input latency? <see langword="false" /> = the output latency.</param>
		/// <returns>If successful, the latency in samples is returned, else -1 is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// The latency is the delay between the sound being recorded and reaching an <see cref="AsioProcedure" />, in the case of input channels.
        /// And the delay between the sample data being fed to an <see cref="AsioProcedure" /> and actually being heard, in the case of output channels. 
		/// The latency is dependant on the buffer size, as specified in the <see cref="Start" /> call.
        /// So the latency should be checked after making that call, not before.
        /// </para>
		/// <para>
        /// The latency time can by calculated be dividing the sample latency by the device sample rate.
        /// When a channel is being resampled, the sample latency will change, but the effective latency time remains constant.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        public static int GetLatency(bool Input) => Checked(BASS_ASIO_GetLatency(Input));
        #endregion

        #region Rate
        [DllImport(DllName)]
        static extern double BASS_ASIO_GetRate();

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetRate(double rate);
        
		/// <summary>
		/// Gets or Sets the current Asio device's sample rate.
		/// </summary>
		/// <remarks>
        /// When it's not possible to set the device to the rate wanted, this can be used to overcome that.
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">The sample rate is not supported by the device/drivers.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static double Rate
        {
            get { return Checked(BASS_ASIO_GetRate()); }
            set { if (!Checked(BASS_ASIO_SetRate(value))) throw new Exception("Could not set rate");}
        }
        #endregion

        #region Version
        [DllImport(DllName)]
        static extern int BASS_ASIO_GetVersion();

        /// <summary>
        /// Gets the version of BassAsio that is Loaded.
        /// </summary>
        public static Version Version => Extensions.GetVersion(BASS_ASIO_GetVersion());
        #endregion

        #region Init
        [DllImport(DllName)]
        static extern bool BASS_ASIO_Init(int device, AsioInitFlags flags);
        
		/// <summary>
		/// Initializes an Asio device/driver.
		/// </summary>
		/// <param name="Device">The device to use... 0 = first device. <see cref="GetDeviceInfo(int, out AsioDeviceInfo)" /> can be used to get the total number of devices.</param>
		/// <param name="Flags">Any combination of <see cref="AsioInitFlags"/>.</param>
		/// <returns>If the device was successfully initialized, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>This function must be successfully called before any input or output can be performed.</para>
		/// <para>
        /// The ASIO driver is accessed via a COM object using the single-threaded apartment model, which means that requests to the driver go through the thread that initialized it, so the thread needs to exist as long as the driver remains initialized.
		/// The thread should also have a message queue.
        /// If device initializing and releasing from multiple threads is required, or the application does not have a message queue (eg. a console application), then the <see cref="AsioInitFlags.Thread"/> flag can be used to have BassAsio create a dedicated thread to host the ASIO driver.
        /// </para>
		/// <para>
        /// Simultaneously using multiple devices is supported in the BassAsio API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions.
        /// <see cref="CurrentDevice" /> is used to switch the current device. 
		/// When successful, the current thread's device is set to the one that was just initialized.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.IllegalDevice">The <paramref name="Device" /> number specified is invalid.</exception>
        /// <exception cref="Errors.Already">A device has already been initialized. You must call <see cref="Free" /> before you can initialize again.</exception>
        /// <exception cref="Errors.DriverNotFound">The driver couldn't be initialized.</exception>
        public static bool Init(int Device, AsioInitFlags Flags) => Checked(BASS_ASIO_Init(Device, Flags));
        #endregion

        #region IsStarted
        [DllImport(DllName)]
        static extern bool BASS_ASIO_IsStarted();

        /// <summary>
		/// Checks, if the current Asio device has been started.
		/// </summary>
		/// <returns>Returns <see langword="true" />, if the device has been started, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        public static bool IsStarted => Checked(BASS_ASIO_IsStarted());
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_ASIO_Monitor")]
        public static extern bool Monitor(int input, int output, int gain, int state, int pan);

        #region SetDSD
        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetDSD(bool dsd);

		/// <summary>
		/// Sets the device's sample format to DSD or PCM.
		/// </summary>
		/// <param name="DSD">Set the sample format to DSD?</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// When a device is switched between PCM and DSD formats, the ASIO channels' format will change accordingly, as reported by <see cref="ChannelGetInfo(bool, int, out AsioChannelInfo)" />.
		/// Any <see cref="ChannelSetFormat" /> and <see cref="ChannelSetRate" /> settings that have been applied will be reset to defaults. 
		/// Other channel settings are unchanged.
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">DSD is not supported by the device/driver.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem.</exception>
        public static bool SetDSD(bool DSD = true) => Checked(BASS_ASIO_SetDSD(DSD));
        #endregion

        #region SetNotify
        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetNotify(AsioNotifyProcedure proc, IntPtr User);
        
		/// <summary>
		/// Sets a notification callback on the ASIO driver.
		/// </summary>
		/// <param name="Procedure">User defined notification function... <see langword="null"/> = disable notifications.</param>
		/// <param name="User">User instance data to pass to the callback function.</param>
		/// <returns>If succesful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>A previously set notification callback can be changed (or removed) at any time, by calling this function again.</remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        public static bool SetNotify(AsioNotifyProcedure Procedure, IntPtr User = default(IntPtr)) => Checked(BASS_ASIO_SetNotify(Procedure, User));
        #endregion

        #region Unicode
        static bool unicode = false;

        [DllImport(DllName)]
        static extern bool BASS_ASIO_SetUnicode(bool Unicode);

		/// <summary>
		/// Gets or Sets the character set used in device information text: if <see langword="false"/>, ANSI is used (default), else UTF-16 is used.
        /// </summary>
		/// <remarks>
        /// This function determines the character set that is used in the <see cref="AsioDeviceInfo" /> structure and in <see cref="AddDevice" /> function calls.
		/// It does not affect ASIO channel names in the <see cref="AsioChannelInfo" /> and <see cref="AsioInfo" /> structure.
		/// <para>The character set choice is finalised in the first <see cref="GetDeviceInfo(int, out AsioDeviceInfo)" />, <see cref="AddDevice" /> or <see cref="Init" /> call, and it cannot be changed after that.</para>
		/// </remarks>
        /// <exception cref="Errors.NotAvailable">This function is only available before any devices have been enumerated.</exception>
        public static bool Unicode
        {
            get { return unicode; }
            set
            {
                if (Checked(BASS_ASIO_SetUnicode(value)))
                    unicode = value;
            }
        }
        #endregion

        #region Start
        [DllImport(DllName)]
        static extern bool BASS_ASIO_Start(int buflen, int threads);
        
		/// <summary>
		/// Starts the current Asio device.
		/// </summary>
		/// <param name="BufferLength">Buffer length in samples... 0 = use current length.</param>
		/// <param name="Threads">The number of processing threads to use... 0 = use current number.</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
        /// Before starting the device, channels must be enabled using <see cref="ChannelEnable" />.
        /// Once started, channels can't be enabled or disabled until the device is stopped, using <see cref="Stop" />.
		/// <para>
        /// The default number of processing threads is 1, which means that the <see cref="AsioProcedure" /> functions of the enabled channels get called in series (starting with the lowest input channel).
		/// Multiple channels can be processed in parallel if multiple threads are created for that purpose via the threads parameter.
		/// The number of threads is automatically capped at the number of enabled channels with an <see cref="AsioProcedure" /> function, which is sufficient to have them all processed simultaneously.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Already">The device has already been started.</exception>
        /// <exception cref="Errors.NoFreeChannelAvailable">No channels have been enabled.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static bool Start(int BufferLength = 0, int Threads = 0) => Checked(BASS_ASIO_Start(BufferLength, Threads));
        #endregion
        
        #region Stop
        [DllImport(DllName)]
        static extern bool BASS_ASIO_Stop();

        /// <summary>
		/// Stops the current Asio device.
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
        /// As in BASS, simultaneously using multiple devices is supported in the BASSASIO API via a context switching system - instead of there being an extra "device" parameter in the function calls, 
		/// the device to be used needs to be set via <see cref="CurrentDevice" /> prior to calling the function. 
		/// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.OutputNotStarted">The device hasn't been started.</exception>
        public static bool Stop() => Checked(BASS_ASIO_Stop());
        #endregion
    }
}
