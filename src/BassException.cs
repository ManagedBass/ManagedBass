using ManagedBass.Dynamics;
using System;

namespace ManagedBass
{
    public class BassException : Exception
    {
        internal BassException() : this(Bass.LastError) { }

        internal BassException(Errors ErrorCode)
            : base(string.Format("Error: {0}", ErrorCode))
        {
            this.ErrorCode = ErrorCode;
        }

        public Errors ErrorCode { get; }
    }
}
