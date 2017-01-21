using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Tags
{
    /// <summary>
    /// BASS audio library has limited support for reading tags, associated with a stream.
    /// This library extends that functionality, allowing developer/user to extract specific song information from the stream handle used with BASS.
    /// The extracted tag values are formatted into text ouput according to given format string (including conditional processing).
    /// </summary>
    public static class BassTags
    {
#if __IOS__
        const string DllName = "__Internal";
#else
        const string DllName = "tags";
#endif
        
        static IntPtr hLib;

        /// <summary>
        /// Load this library into Memory.
        /// </summary>
        /// <param name="Folder">Directory to Load from... <see langword="null"/> (default) = Load from Current Directory.</param>
        /// <returns><see langword="true" />, if the library loaded successfully, else <see langword="false" />.</returns>
        /// <remarks>
        /// <para>
        /// An external library is loaded into memory when any of its methods are called for the first time.
        /// This results in the first method call being slower than all subsequent calls.
        /// </para>
        /// <para>
        /// Some BASS libraries and add-ons may introduce new options to the main BASS lib like new parameters.
        /// But, before using these new options the respective library must be already loaded.
        /// This method can be used to make sure, that this library has been loaded.
        /// </para>
        /// </remarks>
        public static bool Load(string Folder = null) => (hLib = DynamicLibrary.Load(DllName, Folder)) != IntPtr.Zero;

        /// <summary>
        /// Unloads this library from Memory.
        /// </summary>
        /// <returns><see langword="true" />, if the library unloaded successfully, else <see langword="false" />.</returns>
        public static bool Unload() => DynamicLibrary.Unload(hLib);

        [DllImport(DllName)]
        static extern int TAGS_GetVersion();

        /// <summary>
        /// Returns Tags library version.
        /// Current version (in HIBYTE) and build (in LOBYTE).
        /// </summary>
        public static int Version => TAGS_GetVersion();

        // always uses Utf-8
        [DllImport(DllName)]
        static extern bool TAGS_SetUTF8(bool Enable);

        [DllImport(DllName)]
        static extern IntPtr TAGS_GetLastErrorDesc();

        /// <summary>
        /// For debug; the text description of the last <see cref="Read(int,string)"/>/<see cref="Read(int,string,TagType,int)"/> call.
        /// It may say something like: "ID3v2 tag: header is invalid", on poorly-added tags.
        /// An empty string is returned if there was no error.
        /// </summary>
        public static string LastErrorDescription
        {
            get
            {
                TAGS_SetUTF8(true);

                return Extensions.PtrToStringUtf8(TAGS_GetLastErrorDesc());
            }
        }

        [DllImport(DllName, CharSet = CharSet.Ansi)]
        static extern IntPtr TAGS_Read(int Handle, string Format);

        /// <summary>
        /// Reads tag values from the stream and formats them according to given format string.
        /// </summary>
        /// <param name="Handle">BASS handle, obtained normally via CreateStream call, or any other handle, on which <see cref="Bass.ChannelGetTags"/> can be called.</param>
        /// <param name="Format">format string.</param>
        /// <returns>
        /// Empty string when unable to properly read the tag, or when there are no supported tags.
        /// A string containing extracted values from the song tags, on success.
        /// A parser error message text, when format string is ill-formed.
        /// </returns>
        /// <remarks>
        /// Ill-formed string causes some error message to appear in the output, but don't count on it too much...
        /// If a file contains APE, ID3v1 and ID3v2 tags, the order of precedence is: APE, ID3v2, ID3v1.
        /// </remarks>
        public static string Read(int Handle, string Format)
        {
            TAGS_SetUTF8(true);

            return Extensions.PtrToStringUtf8(TAGS_Read(Handle, Format));
        }

        [DllImport(DllName, CharSet = CharSet.Ansi)]
        static extern IntPtr TAGS_ReadEx(int Handle, string Format, TagType TagType, int CodePage);

        /// <summary>
        /// Reads tag values from the stream and formats them according to given format string.
        /// </summary>
        /// <param name="Handle">BASS handle, obtained normally via CreateStream call, or any other handle, on which <see cref="Bass.ChannelGetTags"/> can be called.</param>
        /// <param name="Format">format string.</param>
        /// <param name="TagType">limit processing to a particular tag type, -1 = all tag types.</param>
        /// <param name="CodePage">
        /// Codepage to use when reading "ISO-8859-1" tags.
        /// If an invalid/unavailable codepage is requested, then 1252 (Latin 1) will be used.
        /// 0 can be used to request Windows' default codepage.
        /// </param>
        /// <returns>
        /// Empty string when unable to properly read the tag, or when there are no supported tags.
        /// A string containing extracted values from the song tags, on success.
        /// A parser error message text, when format string is ill-formed.
        /// </returns>
        /// <remarks>See <see cref="Read(int,string)"/></remarks>
        public static string Read(int Handle, string Format, TagType TagType, int CodePage)
        {
            TAGS_SetUTF8(true);

            return Extensions.PtrToStringUtf8(TAGS_ReadEx(Handle, Format, TagType, CodePage));
        }
    }
}
