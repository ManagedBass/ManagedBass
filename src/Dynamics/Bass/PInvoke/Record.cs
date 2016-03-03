using System;
using System.Runtime.InteropServices;
using static ManagedBass.Extensions;

namespace ManagedBass.Dynamics
{
    public static partial class Bass
    {
        #region RecordInit
        [DllImport(DllName)]
        extern static bool BASS_RecordInit(int Device);

        /// <summary>
        /// Initializes a recording device.
        /// </summary>
        /// <param name="Device">The device to use... -1 = default device, 0 = first. <see cref="RecordGetDeviceInfo(int, out DeviceInfo)" /> or <see cref="RecordingDeviceCount" /> can be used to get the total number of devices.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError"/> to get the error code.</returns>
        /// <remarks>
        /// This function must be successfully called before using the recording features.
        /// <para>
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions.
        /// <see cref="CurrentRecordingDevice" /> is used to switch the current recording device.
        /// When successful, <see cref="RecordInit" /> automatically sets the current thread's device to the one that was just initialized
        /// </para>
        /// <para>
        /// When using the default device (device = -1), <see cref="CurrentRecordingDevice" /> can be used to find out which device it was mapped to.
        /// On Windows, it'll always be the first device.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config;
        /// that could map directly to one of the other devices or it could use ALSA plugins.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.DirectX">A sufficient version of DirectX is not installed.</exception>
        /// <exception cref="Errors.IllegalDevice"><paramref name="Device" /> is invalid.</exception>
        /// <exception cref="Errors.Already">The device has already been initialized. <see cref="RecordFree" /> must be called before it can be initialized again.</exception>
        /// <exception cref="Errors.DriverNotFound">There is no available device driver.</exception>
        public static bool RecordInit(int Device = DefaultDevice) => Checked(BASS_RecordInit(Device));
        #endregion

        #region RecordFree
        [DllImport(DllName)]
        extern static bool BASS_RecordFree();

        /// <summary>
		/// Frees all resources used by the recording device.
		/// </summary>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>This function should be called for all initialized recording devices before your program exits.</para>
		/// <para>When using multiple recording devices, the current thread's device setting (as set with <see cref="CurrentRecordingDevice" />) determines which device this function call applies to.</para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="RecordInit" /> has not been successfully called - there are no initialized devices.</exception>
        public static bool RecordFree() => Checked(BASS_RecordFree());
        #endregion

        #region RecordStart
        [DllImport(DllName)]
        extern static int BASS_RecordStart(int freq, int chans, BassFlags flags, RecordProcedure proc, IntPtr User);

        /// <summary>
		/// Starts recording.
		/// </summary>
		/// <param name="freq">The sample rate to record at.</param>
		/// <param name="chans">The number of channels... 1 = mono, 2 = stereo, etc.</param>
		/// <param name="flags">Any combination of <see cref="BassFlags.Byte"/>, <see cref="BassFlags.Float"/> and <see cref="BassFlags.RecordPause"/>.</param>
		/// <param name="proc">The user defined function to receive the recorded sample data... can be <see langword="null" /> if you do not wish to use a callback.</param>
		/// <param name="User">User instance data to pass to the callback function.</param>
		/// <returns>If successful, the new recording's handle is returned, else <see langword="false" /> is returned. Use <see cref="LastError"/> to get the error code.</returns>
		/// <remarks>
		/// Use <see cref="ChannelStop" /> to stop the recording, and <see cref="ChannelPause" /> to pause it.
        /// Recording can also be started in a paused state (via the <see cref="BassFlags.RecordPause"/> flag), allowing DSP/FX to be set on it before any data reaches the callback function.
		/// <para>The sample data will generally arrive from the recording device in blocks rather than in a continuous stream, so when specifying a very short period between callbacks, some calls may be skipped due to there being no new data available since the last call.</para>
		/// <para>
        /// When not using a callback (proc = <see langword="null" />), the recorded data is instead retrieved via <see cref="ChannelGetData(int, IntPtr, int)" />.
        /// To keep latency at a minimum, the amount of data in the recording buffer should be monitored (also done via <see cref="ChannelGetData(int, IntPtr, int)" />, with the <see cref="DataFlags.Available"/> flag) to check that there is not too much data;
		/// freshly recorded data will only be retrieved after the older data in the buffer is.
        /// </para>
		/// <para><b>Platform-specific</b></para>
		/// <para>
        /// Multiple simultaneous recordings can be made from the same device on Windows XP and later, but generally not on older Windows.
        /// Multiple simultaneous recordings are possible on iOS and OSX, but may not always be on Linux or Windows CE.
		/// On OSX and iOS, the device is instructed (when possible) to deliver data at the period set in the HIWORD of flags, even when a callback function is not used.
        /// On other platforms, it is up the the system when data arrives from the device.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="RecordInit" /> has not been successfully called.</exception>
        /// <exception cref="Errors.DeviceBusy">
        /// The device is busy.
        /// An existing recording must be stopped before starting another one.
        /// Multiple simultaneous recordings can be made from the same device on Windows XP and Vista, but generally not on older Windows.
        /// </exception>
        /// <exception cref="Errors.NotAvailable">
        /// The recording device is not available.
        /// Another application may already be recording with it, or it could be a half-duplex device and is currently being used for playback.
        /// </exception>
        /// <exception cref="Errors.UnsupportedSampleFormat">
        /// The specified format is not supported.
        /// If using the <see cref="BassFlags.Float"/> flag, it could be that floating-point recording is not supported.
        /// </exception>
        /// <exception cref="Errors.Memory">There is insufficient memory.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static int RecordStart(int freq, int chans, BassFlags flags, RecordProcedure proc, IntPtr User = default(IntPtr))
        {
            return Checked(BASS_RecordStart(freq, chans, flags, proc, User));
        }

