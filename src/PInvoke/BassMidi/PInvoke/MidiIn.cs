#if __IOS__ || WINDOWS || LINUX || __MAC__
using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    public static partial class BassMidi
    {
        /// <summary>
        /// Frees a MIDI input device.
        /// </summary>
        /// <param name="Device">The device to free.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        /// <exception cref="Errors.Init">The device has not been initialized.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_InFree")]
        public static extern bool InFree(int Device);
        
		/// <summary>
		/// Retrieves information on a MIDI input device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <param name="Info">An instance of the <see cref="MidiDeviceInfo" /> class to store the information at.</param>
		/// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// This function can be used to enumerate the available MIDI input devices for a setup dialog. 
		/// <para><b>Platform-specific</b></para>
		/// <para>MIDI input is not available on Android.</para>
		/// </remarks>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_InGetDeviceInfo")]
        public static extern bool InGetDeviceInfo(int Device, out MidiDeviceInfo Info);
        
		/// <summary>
		/// Retrieves information on a MIDI input device.
		/// </summary>
		/// <param name="Device">The device to get the information of... 0 = first.</param>
		/// <returns>If successful, an instance of the <see cref="MidiDeviceInfo" /> structure is returned. Throws <see cref="BassException"/> on Error.</returns>
		/// <remarks>
        /// This function can be used to enumerate the available MIDI input devices for a setup dialog.
		/// <para><b>Platform-specific</b></para>
		/// <para>MIDI input is not available on Android.</para>
		/// </remarks>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        public static MidiDeviceInfo InGetDeviceInfo(int Device)
        {
            MidiDeviceInfo info;
            if (!InGetDeviceInfo(Device, out info))
                throw new BassException();
            return info;
        }

        /// <summary>
        /// Gets the number of MidiIn devices available.
        /// </summary>
        public static int InDeviceCount
        {
            get
            {
                int i;
                MidiDeviceInfo info;

                for (i = 0; InGetDeviceInfo(i, out info); i++) { }

                return i;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_InInit")]
        public static extern bool InInit(int device, MidiInProcedure proc, IntPtr user = default(IntPtr));
        
		/// <summary>
		/// Starts a MIDI input device.
		/// </summary>
		/// <param name="Device">The device to start.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        /// <exception cref="Errors.Init">The device has not been initialized.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_InStart")]
        public static extern bool InStart(int Device);
        
		/// <summary>
		/// Stops a MIDI input device.
		/// </summary>
		/// <param name="Device">The device to stop.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Device">The device number specified is invalid.</exception>
        /// <exception cref="Errors.Init">The device has not been initialized.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_InStop")]
        public static extern bool InStop(int Device);
    }
}
#endif