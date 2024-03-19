using System;
using System.Runtime.InteropServices;

namespace ManagedBass.loud
{
    public static class Bassloud
    {
        const string DllName = "bassloud";

        #region Version
        [DllImport(DllName)]
        static extern int BASS_Loudness_GetVersion();

        /// <summary>
        /// Gets the Version of BassFx that is loaded.
        /// </summary>
        public static Version Version => Extensions.GetVersion(BASS_Loudness_GetVersion());
        #endregion

        /// <summary>
        /// Retrieves the channel that a loudness measurement is set on.
        /// </summary>
        /// <param name="Handle">The loudness measurement.</param>
        /// <returns>If successful, the loudness measurement's channel handle is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Loudness_GetChannel")]
        public static extern int BASS_Loudness_GetChannel(int Handle);

        /// <summary>
        /// Retrieves the level of a loudness measurement.
        /// </summary>
        /// <param name="Handle">The loudness measurement handle.</param>
        /// <param name="Mode">The measurement type to retrieve. One of the following.</param>
        /// <param name="Level">Pointer to a variable to receive the measurement level.</param>
        /// <returns>If successful, TRUE is returned, else FALSE is returned. Use BASS_ErrorGetCode to get the error code.</returns>
        /// <exception cref="Errors.Handle"></exception>
        /// <exception cref="Errors.Parameter"></exception>
        /// <exception cref="Errors.NotAvailable"></exception>
        [DllImport(DllName, EntryPoint = "BASS_Loudness_GetLevel")]
        public static extern bool BASS_Loudness_GetLevel(int Handle, BassFlags Mode, float Level);

        /// <summary>
        /// Starts loudness measurement on a channel.
        /// </summary>
        /// <param name="Handle"></param>
        /// <param name="Flags"></param>
        /// <param name="Priority"></param>
        /// <returns>The loudness measurement handle is returned if it is successfully started, else 0 is returned. Use BASS_ErrorGetCode to get the error code.</returns>
        /// <exception cref="Errors.Handle"></exception>
        /// <exception cref="Errors.Memory"></exception>
        [DllImport(DllName, EntryPoint = "BASS_Loudness_Start")]
        public static extern int BASS_Loudness_Start(int Handle, BassFlags Flags, int Priority);

        /// <summary>
        /// Stops a loudness measurement or all loudness measurements on a channel.
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns>If successful, TRUE is returned, else FALSE is returned. Use BASS_ErrorGetCode to get the error code.</returns>
        /// <exception cref="Errors.Handle"></exception>
        [DllImport(DllName, EntryPoint = "BASS_Loudness_Stop")]
        public static extern bool BASS_Loudness_Stop(int Handle);
    }
}