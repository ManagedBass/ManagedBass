namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Provides audio from <see cref="WasapiRecordingDevice"/> or <see cref="WasapiLoopbackDevice"/> in a Bass Channel.
    /// </summary>
    public sealed class WasapiToBassStream
    {
        /// <summary>
        /// Gets the Channel Handle.
        /// </summary>
        public int Handle { get; }

        readonly WasapiDevice _wasapiDevice;

        /// <summary>
        /// Creates a new instance of <see cref="WasapiToBassStream"/>.
        /// </summary>
        /// <param name="Device"><see cref="WasapiRecordingDevice"/> to use.</param>
        /// <param name="Decode">Whether to create a decoding channel.</param>
        public WasapiToBassStream(WasapiRecordingDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        /// <summary>
        /// Creates a new instance of <see cref="WasapiToBassStream"/>.
        /// </summary>
        /// <param name="Device"><see cref="WasapiLoopbackDevice"/> to use.</param>
        /// <param name="Decode">Whether to create a decoding channel.</param>
        public WasapiToBassStream(WasapiLoopbackDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        WasapiToBassStream(WasapiDevice WasapiDevice, bool Decode = false)
        {
            var info = WasapiDevice.Info;

            _wasapiDevice = WasapiDevice;

            Handle = Bass.CreateStream(info.MixFrequency, info.MixChannels, Decode ? BassFlags.Decode : 0, StreamProcedureType.Push);

            WasapiDevice.Callback += (B, L) => Bass.StreamPutData(Handle, B, L);
        }

        /// <summary>
        /// Starts the Channel Playback.
        /// </summary>
        public bool Play()
        {
            _wasapiDevice.Start();

            return Bass.ChannelPlay(Handle);
        }

        /// <summary>
        /// Stops the Channel Playback.
        /// </summary>
        public bool Stop()
        {
            var result = Bass.ChannelStop(Handle);

            _wasapiDevice.Stop();

            return result;
        }

        /// <summary>
        /// Frees all resources used by this channel.
        /// </summary>
        public void Dispose()
        {
            Bass.StreamFree(Handle);

            _wasapiDevice.Dispose();
        }
    }
}