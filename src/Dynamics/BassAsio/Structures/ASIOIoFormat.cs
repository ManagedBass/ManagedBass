using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
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

    public enum AsioIOFormatType
    {
        Invalid = -1,
        PCM = 0,
        DSD = 1
    }
}