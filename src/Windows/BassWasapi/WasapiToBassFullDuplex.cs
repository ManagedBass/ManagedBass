namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Provides audio from <see cref="WasapiRecordingDevice"/> or <see cref="WasapiLoopbackDevice"/> in a Bass Channel.
    /// </summary>
    public sealed class WasapiToBassFullDuplex
    {
        public int Handle { get; }
        readonly WasapiDevice _wasapiDevice;

        public WasapiToBassFullDuplex(WasapiRecordingDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        public WasapiToBassFullDuplex(WasapiLoopbackDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        WasapiToBassFullDuplex(WasapiDevice WasapiDevice, bool Decode = false)
        {
            var info = WasapiDevice.Info;

            _wasapiDevice = WasapiDevice;

            Handle = Bass.CreateStream(info.MixFrequency, info.MixChannels, Decode ? BassFlags.Decode : 0, StreamProcedureType.Push);

            WasapiDevice.Callback += s => Bass.StreamPutData(Handle, s.Pointer, s.Length);
        }

        /// <summary>
        /// Starts the Channel Playback.
        /// </summary>
        public bool Play()
        {
            _wasapiDevice.Start();

            return Bass.ChannelPlay(Handle);
        }

        public bool Stop()
        {
            var result = Bass.ChannelStop(Handle);

            _wasapiDevice.Stop();

            return result;
        }

        public void Dispose()
        {
            Bass.StreamFree(Handle);

            _wasapiDevice.Dispose();
        }
    }
}