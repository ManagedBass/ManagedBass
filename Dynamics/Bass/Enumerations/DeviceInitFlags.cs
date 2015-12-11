using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum DeviceInitFlags
    {
        /// <summary>
        /// 0 = 16 bit, stereo, no 3D, no Latency calc, no Speaker Assignments
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use 8 bit resolution, else 16 bit.
        /// </summary>
        Byte = 1,

        /// <summary>
        /// Use mono, else stereo.
        /// </summary>
        Mono = 2,

        /// <summary>
        /// Enable 3D functionality.
        /// Note: If the BASS_DEVICE_3D flag is not specified when initilizing BASS,
        /// then the 3D flags (BASS_SAMPLE_3D and BASS_MUSIC_3D) are ignored when loading/creating a sample/stream/music.
        /// </summary>
        Device3D = 4,

        /// <summary>
        /// Calculate device latency (BassInfo struct).
        /// </summary>
        Latency = 256,

        /// <summary>
        /// Use the Windows control panel setting to detect the number of speakers.
        /// Only use this option if BASS doesn't detect the correct number of supported
        /// speakers automatically and you want to force BASS to use the number of speakers
        /// as configured in the windows control panel.
        /// </summary>
        CPSpeakers = 1024,

        /// <summary>
        /// Force enabling of speaker assignment (always 8 speakers will be used regardless if the soundcard supports them).
        /// Only use this option if BASS doesn't detect the correct number of supported
        /// speakers automatically and you want to force BASS to use 8 speakers.
        /// </summary>
        ForcedSpeakerAssignment = 2048,

        /// <summary>
        /// Ignore speaker arrangement
        /// </summary>
        NoSpeakerAssignment = 4096,

        /// <summary>
        /// Set the device's output rate to freq, otherwise leave it as it is.
        /// </summary>
        Frequency = 16384,
    }
}