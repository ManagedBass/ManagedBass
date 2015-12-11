using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum DeviceInfoFlags
    {
        // Summary:
        //     Bitmask to identify the device type.
        TypeMask = -16777216,
        //
        // Summary:
        //     The device is not enabled and not initialized.
        None = 0,
        //
        // Summary:
        //     The device is enabled. It will not be possible to initialize the device if
        //     this flag is not present.
        Enabled = 1,
        //
        // Summary:
        //     The device is the system default.
        Default = 2,
        //
        // Summary:
        //     The device is initialized, ie. Un4seen.Bass.Bass.BASS_Init(System.Int32,System.Int32,Un4seen.Bass.BASSInit,System.IntPtr)
        //     or Un4seen.Bass.Bass.BASS_RecordInit(System.Int32) has been called.
        Initialized = 4,
        //
        // Summary:
        //     An audio endpoint Device that the user accesses remotely through a network.
        Network = 16777216,
        //
        // Summary:
        //     A set of speakers.
        Speakers = 33554432,
        //
        // Summary:
        //     An audio endpoint Device that sends a line-level analog signal to a line-input
        //     jack on an audio adapter or that receives a line-level analog signal from
        //     a line-output jack on the adapter.
        Line = 50331648,
        //
        // Summary:
        //     A set of headphones.
        Headphones = 67108864,
        //
        // Summary:
        //     A microphone.
        Microphone = 83886080,
        //
        // Summary:
        //     An earphone or a pair of earphones with an attached mouthpiece for two-way
        //     communication.
        Headset = 100663296,
        //
        // Summary:
        //     The part of a telephone that is held in the hand and that contains a speaker
        //     and a microphone for two-way communication.
        Handset = 117440512,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a connector
        //     for a digital interface of unknown type.
        Digital = 134217728,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a Sony/Philips
        //     Digital Interface (S/PDIF) connector.
        SPDIF = 150994944,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a High-Definition
        //     Multimedia Interface (HDMI) connector.
        HDMI = 167772160,
        //
        // Summary:
        //     An audio endpoint Device that connects to an audio adapter through a DisplayPort
        //     connector.
        DisplayPort = 1073741824,
    }
}