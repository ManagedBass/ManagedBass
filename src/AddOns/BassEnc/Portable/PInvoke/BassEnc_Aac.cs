using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ManagedBass.Enc
{

    /// <summary>
    /// BassEnc_Aac is an extension to the BassEnc add-on that allows BASS channels to be Aac encoded, with support for AACENC options.
    /// </summary>
    /// <remarks>
    /// un4seen do not publish a <c>bassenc_aac</c> native for any Apple platform. There, AAC encoding goes through
    /// BASSenc's own Core Audio entry points instead, and this class dispatches to them on
    /// <see cref="CrossPlatformHelper.IsApple"/> at run time so that one assembly serves every platform.
    /// The two paths do not have the same capabilities, so a few members are available on one side only; each says so.
    /// </remarks>
    public static class BassEnc_Aac
    {
#if MANAGEDBASS_INTERNAL_DLLIMPORT
    const string DllName = "__Internal";
#else
    const string DllName = "bassenc_aac";
#endif

        const string AppleUsesCoreAudio = "BassEnc_Aac is not available on Apple platforms - un4seen publish no bassenc_aac native for them. Use BassEnc.EncodeStartCA instead.";

        const string AppleOnlyProcedureEx = "On Apple platforms AAC encoding goes through Core Audio, which reports the encoded data to an EncodeProcedureEx. Use the EncodeProcedureEx overload of Start.";

        const string NonAppleProcedure = "The EncodeProcedureEx overload of Start is Apple-only, because it is Core Audio that reports the encoded data. Use the EncodeProcedure overload.";

        // NOTE: ADTS is the only AAC container Core Audio will write to a stream; the object type selects the profile within it.
        const string AdtsFileType = "adts";

        [DllImport(DllName)]
        static extern int BASS_Encode_AAC_GetVersion();

        /// <summary>
        /// Gets the Version of BassEnc_Aac that is loaded.
        /// </summary>
        /// <remarks>
        /// <b>Platform-specific</b>
        /// Not available on Apple platforms, where there is no bassenc_aac native to report a version for.
        /// </remarks>
        /// <exception cref="PlatformNotSupportedException">The current platform is an Apple platform.</exception>
        public static Version Version => CrossPlatformHelper.IsApple
            ? throw new PlatformNotSupportedException(AppleUsesCoreAudio)
            : Extensions.GetVersion(BASS_Encode_AAC_GetVersion());

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_AAC_Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedure Procedure, IntPtr User);

        /// <summary>
        /// Start Aac Encoding to <see cref="EncodeProcedure"/>.
        /// </summary>
        /// <param name="Handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Options">
        /// Encoder options... null = use defaults.
        /// The following AACENC style options are supported: --object-type --bitrate, --vbr.
        /// Anything else that is included will be ignored.
        /// </param>
        /// <param name="Flags">A combination of <see cref="EncodeFlags"/>.</param>
        /// <param name="Procedure">Optional callback function to receive the encoded data... null = no callback.</param>
        /// <param name="User">User instance data to pass to the callback function.</param>
        /// <returns>The encoder handle is returned if the encoder is successfully started, else 0 is returned. Use <see cref="Bass.LastError"/> to get the error code</returns>
        /// <remarks>
        /// <see cref="BassEnc.EncodeStart(int,string,EncodeFlags,EncoderProcedure,IntPtr)"/> is used internally to apply the encoder to the source channel, so the remarks in its documentation also apply to this function.
        ///
        /// <b>Platform-specific</b>
        /// On Windows and Linux, an SSE supporting CPU is required for sample rates other than 48000/24000/16000/12000/8000 Hz.
        /// Not available on Apple platforms; use the <see cref="EncodeProcedureEx"/> overload there.
        /// </remarks>
        /// <exception cref="PlatformNotSupportedException">The current platform is an Apple platform.</exception>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid</exception>
        /// <exception cref="Errors.SampleFormat">The channel's sample format is not supported by the encoder.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem! </exception>
        public static int Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedure Procedure, IntPtr User) =>
            CrossPlatformHelper.IsApple
                ? throw new PlatformNotSupportedException(AppleOnlyProcedureEx)
                : BASS_Encode_AAC_Start(Handle, Options, Flags | EncodeFlags.Unicode, Procedure, User);

        /// <summary>
        /// Start AAC Encoding to <see cref="EncodeProcedureEx"/> using a Core Audio codec (Apple platforms only).
        /// </summary>
        /// <param name="Handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Options">
        /// Encoder options... null = use defaults.
        /// Only the AACENC style --object-type and --bitrate options are honoured here; --vbr has no Core Audio equivalent and is ignored.
        /// </param>
        /// <param name="Flags">A combination of <see cref="EncodeFlags"/>.</param>
        /// <param name="Procedure">Optional callback function to receive the encoded data... null = no callback.</param>
        /// <param name="User">User instance data to pass to the callback function.</param>
        /// <returns>The encoder handle is returned if the encoder is successfully started, else 0 is returned. Use <see cref="Bass.LastError"/> to get the error code</returns>
        /// <remarks>
        /// <see cref="BassEnc.EncodeStartCA(int,int,int,EncodeFlags,int,EncodeProcedureEx,IntPtr)"/> is used internally, so the remarks in its documentation also apply to this function.
        /// The output is a raw ADTS stream.
        ///
        /// <b>Platform-specific</b>
        /// Apple platforms only, because it is Core Audio that supplies the codec.
        /// </remarks>
        /// <exception cref="PlatformNotSupportedException">The current platform is not an Apple platform.</exception>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid</exception>
        /// <exception cref="Errors.FileFormat">ADTS is not a writable file type on this system.</exception>
        /// <exception cref="Errors.Codec">The AAC object type requested by <paramref name="Options"/> is not available.</exception>
        /// <exception cref="Errors.NotAvailable">The bitrate requested by <paramref name="Options"/> is not supported by the codec.</exception>
        /// <exception cref="Errors.SampleFormat">The channel's sample format is not supported by the encoder.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem! </exception>
        public static int Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedureEx Procedure, IntPtr User) =>
            CrossPlatformHelper.IsApple
                ? BassEnc.EncodeStartCA(Handle, CharsToInt(AdtsFileType), CharsToInt(AudioType(Options)), Flags, Bitrate(Options), Procedure, User)
                : throw new PlatformNotSupportedException(NonAppleProcedure);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_AAC_StartFile(int Handle, string Options, EncodeFlags Flags, string FileName);

        /// <summary>
        /// Start AAC Encoding to File.
        /// </summary>
        /// <param name="Handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Options">
        /// Encoder options... null = use defaults.
        /// The following AACENC style options are supported: --object-type --bitrate, --vbr.
        /// Anything else that is included will be ignored.
        /// </param>
        /// <param name="Flags">A combination of <see cref="EncodeFlags"/>.</param>
        /// <param name="FileName">Output filename... null = no output file.</param>
        /// <returns>The encoder handle is returned if the encoder is successfully started, else 0 is returned. Use <see cref="Bass.LastError"/> to get the error code</returns>
        /// <remarks>
        /// <see cref="BassEnc.EncodeStart(int,string,EncodeFlags,EncoderProcedure,IntPtr)"/> is used internally to apply the encoder to the source channel, so the remarks in its documentation also apply to this function.
        ///
        /// <b>Platform-specific</b>
        /// On Windows and Linux, an SSE supporting CPU is required for sample rates other than 48000/24000/16000/12000/8000 Hz.
        /// On Apple platforms a Core Audio codec is used instead and the output is a raw ADTS stream; only --object-type and --bitrate are honoured there.
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid</exception>
        /// <exception cref="Errors.SampleFormat">The channel's sample format is not supported by the encoder.</exception>
        /// <exception cref="Errors.Create">The file could not be created.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem! </exception>
        public static int Start(int Handle, string Options, EncodeFlags Flags, string FileName) =>
            CrossPlatformHelper.IsApple
                ? BassEnc.EncodeStartCA(Handle, CharsToInt(AdtsFileType), CharsToInt(AudioType(Options)), Flags, Bitrate(Options), FileName)
                : BASS_Encode_AAC_StartFile(Handle, Options, Flags | EncodeFlags.Unicode, FileName);

        /// <summary>
        /// Maps the AACENC --object-type in the command to the matching Core Audio kAudioFormatMPEG4AAC identifier.
        /// </summary>
        /// <param name="Options">The command-line to parse.</param>
        /// <returns>A four character Core Audio audio format identifier; plain LC AAC when no usable object type is given.</returns>
        private static string AudioType(string Options)
        {
            switch (GetArgValueInt(Options, "--object-type"))
            {
                case 5: return "aach";
                case 23: return "aacl";
                case 29: return "aacp";
                case 39: return "aace";
                default: return "aac ";
            }
        }

        /// <summary>
        /// Extracts the AACENC --bitrate from the command.
        /// </summary>
        /// <param name="Options">The command-line to parse.</param>
        /// <returns>The bitrate in bits per second, or 0 for the codec's own default.</returns>
        private static int Bitrate(string Options) => GetArgValueInt(Options, "--bitrate") ?? 0;

        /// <summary>
        /// Extracts the next int value that follows specified argument string in the command.
        /// </summary>
        /// <param name="Cmd">The command-line to parse.</param>
        /// <param name="Arg">The argument string to get the value for.</param>
        /// <returns>The int value that follows the argument string, or null if not found or invalid.</returns>
        private static int? GetArgValueInt(string Cmd, string Arg)
        {
            if (string.IsNullOrEmpty(Cmd) || string.IsNullOrEmpty(Arg))
                return null;
            Arg += " ";
            var posStart = Cmd.IndexOf(Arg, StringComparison.Ordinal);
            if (posStart < 0)
                return null;
            posStart += Arg.Length;
            var posEnd = Cmd.IndexOf(' ', posStart);
            if (posEnd < 0)
                posEnd = Cmd.Length;
            var value = Cmd.Substring(posStart, posEnd - posStart).Trim();
            return int.TryParse(value, out var valueInt) ? valueInt : null;
        }

        /// <summary>
        /// Converts 4 chars into an Int32 value.
        /// </summary>
        /// <param name="Chars">The 4 chars to convert.</param>
        /// <returns>An Int32 value.</returns>
        private static int CharsToInt(string Chars)
        {
            var bytes = Encoding.ASCII.GetBytes(Chars);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
