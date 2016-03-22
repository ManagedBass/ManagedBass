namespace ManagedBass.Asio
{
	/// <summary>
	/// BassAsio Future values that might be used with the <see cref="BassAsio.Future" /> method.
	/// <para>Note: Other/Additional values might be possible - see your drivers manual for details.</para>
	/// </summary>
	public enum AsioFuture
	{
		/// <summary>
		/// No arguments
		/// </summary>
		EnableTimeCodeRead = 1,

		/// <summary>
		/// No arguments
		/// </summary>
		DisableTimeCodeRead = 2,

		/// <summary>
		/// ASIOInputMonitor* in params
		/// </summary>
		SetInputMonitor = 3,

		/// <summary>
		/// ASIOTransportParameters* in params
		/// </summary>
		Transport = 4,

		/// <summary>
		/// ASIOChannelControls* in params, apply gain
		/// </summary>
		SetInputGain = 5,

		/// <summary>
		/// ASIOChannelControls* in params, fill meter
		/// </summary>
		GetInputMeter = 6,

		/// <summary>
		/// ASIOChannelControls* in params, apply gain
		/// </summary>
		SetOutputGain = 7,

		/// <summary>
		/// ASIOChannelControls* in params, fill meter
		/// </summary>
		GetOutputMeter = 8,

		/// <summary>
		/// No arguments
		/// </summary>
		CanInputMonitor = 9,

		/// <summary>
		/// No arguments
		/// </summary>
		CanTimeInfo = 10,

		/// <summary>
		/// No arguments
		/// </summary>
		CanTimeCode = 11,

		/// <summary>
		/// No arguments
		/// </summary>
		CanTransport = 12,

		/// <summary>
		/// No arguments
		/// </summary>
		CanInputGain = 13,

		/// <summary>
		/// No arguments
		/// </summary>
		CanInputMeter = 14,

		/// <summary>
		/// No arguments
		/// </summary>
		CanOutputGain = 15,

		/// <summary>
		/// No arguments
		/// </summary>
		CanOutputMeter = 16,

		/// <summary>
		/// DSD support: ASIOIoFormat * in params
		/// </summary>
		SetIoFormat = 588323169,

		/// <summary>
		/// DSD support: ASIOIoFormat * in params
		/// </summary>
		GetIoFormat = 588323203,

		/// <summary>
		/// DSD support: ASIOIoFormat * in params
		/// </summary>
		CanDoIoFormat = 588324868
	}
}
