using System.Runtime.InteropServices;

namespace ManagedBass.Asio
{
	/// <summary>
	/// Used with <see cref="BassAsio.Future" /> and the DSD IoFormat selector.
	/// </summary>
    [StructLayout(LayoutKind.Sequential)]
	public class AsioIOFormat
	{
		/// <summary>
		/// Format Type
        /// </summary>
		public AsioIOFormatType FormatType = AsioIOFormatType.Invalid;

		/// <summary>
		/// up to 508 chars
		/// </summary>
        public string Future = string.Empty;
	}

    /// <summary>
    /// Asio IO Format type to be used with <see cref="AsioIOFormat.FormatType"/>.
    /// </summary>
    public enum AsioIOFormatType
    {
        /// <summary>
        /// Invalid.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// PCM.
        /// </summary>
        PCM = 0,

        /// <summary>
        /// DSD.
        /// </summary>
        DSD = 1
    }
}