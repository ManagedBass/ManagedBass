using System.Runtime.InteropServices;

namespace ManagedBass.Wasapi
{
    /// <summary>
	/// Used with <see cref="BassWasapi.GetInfo(out WasapiInfo)" /> to retrieve information on the current device.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
    public struct WasapiInfo
    {
        WasapiInitFlags initflags;
        int freq;
        int chans;
        WasapiFormat format;
        int buflen;
        int volmax;
        int volmin;
        int volstep;

        /// <summary>
		/// The flags parameter of the <see cref="BassWasapi.Init" /> call.
		/// </summary>
		public WasapiInitFlags InitFlags => initflags;
        
        /// <summary>
		/// The device's sample format used.
		/// </summary>
		public WasapiFormat Format => format;

        /// <summary>
		/// The sample rate used.
		/// </summary>
		public int Frequency => freq;
        
        /// <summary>
		/// The number of channels used (1 = mono, 2 = stereo, etc.).
		/// </summary>
		public int Channels => chans;
        
        /// <summary>
		/// The buffer size in bytes.
		/// </summary>
		public int BufferLength => buflen;
        public int MaxVolume => volmax;
        public int MinVolume => volmin;
        public int VolumeStep => volstep;

        /// <summary>
		/// Is the device used in event-driven mode?
		/// </summary>
		public bool IsEventDriven => initflags.HasFlag(WasapiInitFlags.EventDriven);
        
        /// <summary>
		/// Is the device used in exclusive mode?
		/// </summary>
		public bool IsExclusive => initflags.HasFlag(WasapiInitFlags.Exclusive);
    }
}