using System;

namespace ManagedBass.Dynamics
{
    [Flags]
    public enum WasapiDeviceType
    {
        /// <summary>
        /// A network device.
        /// </summary>
        NetworkDevice = 0,
        
        /// <summary>
        /// A speakers device.
        /// </summary>
        Speakers = 1,
        
        /// <summary>
        /// A line level device.
        /// </summary>
        LineLevel = 2,
        
        /// <summary>
        /// A headphone device.
        /// </summary>
        Headphones = 3,
        
        /// <summary>
        /// A microphone device.
        /// </summary>
        Microphone = 4,
        
        /// <summary>
        /// A headset device.
        /// </summary>
        Headset = 5,
        
        /// <summary>
        /// A handset device.
        /// </summary>
        Handset = 6,
        
        /// <summary>
        /// A digital device.
        /// </summary>
        Digital = 7,
        
        /// <summary>
        /// A S/PDIF device.
        /// </summary>
        SPDIF = 8,
        
        /// <summary>
        /// A HDMI device.
        /// </summary>
        HDMI = 9,
        
        /// <summary>
        /// An unknown device.
        /// </summary>
        Unknown = 10,
    }
}