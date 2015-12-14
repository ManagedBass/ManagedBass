using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Wraps BassMidi: bassmidi.dll
    /// </summary>
    public static class BassMidi
    {
        const string DllName = "bassmidi.dll";

        static BassMidi() { BassManager.Load(DllName); }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateFile")]
        public static extern int CreateStream(bool mem, string file, long offset, long length, BassFlags flags, int freq);

        #region Configure
        /// <summary>
        /// Automatically compact all soundfonts following a configuration change?
        /// compact (bool): If true, all soundfonts are compacted following a MIDI stream being freed, or a BassMidi.BASS_MIDI_StreamSetFonts() call.
        /// The compacting isn't performed immediately upon a MIDI stream being freed or BassMidi.BASS_MIDI_StreamSetFonts() being called.
        /// It's actually done 2 seconds later (in another thread), 
        /// so that if another MIDI stream starts using the soundfonts in the meantime, they aren't needlessly closed and reopened.
        /// Samples that have been preloaded by BassMidi.BASS_MIDI_FontLoad() are not affected by automatic compacting.
        /// Other samples that have been preloaded by BassMidi.BASS_MIDI_StreamLoadSamples() are affected though,
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
        /// autofont (bool): If true, BASSMIDI will try to load a soundfont matching the MIDI file.
        /// This option only applies to local MIDI files, loaded using BassMidi.BASS_MIDI_StreamCreateFile()
        /// (or Bass.StreamCreateFile() via the plugin system). 
        /// BASSMIDI won't look for matching soundfonts for MIDI files loaded from the internet.
        /// By default, this option is enabled.
        /// </summary>
        public static bool AutoFont
        {
            get { return Bass.GetConfigBool(Configuration.MidiAutoFont); }
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
        /// The CPU usage of a MIDI stream can also be restricted via the ChannelAttribute.BASS_ATTRIB_MIDI_CPU attribute.
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
        /// The number of MIDI input ports to make available
        /// ports (int): Number of input ports... 0 (min) - 10 (max).
        /// MIDI input ports allow MIDI data to be received from other software, not only MIDI devices. 
        /// Once a port has been initialized via BassMidi.InInit(), the ALSA client and port IDs can be retrieved from BassMidi.InGetDeviceInfo(),
        /// which other software can use to connect to the port and send data to it.
        /// Prior to initialization, an input port will have a client ID of 0.
        /// The default is for 1 input port to be available. 
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
        /// When setting the default soundfont, a copy is made of the filename, so it does not need to persist beyond the Bass.Configure(IntPtr) call.
        /// If the specified soundfont cannot be loaded, the default soundfont setting will remain as it is. 
        /// Bass.GetConfigPtr() can be used to confirm what that is.
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
    }
}