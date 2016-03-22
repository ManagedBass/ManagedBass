using System;

namespace ManagedBass
{
    /// <summary>
    /// Channel Position Mode flags to be used with e.g. <see cref="Bass.ChannelGetLength" />, <see cref="Bass.ChannelGetPosition" />,
    /// <see cref="Bass.ChannelSetPosition" /> or <see cref="Bass.StreamGetFilePosition" />.
    /// </summary>
    [Flags]
    public enum PositionFlags
    {
        /// <summary>
        /// Byte position.
        /// </summary>
        Bytes = 0,

        /// <summary>
        /// Order.Row position (HMUSIC only).
        /// LoWord = order, HiWord = row * scaler (<see cref="ChannelAttribute.MusicPositionScaler" />).
        /// </summary>
        MusicOrders = 1,

        /// <summary>
        /// Tick position (MIDI streams only).
        /// </summary>
        MIDITick = 2,

        /// <summary>
        /// OGG bitstream number.
        /// </summary>
        OGG = 3,

        /// <summary>
        /// CD Add-On: the track number.
        /// </summary>
        CDTrack = 4,

        /// <summary>
        /// Midi Add-On: Let the old sound decay naturally (including reverb) when changing the position,
        /// including looping and such can also be used in <see cref="Bass.ChannelSetPosition"/> calls to have it apply to particular position changes.
        /// </summary>
        MIDIDecaySeek = 16384,

        /// <summary>
        /// MOD Music Flag: Stop all notes when moving position.
        /// </summary>
        MusicPositionReset = 32768,

        /// <summary>
        /// MOD Music Flag: Stop all notes and reset bmp/etc when moving position.
        /// </summary>
        MusicPositionResetEx = 4194304,

        /// <summary>
        /// Mixer Flag: Don't ramp-in the start after seeking.
        /// </summary>
        MixerNoRampIn = 8388608,

        /// <summary>
        /// Flag: Allow inexact seeking.
        /// For speed, seeking may stop at the beginning of a block rather than partially processing the block to reach the requested position.
        /// </summary>
        Inexact = 134217728,

        /// <summary>
        /// Get the decoding (not playing) position.
        /// </summary>
        Decode = 268435456,

        /// <summary>
        /// Flag: decode to the position instead of seeking.
        /// </summary>
        DecodeTo = 536870912,

        /// <summary>
        /// Scan the file to build a seek table up to the position, if it has not already been scanned.
        /// Scanning will continue from where it left off previously rather than restarting from the beginning of the file each time.
        /// This flag only applies to MP3/MP2/MP1 files and will be ignored with other file formats.
        /// </summary>
        Scan = 1073741824,
    }
}
