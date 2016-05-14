using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    public static partial class BassMidi
    {
		/// <summary>
		/// Compact a soundfont's memory usage.
		/// </summary>
		/// <param name="Handle">The soundfont to get info on (e.g. as returned by <see cref="FontInit(string, BassFlags)" />)... 0 = all soundfonts.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// Compacting involves freeing any samples that are currently loaded but unused.
        /// The amount of sample data currently loaded can be retrieved using <see cref="FontGetInfo(int, out MidiFontInfo)" />.
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontCompact")]
        public static extern bool FontCompact(int Handle);
        
		/// <summary>
		/// Frees a soundfont.
		/// </summary>
		/// <param name="Handle">The soundfont handle to free (e.g. as returned by <see cref="FontInit(string, BassFlags)" />).</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>When a soundfont is freed, it is automatically removed from any MIDI streams that are using it.</remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontFree")]
        public static extern bool FontFree(int Handle);

        /// <summary>
        /// Retrieves information on a soundfont.
        /// </summary>
        /// <param name="Handle">The soundfont to get info on (e.g. as returned by <see cref="FontInit(string, BassFlags)" />).</param>
        /// <param name="Info">An instance of <see cref="MidiFontInfo"/> structure to store the information into.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetInfo")]
        public static extern bool FontGetInfo(int Handle, out MidiFontInfo Info);

        /// <summary>
        /// Retrieves information on a soundfont.
        /// </summary>
        /// <param name="Handle">The soundfont to get info on (e.g. as returned by <see cref="FontInit(string, BassFlags)" />).</param>
        /// <returns>An instance of <see cref="MidiFontInfo"/> structure is returned. Throws <see cref="BassException"/> on Error.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        public static MidiFontInfo FontGetInfo(int Handle)
        {
            MidiFontInfo info;
            if (!FontGetInfo(Handle, out info))
                throw new BassException();
            return info;
        }

        [DllImport(DllName)]
        static extern IntPtr BASS_MIDI_FontGetPreset(int handle, int preset, int bank);

		/// <summary>
        /// Retrieves the name of a preset in a soundfont.
		/// </summary>
		/// <param name="Handle">The soundfont handle to get the preset name from.</param>
		/// <param name="Preset">Preset number to load... -1 = all presets (the first encountered).</param>
		/// <param name="Bank">Bank number to load... -1 = all banks (the first encountered).</param>
		/// <returns>If successful, the requested preset name is returned, else <see langword="null" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The soundfont does not contain the requested preset.</exception>
        public static string FontGetPreset(int Handle, int Preset, int Bank)
        {
            return Marshal.PtrToStringAnsi(BASS_MIDI_FontGetPreset(Handle, Preset, Bank));
        }
        
		/// <summary>
		/// Retrieves the presets in a soundfont.
		/// </summary>
		/// <param name="Handle">The soundfont handle to get the preset name from.</param>
        /// <param name="Presets">The array to receive the presets.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>The presets are delivered with the preset number in the LOWORD and the bank number in the HIWORD, and in numerically ascending order.</remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetPresets")]
        public static extern bool FontGetPresets(int Handle, [In, Out] int[] Presets);
                
		/// <summary>
		/// Retrieves the presets in a soundfont.
		/// </summary>
		/// <param name="Handle">The soundfont handle to get the preset name from.</param>
		/// <returns>If successful, an array of presets is returned, else <see langword="null" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>The presets are delivered with the preset number in the LOWORD and the bank number in the HIWORD, and in numerically ascending order.</remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        public static int[] FontGetPresets(int Handle)
        {
            MidiFontInfo info;
            
            if (!FontGetInfo(Handle, out info))
                return null;

            var ret = new int[info.presets];
            
            if (FontGetPresets(Handle, ret))
                return ret;

            return null;
        }
        
		/// <summary>
		/// Retrieves a soundfont's volume level.
		/// </summary>
		/// <param name="Handle">The soundfont to get the volume of.</param>
		/// <returns>If successful, the soundfont's volume level is returned, else -1 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetVolume")]
        public static extern float FontGetVolume(int Handle);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MIDI_FontInit(string File, BassFlags flags);

        public static int FontInit(string file, BassFlags flags)
        {
            return BASS_MIDI_FontInit(file, flags | BassFlags.Unicode);
        }

		/// <summary>
		/// Initializes a soundfont via user callback functions.
		/// </summary>
		/// <param name="Procedures">The user defined file function (see <see cref="FileProcedures" />).</param>
		/// <param name="User">User instance data to pass to the callback functions.</param>
		/// <param name="Flags">Unused.</param>
		/// <returns>If successful, the soundfont's handle is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>The unbuffered file system (<see cref="StreamSystem.NoBuffer"/>) is always used by this function.</remarks>
        /// <exception cref="Errors.FileFormat">The file's format is not recognised/supported.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontInitUser")]
        public static extern int FontInit([In, Out] FileProcedures Procedures, IntPtr User, BassFlags Flags);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontLoad")]
        public static extern bool FontLoad(int handle, int preset, int bank);

#if __IOS__ || WINDOWS || LINUX || __MAC__
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_MIDI_FontPack(int handle, string outfile, string encoder, BassFlags flags);

        public static bool FontPack(int handle, string outfile, string encoder, BassFlags flags)
        {
            return BASS_MIDI_FontPack(handle, outfile, encoder, flags | BassFlags.Unicode);
        }
#endif

        /// <summary>
        /// Sets a soundfont's volume level.
        /// </summary>
        /// <param name="Handle">The soundfont to set the volume of.</param>
        /// <param name="Volume">The volume level... 0=silent, 1.0=normal/default.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <remarks>
        /// By default, some soundfonts may be louder than others, which could be a problem when mixing multiple soundfonts. 
        /// This function can be used to compensate for any differences, by raising the level of the quieter soundfonts or lowering the louder ones.
        /// <para>Changes take immediate effect in any MIDI streams that are using the soundfont.</para>
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontSetVolume")]
        public static extern bool FontSetVolume(int Handle, float Volume);
        
		/// <summary>
		/// Unloads presets from a soundfont.
		/// </summary>
		/// <param name="Handle">The soundfont handle.</param>
		/// <param name="Preset">Preset number to load... -1 = all presets.</param>
		/// <param name="Bank">Bank number to load... -1 = all banks.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// An unloaded preset will be loaded again when needed by a MIDI stream.
        /// Any samples that are currently being used by a MIDI stream will not be unloaded.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The soundfont does not contain the specified preset, or the soundfont is memory mapped.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontUnload")]
        public static extern bool FontUnload(int Handle, int Preset, int Bank);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_MIDI_FontUnpack(int handle, string outfile, BassFlags flags);

        public static bool FontUnpack(int handle, string outfile, BassFlags flags)
        {
            return BASS_MIDI_FontUnpack(handle, outfile, flags | BassFlags.Unicode);
        }
    }
}