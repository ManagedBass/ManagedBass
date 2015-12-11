using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum SyncFlags
    {
        /// <summary>
        /// FLAG: sync only once, else continuously
        /// </summary>
        Onetime = -2147483648,

        /// <summary>
        /// Sync when a channel reaches a position.
        /// param : position in bytes
        /// data : not used
        /// </summary>
        Position = 0,

        /// <summary>
        /// Sync when an instrument (sample for the non-instrument based formats) is played in a MOD music (not including retrigs).
        /// param : LOWORD=instrument (1=first) HIWORD=note (0=c0...119=b9, -1=all)
        /// data : LOWORD=note HIWORD=volume (0-64)
        /// </summary>
        MusicInstrument = 1,
        //
        // Summary:
        //     Sync when a channel reaches the end.
        //     param : not used
        //     data : 1 = the sync is triggered by a backward jump in a MOD music, otherwise
        //     not used
        End = 2,
        //
        // Summary:
        //     Sync when the "sync" effect (XM/MTM/MOD: E8x/Wxx, IT/S3M: S2x) is used.
        //     param : 0:data=pos, 1:data="x" value
        //     data : param=0: LOWORD=order HIWORD=row, param=1: "x" value
        MusicFx = 3,
        //
        // Summary:
        //     Sync when metadata is received in a stream.
        //     param : not used
        //     data : not used - the updated metadata is available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag)
        //     (BASS_TAG_META)
        MetadataReceived = 4,
        //
        // Summary:
        //     Sync when an attribute slide is completed.
        //     param : not used
        //     data : the type of slide completed (one of the BASS_SLIDE_xxx values)
        Slided = 5,
        //
        // Summary:
        //     Sync when playback has stalled.
        //     param : not used
        //     data : 0=stalled, 1=resumed
        Stalled = 6,
        //
        // Summary:
        //     Sync when downloading of an internet (or "buffered" user file) stream has
        //     ended.
        //     param : not used
        //     data : not used
        Downloaded = 7,
        //
        // Summary:
        //     Sync when a channel is freed.
        //     param : not used
        //     data : not used
        Freed = 8,
        //
        // Summary:
        //     Sync when a MOD music reaches an order:row position.
        //     param : LOWORD=order (0=first, -1=all) HIWORD=row (0=first, -1=all)
        //     data : LOWORD=order HIWORD=row
        MusicPosition = 10,
        //
        // Summary:
        //     Sync when seeking (inc. looping and restarting). So it could be used to reset
        //     DSP/etc.
        //     param : position in bytes
        //     data : 0=playback is unbroken, 1=if is it broken (eg. buffer flushed). The
        //     latter would be the time to reset DSP/etc.
        Seeking = 11,
        //
        // Summary:
        //     Sync when a new logical bitstream begins in a chained OGG stream. Updated
        //     tags are available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used
        OggChange = 12,
        //
        // Summary:
        //     Sync when the DirectSound buffer fails during playback, eg. when the device
        //     is no longer available.
        //     param : not used
        //     data : not used
        Stop = 14,
        //
        // Summary:
        //     WINAMP add-on: Sync when bitrate is changed or retrieved from a winamp input
        //     plug-in.
        //     param : not used
        //     data : the bitrate retrieved from the winamp input plug-in - called when
        //     it is retrieved or changed (VBR MP3s, OGGs, etc).
        WinampBitRate = 100,
        //
        // Summary:
        //     CD add-on: Sync when playback is stopped due to an error. For example, the
        //     drive door being opened.
        //     param : not used
        //     data : the position that was being read from the CD track at the time.
        CDError = 1000,
        //
        // Summary:
        //     CD add-on: Sync when the read speed is automatically changed due to the BASS_CONFIG_CD_AUTOSPEED
        //     setting.
        //     param : not used
        //     data : the new read speed.
        CDSpeed = 1002,
        //
        // Summary:
        //     MIDI add-on: Sync when a marker is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_MARKER,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_MARKER, data) ...  End Sub
        MidiMarker = 65536,
        //
        // Summary:
        //     MIDI add-on: Sync when a cue is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_CUE,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_CUE, data) ...  End Sub
        MidiCue = 65537,
        //
        // Summary:
        //     MIDI add-on: Sync when a lyric event is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.  If the text begins with a '/' (slash) character, a new line should
        //     be started. If it begins with a '\' (backslash) character, the display should
        //     be cleared.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_MIDI_MARK_LYRIC,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_MIDI_MARK_LYRIC, data) ...  End Sub
        MidiLyric = 65538,
        //
        // Summary:
        //     MIDI add-on: Sync when a text event is encountered.
        //     param : not used
        //     data : the marker index, which can be used in a Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK)
        //     call.  Lyrics can sometimes be found in BASS_MIDI_MARK_TEXT instead of BASS_MIDI_MARK_LYRIC
        //     markers.
        //     void MidiSync(int Handle, int channel, int data, IntPtr User) { BASS_MIDI_MARK
        //     mark = BassMidi.BASS_MIDI_StreamGetMark(channel, BASSMIDIMarker.BASS_SYNC_MIDI_TEXT,
        //     data); ...  } Sub MidiSync(handle As Integer, channel As Integer,
        //     data As Integer, user As IntPtr) Dim mark As BASS_MIDI_MARK = BassMidi.BASS_MIDI_StreamGetMark(channel,
        //     BASSMIDIMarker.BASS_SYNC_MIDI_TEXT, data) ...  End Sub
        MidiText = 65539,
        //
        // Summary:
        //     MIDI add-on: Sync when a type of event is processed, in either a MIDI file
        //     or Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamEvent(System.Int32,System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIEvent,System.Int32).
        //     param : event type (0 = all types).
        //     data : LOWORD = event parameter, HIWORD = channel (high 8 bits contain the
        //     event type when syncing on all types). See Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamEvent(System.Int32,System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIEvent,System.Int32)
        //     for a list of event types and their parameters.
        MidiEvent = 65540,
        //
        // Summary:
        //     MIDI add-on: Sync when reaching a tick position.
        //     param : tick position.
        //     data : not used
        MidiTick = 65541,
        //
        // Summary:
        //     MIDI add-on: Sync when a time signature event is processed.
        //     param : event type.
        //     data : The time signature events are given (by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK))
        //     in the form of "numerator/denominator metronome-pulse 32nd-notes-per-MIDI-quarter-note",
        //     eg. "4/4 24 8".
        MidiTimeSignature = 65542,
        //
        // Summary:
        //     MIDI add-on: Sync when a key signature event is processed.
        //     param : event type.
        //     data : The key signature events are given (by Un4seen.Bass.AddOn.Midi.BassMidi.BASS_MIDI_StreamGetMark(System.Int32,Un4seen.Bass.AddOn.Midi.BASSMIDIMarker,System.Int32,Un4seen.Bass.AddOn.Midi.BASS_MIDI_MARK))
        //     in the form of "a b", where a is the number of sharps (if positive) or flats
        //     (if negative), and b signifies major (if 0) or minor (if 1).
        MidiKeySignature = 65543,
        //
        // Summary:
        //     WMA add-on: Sync on a track change in a server-side playlist. Updated tags
        //     are available via Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used
        WmaChange = 65792,
        //
        // Summary:
        //     WMA add-on: Sync on a mid-stream tag change in a server-side playlist. Updated
        //     tags are available via Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag).
        //     param : not used
        //     data : not used - the updated metadata is available from Un4seen.Bass.Bass.BASS_ChannelGetTags(System.Int32,Un4seen.Bass.BASSTag)
        //     (BASS_TAG_WMA_META)
        WmaMeta = 65793,
        //
        // Summary:
        //     MIX add-on: Sync when an envelope reaches the end.
        //     param : not used
        //     data : envelope type
        MixerEnvelope = 66048,
        //
        // Summary:
        //     MIX add-on: Sync when an envelope node is reached.
        //     param : Optional limit the sync to a certain envelope type (one of the Un4seen.Bass.AddOn.Mix.BASSMIXEnvelope
        //     values).
        //     data : Will contain the envelope type in the LOWORD and the current node
        //     number in the HIWORD.
        MixerEnvelopeNode = 66049,
        //
        // Summary:
        //     FLAG: sync at mixtime, else at playtime
        Mixtime = 1073741824,
    }
}