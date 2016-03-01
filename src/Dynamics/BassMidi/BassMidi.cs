using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
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

        #region From Array
        static int CreateStreamObj(object Memory, long Offset, long Length, BassFlags Flags, int Frequency)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            int Handle = CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(short[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(int[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(float[] Memory, long Offset, long Length, BassFlags Flags, int Frequency = 44100)
        {
            return CreateStreamObj(Memory, Offset, Length, Flags, Frequency);
        }
        #endregion

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags = BassFlags.Default, int Frequency = 44100)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags, Frequency);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateFileUser")]
        public static extern int CreateStream(StreamSystem system, BassFlags flags, [In, Out] FileProcedures procs, IntPtr user = default(IntPtr), int freq = 44100);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MIDI_StreamCreateURL(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100);

        public static int CreateStream(string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100)
        {
            return BASS_MIDI_StreamCreateURL(Url, Offset, Flags | BassFlags.Unicode, Procedure, User, Frequency);
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
            int count = BASS_MIDI_StreamGetEvents(handle, track, filter, null);

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
            int markc = StreamGetMarkers(handle, -1, MidiMarkerType.Marker, null);
            var marks = new MidiMarker[markc];
            StreamGetMarkers(handle, -1, MidiMarkerType.Marker, marks);

            return marks;
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamLoadSamples")]
        public static extern bool StreamLoadSamples(int handle);

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
                IntPtr ptr = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.MidiDefaultFont, ptr);

                Marshal.FreeHGlobal(ptr);
            }
        }
        #endregion

        #region SoundFonts
        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontComapct")]
        public static extern bool FontCompact(int handle);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontFree")]
        public static extern bool FontFree(int handle);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetInfo")]
        public static extern bool FontGetInfo(int handle, out MidiFontInfo info);

        public static MidiFontInfo FontGetInfo(int handle)
        {
            MidiFontInfo info;
            FontGetInfo(handle, out info);
            return info;
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetPreset")]
        public static extern string FontGetPreset(int handle, int preset, int bank);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetPresets")]
        public static extern bool FontGetPresets(int handle, [In, Out] int[] presets);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontGetVolume")]
        public static extern float FontGetVolume(int handle);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_MIDI_FontInit(string File, BassFlags flags);

        public static int FontInit(string file, BassFlags flags)
        {
            return BASS_MIDI_FontInit(file, flags | BassFlags.Unicode);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontInitUser")]
        public static extern int FontInit(FileProcedures procs, IntPtr user, BassFlags flags);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontLoad")]
        public static extern bool FontLoad(int handle, int preset, int bank);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_MIDI_FontPack(int handle, string outfile, string encoder, BassFlags flags);

        public static bool FontPack(int handle, string outfile, string encoder, BassFlags flags)
        {
            return BASS_MIDI_FontPack(handle, outfile, encoder, flags | BassFlags.Unicode);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontSetVolume")]
        public static extern bool FontSetVolume(int handle, float volume);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_FontUnload")]
        public static extern bool FontUnload(int handle, int preset, int bank);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_MIDI_FontUnpack(int handle, string outfile, BassFlags flags);

        public static bool FontUnpack(int handle, string outfile, BassFlags flags)
        {
            return BASS_MIDI_FontUnpack(handle, outfile, flags | BassFlags.Unicode);
        }
        #endregion

        #region In
        [DllImport(DllName, EntryPoint = "BASS_MIDI_InFree")]
        public static extern bool InFree(int device);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_InGetDeviceInfo")]
        public static extern bool InGetDeviceInfo(int device, out MidiDeviceInfo info);

        public static MidiDeviceInfo InGetDeviceInfo(int device)
        {
            MidiDeviceInfo info;
            InGetDeviceInfo(device, out info);
            return info;
        }

        public static int InDeviceCount
        {
            get
            {
                int i;
                MidiDeviceInfo info;

                for (i = 0; InGetDeviceInfo(i, out info); i++) ;

                return i;
            }
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_InInit")]
        public static extern bool InInit(int device, MidiInProcedure proc, IntPtr user = default(IntPtr));

        [DllImport(DllName, EntryPoint = "BASS_MIDI_InStart")]
        public static extern bool InStart(int device);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_InStop")]
        public static extern bool InStop(int device);
        #endregion
    }
}