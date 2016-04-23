using System.Runtime.InteropServices;

namespace ManagedBass
{
    public static partial class Bass
    {
        static readonly IOSNotifyProcedure iosnproc = status => _iosnotify?.Invoke(status);

        public static bool EnableAirplayReceivers(int receivers)
        {
            return Configure(Configuration.Airplay, receivers);
        }

        static event IOSNotifyProcedure _iosnotify;

        public static event IOSNotifyProcedure IOSNotification
        {
            add
            {
                if (_iosnotify == null)
                    Configure(Configuration.IOSNotify,
                              Marshal.GetFunctionPointerForDelegate(iosnproc));

                _iosnotify += value;
            }
            remove { _iosnotify -= value; }
        }

        /// <summary>
        /// Gets or Sets the iOS Audio session category management flags.
        /// </summary>
        public static IOSMixAudioFlags IOSMixAudio
        {
            get { return (IOSMixAudioFlags)GetConfig(Configuration.IOSMixAudio); }
            set { Configure(Configuration.IOSMixAudio, (int)value); }
        }

        /// <summary>
        /// When set to true, disables BASS' audio session category management so that the app can handle that itself.
        /// The <see cref="IOSMixAudio"/>, <see cref="IOSSpeaker"/> and <see cref="PlaybackBufferLength"/> settings will have no effect in that case.
        /// </summary>
        public static bool IOSNoCategory
        {
            get { return GetConfigBool(Configuration.IOSNoCategory); }
            set { Configure(Configuration.IOSNoCategory, value); }
        }

        /// <summary>
        /// When set to true, sends the output to the speaker instead of the receiver.
        /// Enables kAudioSessionPropert_OverrideCategoryDefaultToSpeaker.
        /// The default setting is 0 (off).
        /// </summary>
        public static bool IOSSpeaker
        {
            get { return GetConfigBool(Configuration.IOSSpeaker); }
            set { Configure(Configuration.IOSSpeaker, value); }
        }

        const int BASS_DEVICE_AIRPLAY = 0x1000000;

        /// <summary>
		/// Retrieves information on an Airplay Receiver.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <param name="Info">A <see cref="DeviceInfo" /> object to retrieve the information into.</param>
		/// <returns>
        /// If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned.
        /// Use <see cref="LastError" /> to get the error code.
        /// </returns>
		/// <remarks>
		/// This function can be used to enumerate the available devices for a setup dialog. 
		/// Device 0 is always the "no sound" device, so if you should start at device 1 if you only want to list real devices.
        /// <para><b>Platform-specific</b></para>
		/// <para>
        /// A shared buffer is used for the Airplay receiver name information, which gets overwritten each time Airplay receiver information is requested, so it should be copied if needed. 
		/// EnableAirplayReceivers" can be used to change which of the receiver(s) are used.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        public static bool GetDeviceInfoAirplay(int Device, out DeviceInfo Info) => GetDeviceInfo(Device | BASS_DEVICE_AIRPLAY, out Info);

        /// <summary>
        /// Retrieves information on an Airplay Receiver.
        /// </summary>
        /// <param name="Device">The device to get the information of... 0 = first.</param>
        /// <returns>An instance of the <see cref="DeviceInfo" /> structure is returned. Throws <see cref="BassException"/> on Error.</returns>
        /// <remarks>
        /// This function can be used to enumerate the available devices for a setup dialog. 
        /// Device 0 is always the "no sound" device, so if you should start at device 1 if you only want to list real devices.
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// A shared buffer is used for the Airplay receiver name information, which gets overwritten each time Airplay receiver information is requested, so it should be copied if needed. 
        /// EnableAirplayReceivers" can be used to change which of the receiver(s) are used.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        public static DeviceInfo GetDeviceInfoAirplay(int Device) => GetDeviceInfo(Device | BASS_DEVICE_AIRPLAY);

    }
}
