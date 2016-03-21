using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// WMA encoding flags.
    /// </summary>
    [Flags]
    public enum WMAEncodeFlags
    {
        /// <summary>
		/// File is a Unicode (16-bit characters) filename.
		/// </summary>
		Unicode = -2147483648,

		/// <summary>
		/// Default encoding, no tags.
		/// </summary>
		Default = 0,

		/// <summary>
		/// GetRates: get available CBR quality settings.
		/// </summary>
		RatesCBR = 0,

		/// <summary>
		/// 8 bit sample data.
		/// </summary>
		Byte = 1,

		/// <summary>
		/// 32-bit floating-point sample data.
		/// </summary>
		Float = 256,

		/// <summary>
		/// Standard WMA encoding.
		/// </summary>
		Standard = 8192,

		/// <summary>
		/// WMA Professional encoding.
		/// </summary>
		Professional = 16384,

		/// <summary>
		///  Enable 24-bit encoding.
		/// </summary>
		Encode24Bit = 32768,

		/// <summary>
		/// Save uncompressed PCM data.
		/// <para>When the sample data is floating-point (BASS_SAMPLE_FLOAT flag is used), the BASS_WMA_ENCODE_24BIT flag is considered; 24-bit data is written with it and 16-bit without. The "bitrate" parameter is ignored, except that it should be non-0.</para>
		/// </summary>
		PCM = 65536,

		/// <summary>
		/// GetRates: get available VBR quality settings.
		/// </summary>
		RatesVBR = 65536,

		/// <summary>
		/// Enable the specification of tags mid-stream (after encoding has begun).
		/// </summary>
		Script = 131072,

		/// <summary>
		/// Queue data to feed encoder asynchronously
		/// </summary>
		Queue = 262144,

		/// <summary>
		/// Use a BASS channel as source
		/// </summary>
		Source = 524288
    }
}