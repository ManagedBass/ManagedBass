namespace ManagedBass.Enc
{
    /// <summary>
    /// Writes audio to a file, encoding it using ACM
    /// </summary>
    public class ACMEncodedFileWriter : ACMEncoder, IAudioFileWriter
    {
        /// <summary>
        /// Creates a new instance of <see cref="ACMEncodedFileWriter"/>.
        /// </summary>
        public ACMEncodedFileWriter(string FileName, 
                                    WaveFormatTag Encoding,
                                    PCMFormat Format,
                                    EncodeFlags Flags = EncodeFlags.Default)
            : base(FileName,
                   Flags,
                   Encoding,
                   Format)
        {
            Resolution = Format.Resolution;

            Start();
        }
        
        /// <summary>
        /// The Resolution for the encoded data.
        /// </summary>
        public Resolution Resolution { get; }
    }
}