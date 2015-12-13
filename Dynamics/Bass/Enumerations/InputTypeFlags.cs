﻿using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum InputTypeFlags
    {
        /// <summary>
        /// The type of input is also indicated in the high 8-bits of Bass.RecordGetInput() (use InputTypeMask to test the return value).
        /// </summary>
        InputTypeMask = -16777216,

        /// <summary>
        /// The type of input is errorness.
        /// </summary>
        Error = -1,

        /// <summary>
        /// Anything that is not covered by the other types
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Digital input source, for example, a DAT or audio CD.
        /// </summary>
        Digital = 16777216,

        /// <summary>
        /// Line-in. On some devices, "Line-in" may be combined with other analog sources into a single Analog input.
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
        /// Auxiliary. Like "Line-in", "Aux" may be combined with other analog sources into a single Analog input on some devices.
        /// </summary>
        Auxiliary = 150994944,

        /// <summary>
        /// Analog, typically a mix of all analog sources (what you hear).
        /// </summary>
        Analog = 167772160,
    }
}