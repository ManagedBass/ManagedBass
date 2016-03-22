using System;

namespace ManagedBass.Wasapi
{
    /// <summary>
    /// Provides audio from <see cref="WasapiRecordingDevice"/> or <see cref="WasapiLoopbackDevice"/> in a Bass Channel.
    /// </summary>
    public class WasapiToBassFullDuplex : Channel
    {
        WasapiDevice WasapiDevice;

        public WasapiToBassFullDuplex(WasapiRecordingDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        public WasapiToBassFullDuplex(WasapiLoopbackDevice Device, bool Decode = false)
            : this(Device as WasapiDevice, Decode) { Device.Init(); }

        WasapiToBassFullDuplex(WasapiDevice WasapiDevice, bool Decode = false)
        {
            var info = WasapiDevice.DeviceInfo;

            this.WasapiDevice = WasapiDevice;

            Handle = Bass.CreateStream(info.MixFrequency, info.MixChannels, Decode ? BassFlags.Decode : 0, StreamProcedureType.Push);

            WasapiDevice.Callback += (s) => Bass.StreamPutData(Handle, s.Pointer, s.ByteLength);
        }

        public override bool Start()
        {
            WasapiDevice.Start();

            return base.Start();
        }

        public override bool Pause()
        {
            bool Result = base.Pause();

            WasapiDevice.Stop();

            return Result;
        }

        public override bool Stop()
        {
            bool Result = base.Stop();

            WasapiDevice.Stop();

            return Result;
        }

        public override void Dispose()
        {
            base.Dispose();

            WasapiDevice.Dispose();
        }
    }
}
