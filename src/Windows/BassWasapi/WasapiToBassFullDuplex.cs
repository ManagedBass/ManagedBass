﻿namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Provides audio from <see cref="WasapiRecordingDevice"/> or <see cref="WasapiLoopbackDevice"/> in a Bass Channel.
    /// </summary>
    public sealed class WasapiToBassFullDuplex : Channel
    {
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
        public override bool Play(bool Restart = false)
        {
            _wasapiDevice.Start();

            return base.Play(Restart);
        }

        public override bool Pause()
        {
            var result = base.Pause();

            _wasapiDevice.Stop();

            return result;
        }

        public override bool Stop()
        {
            var result = base.Stop();

            _wasapiDevice.Stop();

            return result;
        }

        public override void Dispose()
        {
            base.Dispose();

            _wasapiDevice.Dispose();
        }
    }
}