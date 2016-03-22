using System;

namespace ManagedBass
{
    /// <summary>
    /// Used with <see cref="Bass.RecordGetInput(int, out float)"/> applying <see cref="InputTypeMask"/> on the return value;
    /// </summary>
    [Flags]
    public enum InputTypeFlags
    {
        /// <summary>
        /// The Type of Input is also indicated in the high 8-bits of <see cref="Bass.RecordGetInput(int, out float)"/> (use this to test the return value).
        /// </summary>
        InputTypeMask = -16777216,

        /// <summary>
        /// The Type of Input is errorness.
        /// </summary>
        Error = -1,

        /// <summary>
        /// Anything that is not covered by the other types
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Digital Input source, for example, a DAT or audio CD.
        /// </summary>
        Digital = 16777216,

        /// <summary>
        /// Line-in. On some devices, "Line-in" may be combined with other analog sources into a single Analog Input.
        /// </summary>
        Line = 33554432,

        /// <summary>
        /// Microphone.
        /// </summary>
        Microphone = 50331648,

        /// <summary>
        /// Internal MIDI synthesizer.
        /// </summary>
        MIDISynthesizer = 67108864,

        /// <summary>
        /// Analog audio CD.
        /// </summary>
        AnalogCD = 83886080,

        /// <summary>
        /// Telephone.
        /// </summary>
        Phone = 100663296,

        /// <summary>
        /// PC speaker.
        /// </summary>
        Speaker = 117440512,

        /// <summary>
        /// The device's WAVE/PCM output.
        /// </summary>
        Wave = 134217728,

        /// <summary>
        /// Auxiliary. Like "Line-in", "Aux" may be combined with other analog sources into a single Analog Input on some devices.
        /// </summary>
        Auxiliary = 150994944,

        /// <summary>
        /// Analog, typically a mix of all analog sources (what you hear).
        /// </summary>
        Analog = 167772160,
    }
}
