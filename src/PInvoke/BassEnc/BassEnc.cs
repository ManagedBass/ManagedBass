using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Enc
{
    /// <summary>
    /// Wraps BassEnc: bassenc.dll
    /// </summary>
    public static partial class BassEnc
    {
#if __IOS__
        const string DllName = "__internal";
#else
        const string DllName = "bassenc";
#endif
        static IntPtr _castProxy;

#if __ANDROID__ || WINDOWS || LINUX || __MAC__
        static IntPtr hLib;
        
        /// <summary>
        /// Load from a folder other than the Current Directory.
        /// <param name="Folder">If null (default), Load from Current Directory</param>
        /// </summary>
        public static void Load(string Folder = null) => hLib = DynamicLibrary.Load(DllName, Folder);

        public static void Unload() => DynamicLibrary.Unload(hLib);
#endif

        #region Version
        [DllImport(DllName)]
        static extern int BASS_Encode_GetVersion();

        /// <summary>
        /// Gets the Version of BassEnc that is loaded.
        /// </summary>
        public static Version Version => Extensions.GetVersion(BASS_Encode_GetVersion());
        #endregion

        #region Configure
        /// <summary>
        /// Encoder DSP priority (default -1000) which determines where in the DSP chain the encoding is performed. 
        /// </summary>
        /// <remarks>
        /// All DSP with a higher priority will be present in the encoding.
        /// Changes only affect subsequent encodings, not those that have already been started.
        /// </remarks>
        public static int DSPPriority
        {
            get { return Bass.GetConfig(Configuration.EncodePriority); }
            set { Bass.Configure(Configuration.EncodePriority, value); }
        }

        /// <summary>
        /// The maximum queue Length of the async encoder (default 10000, 0 = Unlimited) in milliseconds.
        /// </summary>
        /// <remarks>
        /// When queued encoding is enabled, the queue's Buffer will grow as needed to hold the queued data, up to a limit specified by this config option.
        /// Changes only apply to new encoders, not any already existing encoders.
        /// </remarks>
        public static int Queue
        {
            get { return Bass.GetConfig(Configuration.EncodeQueue); }
            set { Bass.Configure(Configuration.EncodeQueue, value); }
        }

        /// <summary>
        /// The time to wait (in milliseconds) to send data to a cast server (default 5000ms)
        /// </summary>
        /// <remarks>
        /// When an attempt to send data is timed-out, the data is discarded. 
        /// <see cref="EncodeSetNotify"/> can be used to receive a notification of when this happens.
        /// Changes take immediate effect.
        /// </remarks>
        public static int CastTimeout
        {
            get { return Bass.GetConfig(Configuration.EncodeCastTimeout); }
            set { Bass.Configure(Configuration.EncodeCastTimeout, value); }
        }

        /// <summary>
        /// Proxy server settings when connecting to Icecast and Shoutcast (in the form of "[User:pass@]server:port"... <see langword="null"/> (default) = don't use a proxy but a direct connection).
        /// </summary>
        /// <remarks>
        /// If only the "server:port" part is specified, then that proxy server is used without any authorization credentials.
        /// This setting affects how the following functions connect to servers: <see cref="CastInit"/>, <see cref="CastGetStats"/>, <see cref="CastSetTitle(int, string, string)"/>.
        /// When a proxy server is used, it needs to support the HTTP 'CONNECT' method.
        /// The default setting is <see langword="null"/> (do not use a proxy).
        /// Changes take effect from the next internet stream creation call. 
        /// By default, BassEnc will not use any proxy settings when connecting to Icecast and Shoutcast.
        /// </remarks>
        public static string CastProxy
        {
            // BassEnc does not make a copy of the config string, so it must reside in the heap (not the stack), eg. a global variable. 
            // This also means that the proxy settings can subsequently be changed at that location without having to call this function again.

            get { return Marshal.PtrToStringAnsi(Bass.GetConfigPtr(Configuration.EncodeCastProxy)); }
            set
            {
                if (_castProxy != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_castProxy);

                    _castProxy = IntPtr.Zero;
                }

                _castProxy = Marshal.StringToHGlobalAnsi(value);

                Bass.Configure(Configuration.EncodeCastProxy, _castProxy);
            }
        }
        #endregion

        #region Encoding
        /// <summary>
        /// Sends a RIFF chunk to an encoder.
        /// </summary>
        /// <param name="Handle">The encoder handle... a HENCODE.</param>
        /// <param name="ID">The 4 character chunk id (e.g. 'bext').</param>
        /// <param name="Buffer">The buffer containing the chunk data (without the id).</param>
        /// <param name="Length">The number of bytes in the buffer.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <remarks>
        /// BassEnc writes the minimum chunks required of a WAV file: "fmt" and "data", and "ds64" and "fact" when appropriate.
        /// This function can be used to add other chunks. 
        /// For example, a BWF "bext" chunk or "INFO" tags.
        /// <para>
        /// Chunks can only be added prior to sample data being sent to the encoder.
        /// The <see cref="EncodeFlags.Pause"/> flag can be used when starting the encoder to ensure that no sample data is sent before additional chunks have been set.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">No RIFF headers/chunks are being sent to the encoder (due to the <see cref="EncodeFlags.NoHeader"/> flag being in effect), or sample data encoding has started.</exception>
        /// <exception cref="Errors.Ended">The encoder has died.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int Handle, string ID, IntPtr Buffer, int Length);

        /// <summary>
        /// Sends a RIFF chunk to an encoder.
        /// </summary>
        /// <param name="Handle">The encoder handle... a HENCODE.</param>
        /// <param name="ID">The 4 character chunk id (e.g. 'bext').</param>
        /// <param name="Buffer">The buffer containing the chunk data (without the id).</param>
        /// <param name="Length">The number of bytes in the buffer.</param>
        /// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <remarks>
        /// BassEnc writes the minimum chunks required of a WAV file: "fmt" and "data", and "ds64" and "fact" when appropriate.
        /// This function can be used to add other chunks. 
        /// For example, a BWF "bext" chunk or "INFO" tags.
        /// <para>
        /// Chunks can only be added prior to sample data being sent to the encoder.
        /// The <see cref="EncodeFlags.Pause"/> flag can be used when starting the encoder to ensure that no sample data is sent before additional chunks have been set.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">No RIFF headers/chunks are being sent to the encoder (due to the <see cref="EncodeFlags.NoHeader"/> flag being in effect), or sample data encoding has started.</exception>
        /// <exception cref="Errors.Ended">The encoder has died.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_AddChunk")]
        public static extern bool EncodeAddChunk(int Handle, string ID, byte[] Buffer, int Length);
        
        /// <summary>
        /// Retrieves the channel that an encoder is set on.
        /// </summary>
        /// <param name="Handle">The encoder to get the channel from.</param>
        /// <returns>If successful, the encoder's channel handle is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_GetChannel")]
        public static extern int EncodeGetChannel(int Handle);

        /// <summary>
        /// Retrieves the amount of data queued, sent to or received from an encoder, or sent to a cast server.
        /// </summary>
        /// <param name="Handle">The encoder handle.</param>
        /// <param name="Count">The count to retrieve (see <see cref="EncodeCount"/>).</param>
        /// <returns>If successful, the requested count (in bytes) is returned, else -1 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <remarks>
        /// <para>
        /// The queue counts are based on the channel's sample format (floating-point if the <see cref="Bass.FloatingPointDSP"/> option is enabled),
        /// while the <see cref="EncodeCount.In"/> count is based on the sample format used by the encoder,
        /// which could be different if one of the Floating-point conversion flags is active or the encoder is using an ACM codec (which take 16-bit data).
        /// </para>
        /// <para>
        /// When the encoder output is being sent to a cast server, the <see cref="EncodeCount.Cast"/> count will match the <see cref="EncodeCount.Out"/> count,
        /// unless there have been problems (eg. network timeout) that have caused data to be dropped.
        /// </para>
        /// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The encoder does not have a queue.</exception>
        /// <exception cref="Errors.Parameter"><paramref name="Count" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_GetCount")]
        public static extern long EncodeGetCount(int Handle, EncodeCount Count);

		/// <summary>
		/// Checks if an encoder is running on a channel.
		/// </summary>
		/// <param name="Handle">The encoder or channel handle... a HENCODE, HSTREAM, HMUSIC, or HRECORD.</param>
		/// <returns>The return value is one of <see cref="PlaybackState"/> values.</returns>
		/// <remarks>
		/// <para>When checking if there's an encoder running on a channel, and there are multiple encoders on the channel, <see cref="PlaybackState.Playing"/> will be returned if any of them are active.</para>
		/// <para>If an encoder stops running prematurely, <see cref="EncodeStop(int)" /> should still be called to release resources that were allocated for the encoding.</para>
		/// </remarks>
        [DllImport(DllName, EntryPoint = "BASS_Encode_IsActive")]
        public static extern PlaybackState EncodeIsActive(int Handle);
        
		/// <summary>
		/// Moves an encoder (or all encoders on a channel) to another channel.
		/// </summary>
		/// <param name="Handle">The encoder or channel handle... a HENCODE, HSTREAM, HMUSIC, or HRECORD.</param>
		/// <param name="Channel">The channel to move the encoder(s) to... a HSTREAM, HMUSIC, or HRECORD.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
        /// The new channel must have the same sample format (rate, channels, resolution) as the old channel, as that is what the encoder is expecting. 
		/// A channel's sample format is available via <see cref="Bass.ChannelGetInfo(int, out ChannelInfo)" />.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> or <paramref name="Channel" /> is not valid.</exception>
        /// <exception cref="Errors.SampleFormat">The new channel's sample format is not the same as the old channel's.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_SetChannel")]
        public static extern bool EncodeSetChannel(int Handle, int Channel);

        [DllImport(DllName, EntryPoint = "BASS_Encode_SetNotify")]
        public static extern bool EncodeSetNotify(int Handle, EncodeNotifyProcedure Procedure, IntPtr User = default(IntPtr));
        
		/// <summary>
		/// Pauses or resumes encoding on a channel.
		/// </summary>
		/// <param name="Handle">The encoder or channel handle... a HENCODE, HSTREAM, HMUSIC, or HRECORD.</param>
		/// <param name="Paused">Paused?</param>
		/// <returns>If no encoder has been started on the channel, <see langword="false" /> is returned, otherwise <see langword="true" /> is returned.</returns>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// When an encoder is paused, no sample data will be sent to the encoder "automatically".
        /// Data can still be sent to the encoder "manually" though, via the <see cref="EncodeWrite(int, IntPtr, int)" /> function.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.s</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_SetPaused")]
        public static extern bool EncodeSetPaused(int Handle, bool Paused = true);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_Start(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user);

        public static int EncodeStart(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user = default(IntPtr))
        {
            return BASS_Encode_Start(handle, cmdline, flags | EncodeFlags.Unicode, proc, user);
        }

#if __MAC__ || __IOS__
        [DllImport(DllName, EntryPoint = "BASS_Encode_StartCA")]
        public static extern int EncodeStartCA(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, EncodeProcedureEx proc, IntPtr user);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartCAFile(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, string filename);

        public static int EncodeStartCA(int handle, int ftype, int atype, EncodeFlags flags, int bitrate, string filename)
        {
            return BASS_Encode_StartCAFile(handle, ftype, atype, flags | EncodeFlags.Unicode, bitrate, filename);
        }
#endif

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartLimit(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user, int limit);

        public static int EncodeStart(int handle, string cmdline, EncodeFlags flags, EncodeProcedure proc, IntPtr user, int limit)
        {
            return BASS_Encode_StartLimit(handle, cmdline, flags | EncodeFlags.Unicode, proc, user, limit);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_Encode_StartUser(int handle, string filename, EncodeFlags flags, EncoderProcedure proc, IntPtr user);

        public static int EncodeStart(int handle, string filename, EncodeFlags flags, EncoderProcedure proc, IntPtr user = default(IntPtr))
        {
            return BASS_Encode_StartUser(handle, filename, flags | EncodeFlags.Unicode, proc, user);
        }
        
		/// <summary>
		/// Stops encoding on a channel.
		/// </summary>
		/// <param name="Handle">The encoder or channel handle... a HENCODE, HSTREAM, HMUSIC, or HRECORD.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// This function will free an encoder immediately, without waiting for any data that may be remaining in the queue.
        /// <see cref="EncodeStop(int, bool)" /> can be used to have an encoder process the queue before it is freed.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_Stop")]
        public static extern bool EncodeStop(int Handle);
        
		/// <summary>
		/// Stops async encoding on a channel.
		/// </summary>
		/// <param name="Handle">The encoder or channel handle... a HENCODE, HSTREAM, HMUSIC, or HRECORD.</param>
		/// <param name="Queue">Process the queue first? If so, the encoder will not be freed until after any data remaining in the queue has been processed, and it will not accept any new data in the meantime.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// When an encoder is told to wait for its queue to be processed, this function will return immediately and the encoder will be freed in the background after the queued data has been processed.
		/// <see cref="EncodeSetNotify" /> can be used to request notification of when the encoder has been freed.
        /// <see cref="EncodeStop(int)" /> (or this function with queue = <see langword="false" />) can be used to cancel to queue processing and free the encoder immediately.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_StopEx")]
        public static extern bool EncodeStop(int Handle, bool Queue);

        #region Encode Write
        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, IntPtr buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, byte[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, short[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, int[] buffer, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_Write")]
        public static extern bool EncodeWrite(int handle, float[] buffer, int length);
        #endregion
        #endregion

        #region Casting
        [DllImport(DllName, EntryPoint = "BASS_Encode_CastGetStats")]
        public static extern string CastGetStats(int handle, EncodeStats type, string pass);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastInit")]
        public static extern bool CastInit(int handle,
            string server,
            string pass,
            string content,
            string name,
            string url,
            string genre,
            string desc,
            string headers,
            int bitrate,
            bool pub);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastSendMeta")]
        public static extern bool CastSendMeta(int handle, EncodeMetaDataType type, IntPtr data, int length);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastSetTitle")]
        public static extern bool CastSetTitle(int handle, string title, string url);

        [DllImport(DllName, EntryPoint = "BASS_Encode_CastSetTitle")]
        public static extern bool CastSetTitle(int handle, byte[] title, byte[] url);
        #endregion

        #region Server
        [DllImport(DllName, EntryPoint = "BASS_Encode_ServerInit")]
        public static extern int ServerInit(int handle, string port, int buffer, int burst, EncodeServer flags, EncodeClientProcedure proc, IntPtr user);
        
		/// <summary>
		/// Kicks clients from a server.
		/// </summary>
		/// <param name="Handle">The encoder handle.</param>
		/// <param name="Client">The client(s) to kick... "" (empty string) = all clients. Unless a port number is included, this string is compared with the start of the connected clients' IP address.</param>
		/// <returns>If successful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>
        /// The clients may not be kicked immediately, but shortly after the call.
        /// If the server has been setup with an <see cref="EncodeClientProcedure" /> callback function, that will receive notification of the disconnections.
        /// </para>
		/// <para><b>Platform-specific</b></para>
		/// <para>This function is not available on Windows CE.</para>
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">No matching clients were found.</exception>
        [DllImport(DllName, EntryPoint = "BASS_Encode_ServerKick")]
        public static extern int ServerKick(int Handle, string Client = "");
        #endregion
    }
}