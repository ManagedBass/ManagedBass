using System;

namespace ManagedBass.Enc
{
    /// <summary>
    /// Forwards to <see cref="BassEnc_Aac"/>, whose name this one misspelt. Kept so existing code still compiles.
    /// </summary>
    /// <remarks>
    /// Only the members this class shipped with are here. The Apple/Core Audio
    /// <see cref="BassEnc_Aac.Start(int,string,EncodeFlags,EncodeProcedureEx,IntPtr)"/> overload is not forwarded:
    /// move to <see cref="BassEnc_Aac"/> to reach it.
    /// </remarks>
    [Obsolete("BassEnc_Acc was a misspelling of BassEnc_Aac. Use BassEnc_Aac; this forwarding class will be removed in a future release.")]
    public static class BassEnc_Acc
    {
        /// <inheritdoc cref="BassEnc_Aac.Version"/>
        public static Version Version => BassEnc_Aac.Version;

        /// <inheritdoc cref="BassEnc_Aac.Start(int,string,EncodeFlags,EncodeProcedure,IntPtr)"/>
        public static int Start(int Handle, string Options, EncodeFlags Flags, EncodeProcedure Procedure, IntPtr User) =>
            BassEnc_Aac.Start(Handle, Options, Flags, Procedure, User);

        /// <inheritdoc cref="BassEnc_Aac.Start(int,string,EncodeFlags,string)"/>
        public static int Start(int Handle, string Options, EncodeFlags Flags, string FileName) =>
            BassEnc_Aac.Start(Handle, Options, Flags, FileName);
    }
}
