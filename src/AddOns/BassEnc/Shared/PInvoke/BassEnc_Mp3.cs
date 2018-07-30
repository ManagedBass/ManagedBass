using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    /// <summary>
    /// BassEnc_MP3 is an extension to the BassMP3 add-on that allows BASS channels to be MP3 encoded.
    /// </summary>
    public static class BassEnc_Mp3
    {
#if __IOS__
        const string DllName = "__Internal";
#else
        const string DllName = "bassenc_mp3";
#endif
                
        [DllImport(DllName)]
        static extern int BASS_Encode_MP3_GetVersion();

        /// <summary>
        /// Gets the Version of BassEnc_MP3 that is loaded.
        /// </summary>
        public static Version Version => Extensions.GetVersion(BASS_Encode_MP3_GetVersion());

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_MP3_Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedure Procedure, IntPtr User);

        /// <summary>
        /// Start Mp3 Encoding to <see cref="EncodeProcedure"/>.
        /// For best documentation on functionality see http://www.un4seen.com/doc/
        /// </summary>
        /// <param name="Handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Options">
        /// Encoder options... NULL = use defaults. 
        /// The following LAME style options are supported: -b, -B, -v, -V, -q, -m, --abr, -Y, --resample, -p, -t, --tt, --ta, --tl, --ty, --tc, --tn, --tg, --tv, --id3v1-only, --id3v2-only, --add-id3v2, --pad-id3v2, --pad-id3v2-size, --noreplaygain. 
        /// Anything else that is included will be ignored. 
        /// See the LAME documentation for details on the aforementioned options and defaults.
        /// https://svn.code.sf.net/p/lame/svn/trunk/lame/USAGE
        /// </param>
        /// <param name="Flags">A combination of <see cref="EncodeFlags"/>.</param>
        /// <param name="Procedure">Optional callback function to receive the encoded data... null = no callback.</param>
        /// <param name="User">User instance data to pass to the callback function.</param>
        /// <returns>The encoder handle is returned if the encoder is successfully started, else 0 is returned. Use <see cref="Bass.LastError"/> to get the error code</returns>
        /// <remarks>
        /// <see cref="BassEnc.EncodeStart(int,string,EncodeFlags,EncoderProcedure,IntPtr)"/> is used internally to apply the encoder to the source channel, so the remarks in its documentation also apply to this function. 
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid</exception>
        /// <exception cref="Errors.SampleFormat">The channel's sample format is not supported by the encoder.</exception>
        /// <exception cref="Errors.NotAvailable">This function is not available on platforms/architectures without an FPU.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem! </exception>
        public static int Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedure Procedure, IntPtr User)
        {
            return BASS_Encode_MP3_Start(Handle, Options, Flags | EncodeFlags.Unicode, Procedure, User);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_MP3_StartFile(int Handle, string Options, EncodeFlags Flags, string FileName);

        /// <summary>
        /// Start Mp3 Encoding to File. For best documentation on functionality see http://www.un4seen.com/doc/
        /// </summary>
        /// <param name="Handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="Options">
        /// Encoder options... NULL = use defaults. 
        /// The following LAME style options are supported: -b, -B, -v, -V, -q, -m, --abr, -Y, --resample, -p, -t, --tt, --ta, --tl, --ty, --tc, --tn, --tg, --tv, --id3v1-only, --id3v2-only, --add-id3v2, --pad-id3v2, --pad-id3v2-size, --noreplaygain. 
        /// Anything else that is included will be ignored. 
        /// See the LAME documentation for details on the aforementioned options and defaults.
        /// https://svn.code.sf.net/p/lame/svn/trunk/lame/USAGE
        /// </param>
        /// <param name="Flags">A combination of <see cref="EncodeFlags"/>.</param>
        /// <param name="FileName">Output filename... null = no output file.</param>
        /// <returns>The encoder handle is returned if the encoder is successfully started, else 0 is returned. Use <see cref="Bass.LastError"/> to get the error code</returns>
        /// <remarks>
        /// <see cref="BassEnc.EncodeStart(int,string,EncodeFlags,EncoderProcedure,IntPtr)"/> is used internally to apply the encoder to the source channel, so the remarks in its documentation also apply to this function. 
        /// 
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle"/> is not valid</exception>
        /// <exception cref="Errors.SampleFormat">The channel's sample format is not supported by the encoder.</exception>
        /// <exception cref="Errors.Create">The file could not be created.</exception>
        /// <exception cref="Errors.NotAvailable">This function is not available on platforms/architectures without an FPU.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem! </exception>
        public static int Start(int Handle, string Options, EncodeFlags Flags, string FileName)
        {
            return BASS_Encode_MP3_StartFile(Handle, Options, Flags | EncodeFlags.Unicode, FileName);
        }
    }
}