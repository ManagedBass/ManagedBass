using System;

namespace ManagedBass.Wma
{
    /// <summary>
    /// Writes a WMA File. Requires BassWma.dll
    /// </summary>
    public class WmaFileWriter : IAudioFileWriter
    {
        readonly int _encoderHandle;
        
        /// <summary>
        /// The Resolution for the encoded data.
        /// </summary>
        public Resolution Resolution { get; }

        /// <summary>
        /// Creates a new instance of <see cref="WmaFileWriter"/>.
        /// </summary>
        public WmaFileWriter(string FilePath, PCMFormat Format, int BitRate = 128000)
        {
            Resolution = Format.Resolution;

            var flags = WMAEncodeFlags.Default;

            switch (Resolution)
            {
                case Resolution.Byte:
                    flags |= WMAEncodeFlags.Byte;
                    break;

                case Resolution.Float:
                    flags |= WMAEncodeFlags.Float;
                    break;
            }

            _encoderHandle = BassWma.EncodeOpenFile(Format.Frequency, Format.Channels, flags, BitRate, FilePath);
        }
        
        /// <summary>
        /// Write data from an IntPtr.
        /// </summary>
        /// <param name="Buffer">IntPtr to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(IntPtr Buffer, int Length) => BassWma.EncodeWrite(_encoderHandle, Buffer, Length);
        
        /// <summary>
        /// Write data from a byte[].
        /// </summary>
        /// <param name="Buffer">byte[] to write from.</param>
        /// <param name="Length">No of bytes to write.</param>
        public bool Write(byte[] Buffer, int Length) => BassWma.EncodeWrite(_encoderHandle, Buffer, Length);
        
        /// <summary>
        /// Write data from a short[].
        /// </summary>
        /// <param name="Buffer">short[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of Shorts) * 2.</param>
        public bool Write(short[] Buffer, int Length) => BassWma.EncodeWrite(_encoderHandle, Buffer, Length);

        /// <summary>
        /// Write data from a float[].
        /// </summary>
        /// <param name="Buffer">float[] to write from.</param>
        /// <param name="Length">No of bytes to write, i.e. (No of floats) * 4.</param>
        public bool Write(float[] Buffer, int Length) => BassWma.EncodeWrite(_encoderHandle, Buffer, Length);

        /// <summary>
        /// Frees all resources used by the writer.
        /// </summary>
        public void Dispose() => BassWma.EncodeClose(_encoderHandle);
    }
}