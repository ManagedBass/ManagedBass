using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public enum MidiEventSequence
    {        
        /// <summary>
        /// Used with BassMidi.CreateStream(MidiEvent[]) to mark the end of the event array.
        /// </summary>
        End = 0,
        
        /// <summary>
        /// No event.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Press or release a key, or stop without sustain/decay.
        /// param : LOBYTE = key number (0-127, 60=middle C), HIBYTE = velocity (0=release, 1-127=press, 255=stop).
        /// </summary>
        Note = 1,
        
        /// <summary>
        /// Select the preset/instrument to use. Standard soundfont presets follow the
        /// General MIDI standard, and generally also include Roland GS variations in other banks (accessible via the MIDI_EVENT_BANK event).
        /// param : preset number (0-127).
        /// </summary>
        Program = 2,
        
        /// <summary>
        /// Set the channel pressure.
        /// param : pressure level (0-127).
        /// </summary>
        ChannelPressure = 3,
        
        /// <summary>
        /// Set the pitch wheel.
        /// param : pitch wheel position (0-16383, 8192=normal/middle).
        /// </summary>
        Pitch = 4,
        
        /// <summary>
        /// Set pitch wheel range (MIDI RPN 0).
        /// param : range in semitones.
        /// </summary>
        PitchRange = 5,
        
        /// <summary>
        /// Set the percussion/drums channel switch. 
        /// The bank and program are reset to 0 when this changes.
        /// param : use drums? (0=no, 1=yes).
        /// </summary>
        Drums = 6,
        
        /// <summary>
        /// Set the fine tuning (MIDI RPN 1).
        /// param : finetune in cents (0-16383, 0=-100, 8192=normal, 16383=+100).
        /// </summary>
        FineTune = 7,
        
        /// <summary>
        /// Set the coarse tuning (MIDI RPN 2).
        /// param : finetune in semitones (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        CoarseTune = 8,
        
        /// <summary>
        /// Set the master volume.
        /// param : volume level (0-16383, 0=silent, 16363=normal/full).
        /// </summary>
        MasterVolume = 9,
        
        /// <summary>
        /// Select the bank to use (MIDI controller 0).
        /// param : bank number (0-127).
        /// </summary>
        Bank = 10,
        
        /// <summary>
        /// Set the modulation (MIDI controller 1).
        /// param : modulation level (0-127).
        /// </summary>
        Modulation = 11,
        
        /// <summary>
        /// Set the volume (MIDI controller 7).
        /// param : volume level (0-127).
        /// </summary>
        Volume = 12,
        
        /// <summary>
        /// Set the pan position (MIDI controller 10).
        /// param : pan position (0-128, 0=left, 64=middle, 127=right, 128=random).
        /// </summary>
        Pan = 13,
        
        /// <summary>
        /// Set the expression (MIDI controller 11).
        /// param : expression level (0-127).
        /// </summary>
        Expression = 14,
        
        /// <summary>
        /// Set the sustain switch (MIDI controller 64).
        /// param : enable sustain? (0-63=no, 64-127=yes).
        /// </summary>
        Sustain = 15,
        
        /// <summary>
        /// Stop all sounds (MIDI controller 120).
        /// param : not used.
        /// </summary>
        SoundOff = 16,
        
        /// <summary>
        /// Reset controllers (MIDI controller 121), that is modulation=0, expression=127,
        /// sustain=0, pitch wheel=8192, channel pressure=0.
        /// param : not used.
        /// </summary>
        Reset = 17,
        
        /// <summary>
        /// Release all keys (MIDI controller 123).
        /// param : not used.
        /// </summary>
        NotesOff = 18,
        
        /// <summary>
        /// Set the portamento switch (MIDI controller 65).
        /// param : enable portamento? (0-63=no, 64-127=yes).
        /// </summary>
        Portamento = 19,
        
        /// <summary>
        /// Set the portamento time (MIDI controller 5).
        /// param : portamento time (0-127).
        /// </summary>
        PortamentoTime = 20,
        
        /// <summary>
        /// Set the portamento start key - the next note starts at this key (MIDI controller 84).
        /// param : key number (1-127, 60=middle C).
        /// </summary>
        PortamentoNote = 21,
        
        /// <summary>
        /// Set poly/mono mode (MIDI controllers 126 & 127).
        /// param : mode (0=poly, 1=mono).
        /// </summary>
        Mode = 22,
        
        /// <summary>
        /// Set the reverb send level (MIDI controller 91).
        /// param : reverb level (0-127).
        /// </summary>
        Reverb = 23,
        
        /// <summary>
        /// Set the chorus send level (MIDI controller 93).
        /// param : chorus level (0-127).
        /// </summary>
        Chorus = 24,
        
        /// <summary>
        /// Set the low-pass filter cutoff (MIDI controller 74, NRPN 120h).
        /// param : cutoff level (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        CutOff = 25,
        
        /// <summary>
        /// Set the low-pass filter resonance (MIDI controller 71, NRPN 121h).
        /// param : resonance level (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        Resonance = 26,
        
        /// <summary>
        /// Set the release time (MIDI controller 72, NRPN 166h).
        /// param : release time (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        Release = 27,
        
        /// <summary>
        /// Set the attack time (MIDI controller 73, NRPN 163h).
        /// param : attack time (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        Attack = 28,
        
        /// <summary>
        /// To be defined.
        /// param : to be defined.
        /// </summary>
        ReverbMacro = 30,
        
        /// <summary>
        /// To be defined.
        /// param : to be defined.
        /// </summary>
        ChorusMacro = 31,
        
        /// <summary>
        /// Set the reverb time.
        /// param : reverb time in milliseconds.
        /// </summary>
        ReverbTime = 32,
        
        /// <summary>
        /// Set the reverb delay.
        /// param : reverb delay in millisecond 10ths.
        /// </summary>
        ReverbDelay = 33,
        
        /// <summary>
        /// Set the reverb low-pass cutoff.
        /// param : reverb low-pass cutoff in hertz (0=off).
        /// </summary>
        ReverbLowPassCutOff = 34,
        
        /// <summary>
        /// Set the reverb high-pass cutoff.
        /// param : reverb high-pass cutoff in hertz (0=off).
        /// </summary>
        ReverbHighPassCutOff = 35,
        
        /// <summary>
        /// Set the reverb level.
        /// param : reverb level (0=off, 100=0dB, 200=+6dB).
        /// </summary>
        ReverbLevel = 36,
        
        /// <summary>
        /// Set the chorus delay.
        /// param : chorus delay in millisecond 10ths.
        /// </summary>
        ChorusDelay = 37,
        
        /// <summary>
        /// Set the chorus depth.
        /// param : chorus depth in millisecond 10ths.
        /// </summary>
        ChorusDepth = 38,
        
        /// <summary>
        /// Set the chorus rate.
        /// param : chorus rate in hertz 100ths.
        /// </summary>
        ChorusRate = 39,
        
        /// <summary>
        /// Set the chorus feedback level.
        /// param : chorus feedback level (0=-100%, 100=off, 200=+100%).
        /// </summary>
        ChorusFeedback = 40,
        
        /// <summary>
        /// Set the chorus level.
        /// param : chorus level (0=off, 100=0dB, 200=+6dB).
        /// </summary>
        ChorusLevel = 41,
        
        /// <summary>
        /// Set the chorus send to reverb level.
        /// param : chorus send to reverb level (0=off, 100=0dB, 200=+6dB).
        /// </summary>
        ChorusReverb = 42,
        
        /// <summary>
        /// Set the fine tuning of a drum key (MIDI NRPN 19knh).
        /// param : LOBYTE = key number (0-127), HIBYTE = finetune in cents (0-127, 0=-100, 64=normal, 127=+100).
        /// </summary>
        DrumFineTune = 50,
        
        /// <summary>
        /// Set the coarse tuning of a drum key (MIDI NRPN 18knh).
        /// param : LOBYTE = key number (0-127), HIBYTE = finetune in semitones (0-127, 0=-64, 64=normal, 127=+63).
        /// </summary>
        DrumCoarseTune = 51,
        
        /// <summary>
        /// Set the pan position of a drum key (MIDI NRPN 1Cknh).
        /// param : LOBYTE = key number (0-127), HIBYTE = pan position (0-127, 0=random, 64=middle).
        /// </summary>
        DrumPan = 52,
        
        /// <summary>
        /// Set the reverb send level of a drum key (MIDI NRPN 1Dknh).
        /// param : LOBYTE = key number (0-127), HIBYTE = reverb level (0-127).
        /// </summary>
        DrumReverb = 53,
        //
        // Summary:
        //     Set the chorus send level of a drum key (MIDI NRPN 1Eknh).
        //     param : LOBYTE = key number (0-127), HIBYTE = chorus level (0-127).
        DrumChorus = 54,
        //
        // Summary:
        //     Set the low-pass filter cutoff of a drum key (MIDI NRPN 14knh).
        //     param : LOBYTE = key number (0-127), HIBYTE = cutoff level (0-127, 0=-64,
        //     64=normal, 127=+63).
        DrumCutOff = 55,
        //
        // Summary:
        //     Set the low-pass filter resonance of a drum key (MIDI NRPN 15knh).
        //     param : LOBYTE = key number (0-127), HIBYTE = resonance level (0-127, 0=-64,
        //     64=normal, 127=+63).
        DrumResonance = 56,
        //
        // Summary:
        //     Set the drum level NRPN of a drum key (MIDI NRPN 16knh).
        //     param : LOBYTE = key number (0-127), HIBYTE = level (0-127, 127=full/normal).
        DrumLevel = 57,
        //
        // Summary:
        //     Set the soft pedal/switch (MIDI controller 67).
        //     param : soft is on? (0-63=no, 64-127=yes).
        Soft = 60,
        //
        // Summary:
        //     Set the system mode, resetting everything to the system's defaults.  MIDI_SYSTEM_DEFAULT
        //     is identical to MIDI_SYSTEM_GS, except that channel 10 is melodic if there
        //     are not 16 channels. MIDI_EVENT_SYSTEM does not reset things in any additional
        //     channels allocated to a MIDI file stream via the Un4seen.Bass.BASSAttribute.BASS_ATTRIB_MIDI_CHANS
        //     attribute, while MIDI_EVENT_SYSTEMEX does.
        //     param : system mode (see Un4seen.Bass.AddOn.Midi.BASSMIDISystem).
        System = 61,
        //
        // Summary:
        //     Set the tempo (MIDI meta event 81). Changing the tempo affects the stream
        //     length, and the Un4seen.Bass.Bass.BASS_ChannelGetLength(System.Int32,Un4seen.Bass.BASSMode)
        //     value will no longer be valid.
        //     param : tempo in microseconds per quarter note.
        Tempo = 62,
        //
        // Summary:
        //     Set the tuning of a note in every octave.
        //     param : LOWORD = tuning change in cents (0-16383, 0=-100, 8192=normal, 16383=+100),
        //     HIWORD = note (0-11, 0=C).
        ScaleTuning = 63,
        
        /// <summary>
        /// Control event.
        /// </summary>
        Control = 64,
        
        /// <summary>
        /// Change Preset Vibrato.
        /// </summary>
        ChangePresetVibrato = 65,
        
        /// <summary>
        /// Change Preset Pitch.
        /// </summary>
        ChangePresetPitch = 66,
        
        /// <summary>
        /// Change Preset Filter.
        /// </summary>
        ChangePresetFilter = 67,
        
        /// <summary>
        /// Change Preset Volume.
        /// </summary>
        ChangePresetVolume = 68,
        
        /// <summary>
        /// Mod Range.
        /// </summary>
        ModRange = 69,
        
        /// <summary>
        /// Bank LSB.
        /// </summary>
        BankLSB = 70,
        
        /// <summary>
        /// Set the level.
        /// param : the level (0=silent, 100=0dB, 200=+6dB).
        /// </summary>
        Level = 65536,
        
        /// <summary>
        /// Transpose all notes. 
        /// Changes take effect from the next note played, and affect melodic channels only (not drum channels).
        /// param : transposition amount in semitones (0=-100, 100=normal, 200=+100).
        /// </summary>
        Transpose = 65537,
        
        /// <summary>
        /// Set the system mode, resetting everything to the system's defaults. 
        /// MIDI_SYSTEM_DEFAULT is identical to MIDI_SYSTEM_GS, except that channel 10 is melodic if there are not 16 channels.
        /// MIDI_EVENT_SYSTEM does not reset things in any additional channels allocated to a MIDI file stream 
        /// via the ChannelAttribute.MidiChannels attribute, while MIDI_EVENT_SYSTEMEX does.
        /// param : system mode (see Un4seen.Bass.AddOn.Midi.BASSMIDISystem).
        /// </summary>
        SystemEx = 65538,
        
        /// <summary>
        /// Used with BassMidi.StreamCreateEvents() to mark the end of a track (the next event will be in a new track).
        /// </summary>
        EndTrack = 65539,
        
        /// <summary>
        /// Flag: no running status.
        /// </summary>
        NoRunningStatus = 33554432,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MidiEvent
    {
        int Event;
        int param;
        int chan;
        int tick;
        int pos;
    }

    /// <summary>
    /// Wraps BassMidi: bassmidi.dll
    /// </summary>
    public static class BassMidi
    {
        const string DllName = "bassmidi.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

        #region Create Stream
        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreate")]
        public static extern int CreateStream(int Channels, BassFlags Flags, int Frequency);

        [DllImport(DllName, EntryPoint="BASS_MIDI_StreamCreateEvents")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPArray)] MidiEvent events, int ppqn, BassFlags flags, int freq);

        [DllImport(DllName)]
        public static extern int BASS_MIDI_StreamCreateFile(bool mem, [MarshalAs(UnmanagedType.LPWStr)]string file, long offset, long length, BassFlags flags, int freq);

        [DllImport(DllName)]
        public static extern int BASS_MIDI_StreamCreateFile(bool mem, IntPtr file, long offset, long length, BassFlags flags, int freq);

        public static int CreateStream(string File, long Offset, long Length, BassFlags Flags, int Frequency)
        {
            return BASS_MIDI_StreamCreateFile(false, File, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(IntPtr Memory, long Offset, long Length, BassFlags Flags, int Frequency)
        {
            return BASS_MIDI_StreamCreateFile(true, Memory, Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(byte[] Memory, long Offset, long Length, BassFlags Flags, int Frequency)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            return CreateStream(GCPin.AddrOfPinnedObject(), Offset, Length, Flags, Frequency);
        }

        public static int CreateStream(Stream Stream, int Offset, int Length, BassFlags Flags, int Frequency)
        {
            var buffer = new byte[Length];

            Stream.Read(buffer, Offset, Length);

            return CreateStream(buffer, 0, Length, Flags, Frequency);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateFileUser")]
        public static extern int CreateStream(int system, BassFlags flags, FileProcedures procs, IntPtr user, int Frequency);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateURL")]
        public static extern int CreateStream([MarshalAs(UnmanagedType.LPWStr)]string Url, int Offset, BassFlags Flags, DownloadProcedure Procedure, IntPtr User = default(IntPtr), int Frequency = 44100);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamEvent")]
        public static extern bool StreamEvent(int handle, int chan, MidiEventSequence Event, int param);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamEvents")]
        public static extern int StreamEvents(int handle, MidiEventSequence mode, [MarshalAs(UnmanagedType.LPArray)] MidiEvent[] Event, int length);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetChannel")]
        public static extern int StreamGetChannel(int handle, int chan);

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamGetEvent")]
        public static extern int StreamGetEvent(int handle, int chan, MidiEventSequence Event);

        [DllImport(DllName)]
        static extern int BASS_MIDI_StreamGetEvents(int handle, int track, int filter, [MarshalAs(UnmanagedType.LPArray)] MidiEvent[] events);

        public static MidiEvent[] StreamGetEvents(int handle, int track, int filter)
        {
            int count = BASS_MIDI_StreamGetEvents(handle, track, filter, null);

            var events = new MidiEvent[count];

            BASS_MIDI_StreamGetEvents(handle, track, filter, events);

            return events;
        }

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

        // TODO: Config AutoFont Update according to Documentation: int
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