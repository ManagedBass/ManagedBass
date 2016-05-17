using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass.Wma
{
    /// <summary>
    /// Wraps basswma.dll
    /// </summary>
    /// <remarks>
    /// Supports: .wma, .wmv
    /// <para>Not available for Linux and OSX</para>
    /// </remarks>
    public static partial class BassWma
    {
        [DllImport(DllName, EntryPoint = "BASS_WMA_GetWMObject")]
        public static extern IntPtr GetWMObject(int handle);

        #region CreateStream Auth
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_StreamCreateFileAuth(bool mem, IntPtr file, long offset, long length, BassFlags flags, string user, string pass);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_StreamCreateFileAuth(bool mem, string file, long offset, long length, BassFlags flags, string user, string pass);

        public static int CreateStream(string File, BassFlags Flags, string UserName, string Password)
        {
            return BASS_WMA_StreamCreateFileAuth(false, File, 0, 0, Flags | BassFlags.Unicode, UserName, Password);
        }

        public static int CreateStream(IntPtr Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            return BASS_WMA_StreamCreateFileAuth(true, Memory, 0, Length, Flags | BassFlags.Unicode, UserName, Password);
        }

        public static int CreateStream(byte[] Memory, long Length, BassFlags Flags, string UserName, string Password)
        {
            var GCPin = GCHandle.Alloc(Memory, GCHandleType.Pinned);

            var Handle = CreateStream(GCPin.AddrOfPinnedObject(), Length, Flags, UserName, Password);

            if (Handle == 0) GCPin.Free();
            else Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => GCPin.Free());

            return Handle;
        }
        #endregion

        #region Encode
        #region Encode Write
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, IntPtr Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, byte[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, short[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, int[] Buffer, int Length);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeWrite")]
        public static extern bool EncodeWrite(int Handle, float[] Buffer, int Length);
        #endregion

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpen")]
        public static extern int EncodeOpen(int Frequency, int Channels, WMAEncodeFlags Flags, int Bitrate, WMEncodeProcedure Procedure, IntPtr User = default(IntPtr));

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_EncodeOpenFile(int freq, int chans, WMAEncodeFlags flags, int bitrate, string file);

        public static int EncodeOpenFile(int freq, int chans, WMAEncodeFlags flags, int bitrate, string file)
        {
            return BASS_WMA_EncodeOpenFile(freq, chans, flags | WMAEncodeFlags.Unicode, bitrate, file);
        }

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetwork")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, int bitrate, int port, int clients);

        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeOpenNetworkMulti")]
        public static extern int EncodeOpenNetwork(int freq, int chans, WMAEncodeFlags flags, int[] bitrate, int port, int clients);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int bitrate, string url, string user, string pass);

        public static int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int bitrate, string url, string user, string pass)
        {
            return BASS_WMA_EncodeOpenPublish(freq, chans, flags | WMAEncodeFlags.Unicode, bitrate, url, user, pass);
        }

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern int BASS_WMA_EncodeOpenPublishMulti(int freq, int chans, WMAEncodeFlags flags, int[] bitrate, string url, string user, string pass);

        public static int EncodeOpenPublish(int freq, int chans, WMAEncodeFlags flags, int[] bitrate, string url, string user, string pass)
        {
            return BASS_WMA_EncodeOpenPublishMulti(freq, chans, flags | WMAEncodeFlags.Unicode, bitrate, url, user, pass);
        }
        
		/// <summary>
		/// Sets a client connection notification callback on a network encoder.
		/// </summary>
		/// <param name="Handle">The encoder handle.</param>
		/// <param name="Procedure">User defined notification function... <see langword="null" /> = disable notifications.</param>
		/// <param name="User">User instance data to pass to the callback function.</param>
		/// <returns>If succesful, <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>A previously set notification callback can be changed (or removed) at any time, by calling this function again.</remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The encoder is not a network encoder, so no port is being used.</exception>
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeSetNotify")]
        public static extern bool EncodeSetNotify(int Handle, ClientConnectProcedure Procedure, IntPtr User);

        #region EncodeSetTag
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeSetTag")]
        public static extern bool EncodeSetTag(int Handle, IntPtr Tag, IntPtr Value, WMATagFormat Format);

        [DllImport(DllName)]
        static extern bool BASS_WMA_EncodeSetTag(int Handle, string Tag, IntPtr Value, WMATagFormat Format);

        [DllImport(DllName)]
        static extern bool BASS_WMA_EncodeSetTag(int Handle, string Tag, [In, Out] byte[] Value, WMATagFormat Format);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern bool BASS_WMA_EncodeSetTag(int Handle, string Tag, string Value, WMATagFormat Format = WMATagFormat.Unicode);

        public static bool EncodeSetTag(int Handle, string Tag, string Value)
        {
            return BASS_WMA_EncodeSetTag(Handle, Tag, Value);
        }

        public static bool EncodeSetTag(int Handle, string Tag, IntPtr Value, int Length)
        {
            return BASS_WMA_EncodeSetTag(Handle, Tag, Value, (WMATagFormat)BitHelper.MakeLong((short)WMATagFormat.Binary, (short)Length));
        }

        public static bool EncodeSetTag(int Handle, string Tag, byte[] Value, int Length)
        {
            return BASS_WMA_EncodeSetTag(Handle, Tag, Value, (WMATagFormat)BitHelper.MakeLong((short)WMATagFormat.Binary, (short)Length));
        }
        #endregion

        /// <summary>
        /// Retrieves the number of clients currently connected to the encoder.
        /// </summary>
        /// <param name="Handle">The encoder handle.</param>
        /// <returns>If succesful, the number of clients is returned, else -1 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The encoder was not created with <see cref="EncodeOpenNetwork(int, int, WMAEncodeFlags, int, int, int)" />.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetClients")]
        public static extern int EncodeGetClients(int Handle);
        
		/// <summary>
		/// Retrieves the network port for clients to connect to.
		/// </summary>
		/// <param name="Handle">The encoder handle.</param>
		/// <returns>If succesful, the port number is returned, else 0 is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// If you have choosen to let the system select a port (e.g. in your <see cref="EncodeOpenNetwork(int, int, WMAEncodeFlags, int, int, int)" /> or <see cref="EncodeOpenNetwork(int, int, WMAEncodeFlags, int[], int, int)" />), 
		/// this is the function to retrieve the port actually being used.
		/// </remarks>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        /// <exception cref="Errors.NotAvailable">The encoder is not a network encoder, so no port is being used.</exception>
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeGetPort")]
        public static extern int EncodeGetPort(int Handle);
        
		/// <summary>
		/// Finishes encoding and closes the file or network port.
		/// </summary>
		/// <param name="Handle">The encoder handle.</param>
        /// <returns>If successful, then <see langword="true" /> is returned, else <see langword="false" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
        /// <exception cref="Errors.Handle"><paramref name="Handle" /> is not valid.</exception>
        [DllImport(DllName, EntryPoint = "BASS_WMA_EncodeClose")]
        public static extern bool EncodeClose(int Handle);

        [DllImport(DllName)]
        static extern unsafe int* BASS_WMA_EncodeGetRates(int freq, int chans, WMAEncodeFlags Flags);
        
		/// <summary>
		/// Retrieves the WMA encoding bitrates available for a specified sample format.
		/// </summary>
		/// <param name="Frequency">The sample rate in Hz, or a BASS channel handle if the <see cref="WMAEncodeFlags.Source"/> flag is specified.</param>
		/// <param name="Channels">The number of channels (1=mono, 2=stereo, etc.).</param>
		/// <param name="Flags">A combination of <see cref="WMAEncodeFlags"/>.</param>
		/// <returns>If succesful, an array of the available bitrates is returned (int[], in bits per second), else <see langword="null" /> is returned. Use <see cref="Bass.LastError" /> to get the error code.</returns>
		/// <remarks>
		/// <para>When requesting VBR rates, the rates returned are quality settings. For example, 10 = 10% quality, 25 = 25% quality, etc... 100% quality is lossless.</para>
		/// <para>
        /// The WMA codec expects 16-bit or 24-bit sample data depending on the <see cref="WMAEncodeFlags.Encode24Bit"/> flag, but BassWma will accept 8-bit, 16-bit or floating-point data, and convert it to the appropriate format.
        /// Of course, it makes little sense to encode 8-bit or 16-bit data in 24-bit.
        /// </para>
		/// <para>
        /// The WMA codec currently supports the following sample rates: 8000, 11025, 16000, 22050, 32000, 44100, 48000, 88200, 96000.
        /// And the following number of channels: 1, 2, 6, 8.
        /// But not all combinations of these are supported.
        /// To encode other sample formats, the data will first have to be resampled to a supported format.
        /// </para>
		/// </remarks>
        /// <exception cref="Errors.WM9">The Windows Media modules (v9 or above) are not installed.</exception>
        /// <exception cref="Errors.NotAvailable">No codec could be found to support the specified sample format.</exception>
        /// <exception cref="Errors.Unknown">Some other mystery problem!</exception>
        public static unsafe int[] EncodeGetRates(int Frequency, int Channels, WMAEncodeFlags Flags)
        {
            var list = new List<int>();

            var rates = BASS_WMA_EncodeGetRates(Frequency, Channels, Flags);

            if (rates != null)
            {
                while (*rates != 0)
                {
                    list.Add(*rates);
                    rates++;
                }
            }
            else return null;

            return list.ToArray();
        }
        #endregion

        #region Configuration
        /// <summary>
        /// Enable network seeking?
        /// seek (bool): If true seeking in network files/streams is enabled (default is false).
        /// If true, it allows seeking before the entire file has been downloaded/cached.
        /// Seeking is slow that way, so it's disabled by default.
        /// </summary>
        public static bool CanSeekNetworkStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetSeek); }
            set { Bass.Configure(Configuration.WmaNetSeek, value); }
        }

        /// <summary>
        /// Play audio from WMV (video) files?
        /// playwmv (bool): If true (default) BASSWMA will play the audio from WMV video files.
        /// If false WMV files will not be played.
        /// </summary>
        public static bool PlayWMVAudio
        {
            get { return Bass.GetConfigBool(Configuration.WmaVideo); }
            set { Bass.Configure(Configuration.WmaVideo, value); }
        }

        /// <summary>
        /// Prebuffer internet streams on creation, before returning from BassWma.StreamCreateFile()?
        /// prebuf (bool): The Windows Media modules must prebuffer a stream before starting decoding/playback of it.
        /// This option determines when/where to wait for that to be completed.
        /// The Windows Media modules must prebuffer a stream before starting decoding/playback of it.
        /// This option determines whether the stream creation function (eg. BassWma.CreateStream())
        /// will wait for the prebuffering to complete before returning.
        /// If playback of a stream is attempted before it has prebuffered,
        /// it will stall and then resume once it has finished prebuffering.
        /// The prebuffering progress can be monitored via Bass.StreamGetFilePosition() (FileStreamPosition.WmaBuffer).
        /// This option is enabled by default.
        /// </summary>
        public static bool PrebufferInternetStreams
        {
            get { return Bass.GetConfigBool(Configuration.WmaNetPreBuffer); }
            set { Bass.Configure(Configuration.WmaNetPreBuffer, value); }
        }

        /// <summary>
        /// Use BASS file handling.
        /// bassfile (bool): Default is disabled (false).
        /// When enabled (true) BASSWMA uses BASS's file routines when playing local files.
        /// It uses the IStream interface to do that.
        /// This would also allow to support the "offset" parameter for WMA files with Bass.CreateStream().
        /// The downside of enabling this feature is, that it stops playback while encoding from working.
        /// </summary>
        public static bool UseBassFileHandling
        {
            get { return Bass.GetConfigBool(Configuration.WmaBassFileHandling); }
            set { Bass.Configure(Configuration.WmaBassFileHandling, value); }
        }

        /// <summary>
        /// Use a seperate thread to decode the data?
        /// async (bool): If true BASSWMA will decode the data in a seperate thread.
        /// If false (default) the normal file system will be used.
        /// The WM decoder can by synchronous (decodes data on demand) or asynchronous (decodes in the background).
        /// With the background decoding, BASSWMA buffers the data that it receives from the decoder for the STREAMPROC to access.
        /// The start of playback/seeking may well be slightly delayed due to there being no data available immediately.
        /// Internet streams are only supported by the asynchronous system,
        /// but local files can use either, and BASSWMA uses the synchronous system by default.
        /// </summary>
        public static bool AsyncDecoding
        {
            get { return Bass.GetConfigBool(Configuration.WmaAsync); }
            set { Bass.Configure(Configuration.WmaAsync, value); }
        }
        #endregion

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        static extern IntPtr BASS_WMA_GetTags(string File, BassFlags Flags);

        public static string[] GetTags(string File, BassFlags Flags) => Extensions.ExtractMultiStringUtf8(BASS_WMA_GetTags(File, Flags | BassFlags.Unicode));
    }
}