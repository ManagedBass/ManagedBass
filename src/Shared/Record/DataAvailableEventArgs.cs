using System;

namespace ManagedBass
{
    public class DataAvailableEventArgs : EventArgs
    {
        public byte[] Buffer { get; set; }
        public int Length { get; set; }

        public DataAvailableEventArgs(byte[] Buffer, int Length)
        {
            this.Buffer = Buffer;
            this.Length = Length;
        }
    }
}