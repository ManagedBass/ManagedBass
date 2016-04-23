using System;

namespace ManagedBass
{
    /// <summary>
    /// Writes audio to a file.
    /// </summary>
    public interface IAudioFileWriter : IDisposable
    {
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        bool Write(byte[] Buffer, int Length);

        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        bool Write(short[] Buffer, int Length);

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        bool Write(float[] Buffer, int Length);

        /// <summary>
        /// The Resolution for the encoded data.
        /// </summary>
        Resolution Resolution { get; }
    }
}
