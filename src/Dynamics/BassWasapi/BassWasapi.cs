using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps basswasapi.dll: Windows Audio Session API driver library
    /// </summary>
    /// <remarks>
    /// <para>BASSWASAPI is basically a wrapper for Windows Audio Session API drivers, with the addition of channel joining, format conversion and resampling.</para>
    /// <para>
    /// BASSWASAPI requires a soundcard with a Windows Session API drivers installed (Vista or above).
    /// It also makes use of SSE2 and 3DNow optimizations, but is fully functional without them.
    /// BASS is not required by BASSWASAPI, but BASS can of course be used to decode, apply DSP/FX, etc.
    /// </para>
    /// </remarks>
    public static class BassWasapi
    {
        public const int DefaultDevice = -1,
                         DefaultInputDevice = -2,
                         DefaultLoopbackDevice = -3;

        const string DllName = "basswasapi";
        static IntPtr hLib;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        #region CPU
        [DllImport(DllName)]
        extern static float BASS_WASAPI_GetCPU();

        /// <summary>
        /// Retrieves the current CPU usage of BASSWASAPI.
        /// </summary>
        /// <returns>The BASSWASAPI CPU usage as a percentage of total CPU time.</returns>
        /// <remarks>This function includes the time taken by the <see cref="WasapiProcedure" /> callback functions.</remarks>
        public static double CPUUsage => BASS_WASAPI_GetCPU();
        #endregion

        #region Current Device
        [DllImport(DllName)]
        extern static int BASS_WASAPI_GetDevice();

        [DllImport(DllName)]
        extern static bool BASS_WASAPI_SetDevice(int device);

		/// <summary>
		/// Gets or Sets the Wasapi device to use for susequent calls in the current thread... 0 = first device. Use <see cref="Bass.LastError" /> to get the error code.
		/// </summary>
		/// <remarks>
		/// <para>
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system;
        /// instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions.
        /// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
        /// </para>
		/// <para>
        /// All of the BassWasapi functions that do not have their own "device" parameter make use of this device selection.
        /// When one of them is called, BassWasapi will check the current thread's device setting, and if no device is selected (or the selected device is not initialized), BassWasapi will automatically select the lowest device that is initialized.
        /// This means that when using a single device, there is no need to use this function;
        /// BassWasapi will automatically use the device that is initialized.
        /// Even if you free the device, and initialize another, BassWasapi will automatically switch to the one that is initialized.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        public static int CurrentDevice
        {
            get { return BASS_WASAPI_GetDevice(); }
            set 
            { 
                if (!BASS_WASAPI_SetDevice(value))
                    throw new BassException(); 
            }
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetDeviceInfo")]
        public extern static bool GetDeviceInfo(int device, out WasapiDeviceInfo info);
        
		/// <summary>
		/// Sets a device change notification callback.
		/// </summary>
		/// <param name="Procedure">User defined notification function... <see langword="null" /> = disable notifications.</param>
		/// <param name="User">User instance data to pass to the callback function.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>A previously set notification callback can be changed (or removed) at any time, by calling this function again.</remarks>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetNotify")]
        public extern static bool SetNotify(WasapiNotifyProcedure Procedure, IntPtr User = default(IntPtr));

        public static WasapiDeviceInfo GetDeviceInfo(int Device)
        {
            WasapiDeviceInfo info;
            if (!GetDeviceInfo(Device, out info))
                throw new BassException();
            return info;
        }
        
		/// <summary>
		/// Gets the total number of available Wasapi devices.
		/// </summary>
        public static int DeviceCount
        {
            get
            {
                WasapiDeviceInfo info;

                int i;

                for (i = 0; GetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_CheckFormat")]
        public extern static WasapiFormat CheckFormat(int device, int freq, int chans, WasapiInitFlags flags);
        
		/// <summary>
		/// Retrieves information on the Wasapi device being used.
		/// </summary>
		/// <param name="Info">An instance of the <see cref="WasapiInfo" /> structure to store the information at.</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// This method can be used to get the effective settings used with an initialized Wasapi device (endpoint).
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetInfo")]
        public extern static bool GetInfo(out WasapiInfo Info);
        
		/// <summary>
		/// Retrieves information on the Wasapi device being used.
		/// </summary>
		/// <returns>An instance of the <see cref="WasapiInfo" /> structure.</returns>
		/// <remarks>
        /// This method can be used to get the effective settings used with an initialized Wasapi device (endpoint).
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        public static WasapiInfo Info
        {
            get
            {
                WasapiInfo info;
                if (!GetInfo(out info))
                    throw new BassException();
                return info;
            }
        }
        
		/// <summary>
		/// Frees the Wasapi device/driver (endpoint).
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// This function should be called for all initialized devices before the program closes.
        /// Freed devices do not need to have been stopped with <see cref="Stop" /> beforehand.
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Free")]
        public extern static bool Free();

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetData")]
        public extern static int GetData(IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetData")]
        public extern static int GetData([In, Out] float[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_PutData")]
        public extern static int PutData(IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_PutData")]
        public extern static int PutData(float[] Buffer, int Length);
                
		/// <summary>
		/// Locks the device to the current thread.
		/// </summary>
		/// <param name="State">If <see langword="false" />, unlock WASAPI, else lock it.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// Locking a device prevents other threads from accessing the device buffer, including a <see cref="WasapiProcedure"/>.
        /// Other threads wanting to access a locked device will block until it is unlocked, so a device should only be locked very briefly.
		/// A device must be unlocked in the same thread that it was locked.
		/// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Lock")]
        public extern static bool Lock(bool State = true);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetMute")]
        public extern static bool GetMute(WasapiVolumeTypes mode = WasapiVolumeTypes.Device);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetMute")]
        public extern static bool SetMute(WasapiVolumeTypes mode, bool mute);
        
		/// <summary>
		/// Gets the audio meter information of the current Wasapi device/driver (endpoint).
		/// </summary>
		/// <param name="Device">The device to use... 0 = first device. <see cref="GetDeviceInfo(int, out WasapiDeviceInfo)" /> can be used to get the total number of devices.</param>
		/// <param name="Channel">The channel number to get the audio level meter information from (0=first, -1=all).</param>
		/// <returns>The audio level between 0.0 (silence) and 1.0 (maximum).</returns>
		/// <remarks>
        /// This method returns the global session level for the device which might include the level of other applications using the same device in shared-mode.
		/// <para>
        /// This function gets the level from the device/driver, or WASAPI if the device does not have its own level meter.
        /// If the latter case, the level will be unavailable when exclusive mode is active.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Wasapi">WASAPI is not available.</exception>
        /// <exception cref="Errors.Device"><paramref name="Device" /> is not valid.</exception>
        /// <exception cref="Errors.Driver">The device driver does not support level retrieval.</exception>
        /// <exception cref="Errors.Parameter"><paramref name="Channel" /> is not valid.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetDeviceLevel")]
        public extern static float GetDeviceLevel(int Device, int Channel = -1);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetVolume")]
        public extern static float GetVolume(WasapiVolumeTypes curve = WasapiVolumeTypes.Device);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_SetVolume")]
        public extern static bool SetVolume(WasapiVolumeTypes curve, float volume);

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Init")]
        public extern static bool Init(int Device,
                                        int Frequency = 0,
                                        int Channels = 0,
                                        WasapiInitFlags Flags = WasapiInitFlags.Shared,
                                        float Buffer = 0,
                                        float Period = 0,
                                        WasapiProcedure Procedure = null,
                                        IntPtr User = default(IntPtr));

        #region IsStarted
        [DllImport(DllName)]
        extern static bool BASS_WASAPI_IsStarted();
        
		/// <summary>
		/// Checks, if the current Wasapi device/driver (endpoint) has been already started (via <see cref="Start" />).
		/// </summary>
		/// <returns>Returns <see langword="true" />, if the device has been started, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        public static bool IsStarted => BASS_WASAPI_IsStarted();
        #endregion
        
		/// <summary>
		/// Starts processing the current Wasapi device/driver (endpoint).
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// Before starting the device, it must be initialized using <see cref="Init" />.
        /// Use <see cref="Stop" /> to stop processing the device.
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Start")]
        public extern static bool Start();
        
		/// <summary>
		/// Stops the current Wasapi device/driver (endpoint).
		/// </summary>
		/// <param name="Reset">Flush the device buffer?
		/// <para>
        /// <see langword="true" /> will clear the output buffer.
        /// Otherwise it is like pausing, eg. <see cref="Start" /> will resume playing the buffered data.
        /// </para>
		/// </param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>If the device buffer is left unflushed (<paramref name="Reset"/> = <see langword="false"/>), a subsequent <see cref="Start" /> call will resume things with the buffered data, otherwise it will resume with fresh data.</para>
		/// <para>When using multiple devices, the current thread's device setting (as set with <see cref="CurrentDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.Start">The device hasn't been started.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_Stop")]
        public extern static bool Stop(bool Reset = true);
        
		/// <summary>
		/// Retrieves the level (peak amplitude) of the current Wasapi device/driver (endpoint).
		/// </summary>
		/// <returns>If an error occurs, -1 is returned, use <see cref="Bass.LastError" /> to get the error code.
		/// <para>
        /// If successful, the level of the left channel is returned in the low word (low 16-bits), and the level of the right channel is returned in the high word (high 16-bits).
        /// If the channel is mono, then the low word is duplicated in the high word. 
		/// The level ranges linearly from 0 (silent) to 32768 (max). 0 will be returned when a channel is stalled.
        /// </para>
		/// </returns>
		/// <remarks>
        /// This function is like the standard <see cref="Bass.ChannelGetLevel(int)" />, but it gets the level from the devices's buffer instead of decoding data from a channel, which means that the device doesn't miss out on any data. 
		/// The <see cref="WasapiInitFlags.Buffer"/> flag needs to have been specified in the device's initialization to enable the use of this function.
		/// </remarks>
        /// <exception cref="Errors.Init"><see cref="Init" /> has not been successfully called.</exception>
        /// <exception cref="Errors.NotAvailable">The device was not initialized using buffering (<see cref="WasapiInitFlags.Buffer"/>).</exception>
        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetLevel")]
        public extern static int GetLevel();

        [DllImport(DllName, EntryPoint = "BASS_WASAPI_GetLevelEx")]
        public static extern int GetLevel([In, Out] float[] Levels, float Length, LevelRetrievalFlags Flags);

        #region Version
        [DllImport(DllName)]
        extern static int BASS_WASAPI_GetVersion();

        /// <summary>
        /// Gets the Version of BassWasapi that is loaded.
        /// </summary>
        public static Version Version => Extensions.GetVersion(BASS_WASAPI_GetVersion());
        #endregion
    }
}
