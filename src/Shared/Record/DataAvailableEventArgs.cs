using System;

namespace ManagedBass
{
    /// <summary>
    /// Provides access to recorded data.
    /// </summary>
    public class DataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// The Data buffer.
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// No of bytes in the <see cref="Buffer"/>.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DataAvailableEventArgs"/>.
        /// </summary>
        /// <param name="Buffer">The Data Buffer.</param>
        /// <param name="Length">No of bytes in the <paramref name="Buffer"/></param>
        public DataAvailableEventArgs(byte[] Buffer, int Length)
        {
            this.Buffer = Buffer;
            this.Length = Length;
        }
    }
}