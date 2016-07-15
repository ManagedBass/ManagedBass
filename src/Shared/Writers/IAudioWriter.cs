using System;

namespace ManagedBass
{
    /// <summary>
    /// Writes audio to a file.
    /// </summary>
    public interface IAudioWriter : IDisposable
    {
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Data">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        bool Write(byte[] Data, int Length);

        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Data">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        bool Write(short[] Data, int Length);

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Data">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        bool Write(float[] Data, int Length);
    }
}