        /// <summary>
		/// Starts recording.
		/// </summary>
		/// <param name="freq">The sample rate to record at.</param>
		/// <param name="chans">The number of channels... 1 = mono, 2 = stereo.</param>
		/// <param name="flags">Any combination of <see cref="BassFlags.Byte"/>, <see cref="BassFlags.Float"/> and <see cref="BassFlags.RecordPause"/>.</param>
		/// <param name="period">
        /// Set the period (in milliseconds) between calls to the callback function (<see cref="RecordProcedure" />).
        /// The minimum period is 5ms, the maximum the maximum is half the <see cref="RecordingBufferLength"/> setting.
        /// If the period specified is outside this range, it is automatically capped. The default is 100ms.
        /// </param>
		/// <param name="proc">The user defined function to receive the recorded sample data... can be <see langword="null" /> if you do not wish to use a callback.</param>
		/// <param name="user">User instance data to pass to the callback function.</param>
		/// <returns>If successful, the new recording's handle is returned, else <see langword="false" /> is returned. Use <see cref="LastError"/> to get the error code.</returns>
        public static int RecordStart(int freq, int chans, BassFlags flags, int period, RecordProcedure proc, IntPtr user)
		{
			return RecordStart(freq, chans, (BassFlags)BitHelper.MakeLong((short)flags, (short)period), proc, user);
		}
        #endregion

        #region Current Recording Device
        [DllImport(DllName)]
        extern static int BASS_RecordGetDevice();

        [DllImport(DllName)]
        extern static bool BASS_RecordSetDevice(int Device);

        /// <summary>
		/// Gets or Sets the recording device setting in the current thread... 0 = first recording device.
        /// A value of -1 indicates error. Use <see cref="LastError" /> to get the error code.
		/// </summary>
        /// <remarks>
		/// <para>Simultaneously using multiple devices is supported in the BASS API via a context switching system - instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions. The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.</para>
		/// <para>The functions that use the recording device selection are the following: 
		/// <see cref="RecordFree" />, <see cref="RecordGetInfo(out RecordInfo)" />, <see cref="RecordGetInput(int, ref float)" />, <see cref="RecordGetInputName(int)" />, <see cref="RecordSetInput(int, InputFlags, float)" />, <see cref="RecordStart(int, int, BassFlags, RecordProcedure, IntPtr)" />.</para>
		/// <para>When one of the above functions (or <see cref="M:Un4seen.Bass.Bass.BASS_RecordGetDevice" />) is called, BASS will check the current thread's recording device setting, and if no device is selected (or the selected device is not initialized), BASS will automatically select the lowest device that is initialized. 
		/// This means that when using a single device, there is no need to use this function - BASS will automatically use the device that's initialized. 
		/// Even if you free the device, and initialize another, BASS will automatically switch to the one that is initialized.</para>
		/// </remarks>
        /// <exception cref="Errors.NotInitialised"><see cref="RecordInit" /> has not been successfully called - there are no initialized.</exception>
        /// <exception cref="Errors.IllegalDevice">Specified device number is invalid.</exception>
        /// <seealso cref="RecordInit"/>
        public static int CurrentRecordingDevice
        {
            get { return Checked(BASS_RecordGetDevice()); }
            set { Checked(BASS_RecordSetDevice(value)); }
        }
        #endregion

