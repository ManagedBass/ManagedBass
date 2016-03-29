using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Midi
{
    /// <summary>
    /// Wraps BassMidi: bassmidi.dll
    /// 
    /// <para>Supports: .midi, .mid, .rmi, .kar</para>
    /// </summary>
    public static class BassMidi
    {
        const int BASS_MIDI_FONT_EX = 0x1000000;
        const string DllName = "bassmidi";
        static IntPtr hLib;

        public const int ChorusChannel = -1,
                         ReverbChannel = -2,
                         UserFXChannel = -3;

        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = Extensions.Load(DllName, Folder);

        public static void Unload() => Extensions.Unload(hLib);

        #region Create Stream
        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreate")]
        public static extern int CreateStream(int Channels, BassFlags Flags = BassFlags.Default, int Frequency = 44100);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateEvents")]
        public static extern int CreateStream(MidiEvent[] events, int ppqn, BassFlags flags = BassFlags.Default, int freq = 44100);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MIDI_StreamCreateFile(bool mem, string file, long offset, long length, BassFlags flags, int freq);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags, int freq);

        public static int CreateStream(string File, long Offset = 0, long Length = 0, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            return BASS_MIDI_StreamCreateFile(false, File, Offset, Length, Flags | BassFlags.Unicode, Frequency);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            return BASS_MIDI_StreamCreateFile(true, Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            var Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamCreateFileUser(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user, int freq);

        public static int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default(IntPtr), int freq = 44100)
        {
            var h = BASS_MIDI_StreamCreateFileUser(system, flags, procs, user, freq);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, procs);

            return h;
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MIDI_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100);

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100)
        {
            var h = BASS_MIDI_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User, Frequency);

            if (h != 0)
                Extensions.ChannelReferences.Add(h, 0, Procedure);

            return h;
        }
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamEvent")]
        public static extern bool StreamEvent(int handle, int chan, MidiEventType Event, int param);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamEvents(int handle, MidiEventsMode mode, MidiEvent[] Event, int length);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamEvents(int handle, MidiEventsMode mode, byte[] Event, int length);

        public static int StreamEvents(int handle, MidiEvent[] events, bool NoRunningStatus = false, bool Sync = false)
        {
            var flags = MidiEventsMode.Struct;
            if (NoRunningStatus) flags |= MidiEventsMode.NoRunningStatus;
            if (Sync) flags |= MidiEventsMode.Sync;

            return BASS_MIDI_StreamEvents(handle, flags, events, events.Length);
        }

        public static int StreamEvents(int handle, byte[] raw, int length = 0, bool NoRunningStatus = false, bool Sync = false)
        {
            var flags = MidiEventsMode.Raw;
            if (NoRunningStatus) flags |= MidiEventsMode.NoRunningStatus;
            if (Sync) flags |= MidiEventsMode.Sync;

            return BASS_MIDI_StreamEvents(handle, flags, raw, length == 0 ? raw.Length : length);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetChannel")]
        public static extern int StreamGetChannel(int handle, int chan);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetEvent")]
        public static extern int StreamGetEvent(int handle, int chan, MidiEventType Event);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamGetEvents(int handle, int track, int filter, [In, Out] MidiEvent[] events);

        public static MidiEvent[] StreamGetEvents(int handle, int track, int filter)
        {
            var count = BASS_MIDI_StreamGetEvents(handle, track, filter, null);

            if (count <= 0)
                return null;

            var events = new MidiEvent[count];

            BASS_MIDI_StreamGetEvents(handle, track, filter, events);

            return events;
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetFonts")]
        public static extern int StreamGetFonts(int handle, IntPtr fonts, int count);

        public static MidiFont[] StreamGetFonts(int handle, int count)
        {
            var arr = new MidiFont[count];

            var gch = GCHandle.Alloc(arr, GCHandleType.Pinned);

            StreamGetFonts(handle, gch.AddrOfPinnedObject(), count);

            gch.Free();

            return arr;
        }

        public static MidiFontEx[] StreamGetFontsEx(int handle, int count)
        {
            var arr = new MidiFontEx[count];

            var gch = GCHandle.Alloc(arr, GCHandleType.Pinned);

            StreamGetFonts(handle, gch.AddrOfPinnedObject(), count | BASS_MIDI_FONT_EX);

            gch.Free();

            return arr;
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetMark")]
        public static extern bool StreamGetMarker(int handle, MidiMarkerType type, int index, out MidiMarker mark);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetMarks")]
        public static extern int StreamGetMarkers(int handle, int track, MidiMarkerType type, [In, Out] MidiMarker[] marks);

        public static MidiMarker[] StreamGetAllMarkers(int handle)
        {
            var markc = StreamGetMarkers(handle, -1, MidiMarkerType.Marker, null);

            if (markc <= 0)
                return null;

            var marks = new MidiMarker[markc];
            StreamGetMarkers(handle, -1, MidiMarkerType.Marker, marks);

            return marks;
        }
        
		/// <summary>
		/// Preloads the samples required by a MIDI file stream.
		/// </summary>
		/// <param name="Handle">The MIDI stream handle.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// Samples are normally loaded as they are needed while rendering a MIDI stream, which can result in CPU spikes, particularly with packed soundfonts.
        /// That generally won't cause any problems, but when smooth/constant performance is critical this function can be used to preload the samples before rendering, so avoiding the need to load them while rendering.
        /// </para>
		/// <para>Preloaded samples can be compacted/unloaded just like any other samples, so it is probably wise to disable the <see cref="Compact"/> option when preloading samples, to avoid any chance of the samples subsequently being automatically unloaded.</para>
		/// <para>This function should not be used while the MIDI stream is being rendered, as that could interrupt the rendering.</para>
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The stream is for real-time events only, so it's not possible to know what presets are going to be used. Use <see cref="FontLoad" /> instead.</exception>
        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamLoadSamples")]
        public static extern bool StreamLoadSamples(int Handle);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamSetFonts")]
        public static extern int StreamSetFonts(int handle, MidiFont fonts, int count);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamSetFonts(int handle, MidiFontEx fonts, int count);

        public static int StreamSetFonts(int handle, MidiFontEx fonts, int count)
        {
            return BASS_MIDI_StreamSetFonts(handle, fonts, count | BASS_MIDI_FONT_EX);
        }

        #region Configure
        /// <summary>
        /// Automatically compact all soundfonts following a configuration change?
        /// compact (bool): If true, all soundfonts are compacted following a MIDI stream being freed, or a <see cref="StreamSetFonts(int,MidiFont,int)"/> call.
        /// The compacting isn't performed immediately upon a MIDI stream being freed or <see cref="StreamSetFonts(int,MidiFont,int)"/> being called.
        /// It's actually done 2 seconds later (in another thread), 
        /// so that if another MIDI stream starts using the soundfonts in the meantime, they aren't needlessly closed and reopened.
        /// Samples that have been preloaded by <see cref="FontLoad"/> are not affected by automatic compacting.
        /// Other samples that have been preloaded by <see cref="StreamLoadSamples"/> are affected though,
        /// so it is probably wise to disable this option when using that function.
        /// By default, this option is enabled.
        /// </summary>
        public static bool Compact
        {
            get { return Bass.GetConfigBool(Configuration.MidiCompact); }
            set { Bass.Configure(Configuration.MidiCompact, value); }
        }

        /// <summary>
        /// Automatically load matching soundfonts?
        /// If set to 1 (default), BASSMIDI will try to load a soundfont matching the MIDI file. If set to 2, the matching soundfont will also be used on all banks. 
        /// </summary>
        public static int AutoFont
        {
            get { return Bass.GetConfig(Configuration.MidiAutoFont); }
            set { Bass.Configure(Configuration.MidiAutoFont, value); }
        }

        /// <summary>
        /// The maximum number of samples to play at a time (polyphony).
        /// voices (int): Maximum number of samples to play at a time... 1 (min) - 1000 (max).
        /// This setting determines the maximum number of samples that can play together in a single MIDI stream. 
        /// This isn't necessarily the same thing as the maximum number of notes, due to presets often layering multiple samples. 
        /// When there are no voices available to play a new sample, the voice with the lowest volume will be killed to make way for it.
        /// The more voices that are used, the more CPU that is required. 
        /// So this option can be used to restrict that, for example on a less powerful system. 
        /// The CPU usage of a MIDI stream can also be restricted via the <see cref="ChannelAttribute.MidiCPU"/> attribute.
        /// Changing this setting only affects subsequently created MIDI streams, not any that have already been created. 
        /// The default setting is 128 voices.
        /// Platform-specific
        /// The default setting is 100, except on iOS, where it is 40.
        /// </summary>
        public static int Voices
        {
            get { return Bass.GetConfig(Configuration.MidiVoices); }
            set { Bass.Configure(Configuration.MidiVoices, value); }
        }

        /// <summary>
        /// The number of MIDI Input ports to make available
        /// ports (int): Number of Input ports... 0 (min) - 10 (max).
        /// MIDI Input ports allow MIDI data to be received from other software, not only MIDI devices. 
        /// Once a port has been initialized via <see cref="InInit"/>, the ALSA client and port IDs can be retrieved from <see cref="InGetDeviceInfo(int, out MidiDeviceInfo)"/>,
        /// which other software can use to connect to the port and send data to it.
        /// Prior to initialization, an Input port will have a client ID of 0.
        /// The default is for 1 Input port to be available. 
        /// Note: This option is only available on Linux.
        /// </summary>
        public static int InputPorts
        {
            get { return Bass.GetConfig(Configuration.MidiInputPorts); }
            set { Bass.Configure(Configuration.MidiInputPorts, value); }
        }

        /// <summary>
        /// Default soundfont usage
        /// filename (string): Filename of the default soundfont to use (null = no default soundfont).
        /// If the specified soundfont cannot be loaded, the default soundfont setting will remain as it is.
        /// On Windows, the default is to use one of the Creative soundfonts (28MBGM.SF2 or CT8MGM.SF2 or CT4MGM.SF2 or CT2MGM.SF2),
        /// if present in the windows system directory.
        /// </summary>
        public static string DefaultFont
        {
            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.MidiDefaultFont)); }
            set
            {
                var ptr = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.MidiDefaultFont, ptr);

                Marshal.FreeHGlobal(ptr);
            }
        }
        #endregion

        #region SoundFonts
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
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontComapct")]
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

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetInfo")]
        public static extern bool FontGetInfo(int handle, out MidiFontInfo info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>An instance of <see cref="MidiFontInfo"/> structure is returned. Throws <see cref="BassException"/> on Error.</returns>
        public static MidiFontInfo FontGetInfo(int handle)
        {
            MidiFontInfo info;
            if (!FontGetInfo(handle, out info))
                throw new BassException();
            return info;
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetPreset")]
        public static extern string FontGetPreset(int handle, int preset, int bank);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetPresets")]
        public static extern bool FontGetPresets(int handle, [In, Out] int[] presets);
        
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

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_MIDI_FontPack(int handle, string outfile, string encoder, BassFlags flags);

        public static bool FontPack(int handle, string outfile, string encoder, BassFlags flags)
        {
            return BASS_MIDI_FontPack(handle, outfile, encoder, flags | BassFlags.Unicode);
        }
        
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
        #endregion

        #region In
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
        #endregion
    }
}