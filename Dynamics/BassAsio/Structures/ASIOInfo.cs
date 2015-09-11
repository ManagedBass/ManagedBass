using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// Used with <see cref="BassAsio.Info" /> to retrieve information on the current device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AsioInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        char[] name;

        int version;
        int inputs;
        int outputs;
        int bufmin;
        int bufmax;
        int bufpref;
        int bufgran;
        public AsioInitFlags initflags;

        /// <summary>
        /// The name of the device/driver.
        /// </summary>
        public string Name { get { return new string(name).Replace("\0", "").Trim(); } }

        /// <summary>
        /// The driver version.
        /// </summary>
        public int DriverVersion { get { return version; } }

        /// <summary>
        /// The number of input channels available.
        /// </summary>
        public int Inputs { get { return inputs; } }

        /// <summary>
        /// The number of output channels available.
        /// </summary>
        public int Outputs { get { return outputs; } }

        /// <summary>
        /// The minimum Buffer Length, in samples.
        /// </summary>
        public int MinBufferLength { get { return bufmin; } }

        /// <summary>
        /// The maximum Buffer Length, in samples.
        /// </summary>
        public int MaxBufferLength { get { return bufmin; } }

        /// <summary>
        /// The preferred/default Buffer Length, in samples.
        /// </summary>
        public int PreferredBufferLength { get { return bufpref; } }

        /// <summary>
        /// The Buffer Length granularity, that is the smallest possible Length change... -1 = the possible Buffer lengths increase in powers of 2.
        /// </summary>
        public int BufferLengthGranularity { get { return bufgran; } }

        public override string ToString() { return Name; }
    }
}