        #region Record Get Device Info
        /// <summary>
		/// Retrieves information on a recording device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <param name="Info">A <see cref="DeviceInfo" /> object to retreive the information into.</param>
		/// <returns>
        /// If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// This function does not show <see cref="BassException"/>.
        /// </returns>
		/// <remarks>
		/// This function can be used to enumerate the available recording devices for a setup dialog.
		/// <para><b>Platform-specific</b></para>
		/// <para>
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.IllegalDevice">The device number specified is invalid.</exception>
        /// <exception cref="Errors.DirectX">A sufficient version of DirectX is not installed.</exception>
        [DllImport(DllName, EntryPoint = "BASS_RecordGetDeviceInfo")]
        public static extern bool RecordGetDeviceInfo(int Device, out DeviceInfo Info);

        /// <summary>
		/// Retrieves information on a recording device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <returns>An instance of the <see cref="DeviceInfo" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para><b>Platform-specific</b></para>
		/// <para>
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.IllegalDevice">The device number specified is invalid.</exception>
        /// <exception cref="Errors.DirectX">A sufficient version of DirectX is not installed.</exception>
        public static DeviceInfo RecordGetDeviceInfo(int Device)
        {
            DeviceInfo info;
            Checked(RecordGetDeviceInfo(Device, out info));
            return info;
        }
        #endregion

        #region Record Get Info
	    /// <summary>
		/// Retrieves information on the recording device being used.
		/// </summary>
		/// <param name="info">A <see cref="RecordInfo" /> object to retrieve the information into.</param>
		/// <returns>
        /// If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// This function does not show <see cref="BassException"/>.
        /// </returns>
        /// <exception cref="Errors.NotInitialised"><see cref="RecordInit" /> has not been successfully called - there are no initialized devices.</exception>
        [DllImport(DllName, EntryPoint = "BASS_RecordGetInfo")]
        public static extern bool RecordGetInfo(out RecordInfo info);
        
		/// <summary>
		/// Retrieves information on the recording device being used.
		/// </summary>
		/// <returns>An instance of the <see cref="RecordInfo" /> is returned. Use <see cref="LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.NotInitialised"><see cref="RecordInit" /> has not been successfully called - there are no initialized devices.</exception>
		public static RecordInfo RecordingInfo
        {
            get
            {
                RecordInfo info;
                Checked(RecordGetInfo(out info));
                return info;
            }
        }
        #endregion

        /// <summary>
        /// The Buffer Length for recording channels in milliseconds... 1000 (min) - 5000 (max). default = 2000.
        /// </summary>
        /// <remarks>
        /// If the Length specified is outside this range, it is automatically capped.
        /// Unlike a playback Buffer, where the aim is to keep the Buffer full, a recording
        /// Buffer is kept as empty as possible and so this setting has no effect on latency.
        /// Unless processing of the recorded data could cause significant delays, or you want to
        /// use a large recording period with <see cref="RecordStart(int, int, BassFlags, RecordProcedure, IntPtr)"/>, there should be no need to increase this.
        /// Using this config option only affects the recording channels that are created afterwards,
        /// not any that have already been created.
        /// So you can have channels with differing Buffer lengths by using this config option each time before creating them.
        /// </remarks>
        public static int RecordingBufferLength
        {
            get { return GetConfig(Configuration.RecordingBufferLength); }
            set { Configure(Configuration.RecordingBufferLength, value); }
        }

        /// <summary>
        /// No of Recording devices available.
        /// </summary>
        public static int RecordingDeviceCount
        {
            get
            {
                int i;
                DeviceInfo info;

                for (i = 0; RecordGetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_RecordGetInput")]
        public extern static int RecordGetInput(int input, ref float volume);

        [DllImport(DllName)]
        extern static int BASS_RecordGetInput(int input, IntPtr volume);

        public static InputTypeFlags RecordGetInputType(int input)
        {
            int n = BASS_RecordGetInput(input, IntPtr.Zero);
            if (n == -1) return InputTypeFlags.Error;
            return (InputTypeFlags)(n & 0xff0000);
        }

        [DllImport(DllName, EntryPoint = "BASS_RecordGetInputName")]
        public extern static string RecordGetInputName(int input);

        [DllImport(DllName, EntryPoint = "BASS_RecordSetInput")]
        public extern static bool RecordSetInput(int input, InputFlags setting, float volume);
    }
}
