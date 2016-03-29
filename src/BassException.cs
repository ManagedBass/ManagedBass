using System;
using ManagedBass.Asio;

namespace ManagedBass
{
    /// <summary>
    /// An Exception wrapping <see cref="Bass.LastError"/> or <see cref="BassAsio.LastError"/>.
    /// </summary>
    public class BassException : Exception
    {
        internal BassException() : this(Bass.LastError) { }

        internal BassException(Errors ErrorCode)
            : base($"Error: {ErrorCode}")
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Gets the Bass Error Code.
        /// </summary>
        public Errors ErrorCode { get; }
    }
}
