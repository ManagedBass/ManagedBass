using System;

namespace ManagedBass.Wma
{
    /// <summary>
    /// The "type" of the tag and values strings for use with <see cref="BassWma.EncodeSetTag(int,IntPtr,IntPtr,WMATagFormat)" />.
    /// </summary>
    public enum WMATagFormat
    {
        /// <summary>
        /// ANSI strings.
        /// </summary>
        Ansi = 0,

        /// <summary>
        /// Unicode (UTF-16) strings (recommended to be used with .Net).
        /// </summary>
        Unicode = 1,
        
        /// <summary>
        /// UTF-8 strings.
        /// </summary>
        Utf8 = 2,
        
        /// <summary>
        /// Write a binary tag.
        /// <para>The length of the binary data is given in the high word.</para>
        /// </summary>
        Binary = 256
    }
}