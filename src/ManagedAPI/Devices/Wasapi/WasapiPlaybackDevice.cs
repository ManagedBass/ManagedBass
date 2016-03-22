using ManagedBass.Mix;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass.Wasapi
{
    public class WasapiPlaybackDevice : WasapiDevice
    {
        WasapiPlaybackDevice(int Index) : base(Index) { }

        public static WasapiPlaybackDevice Get(int Device)
        {
            if (Singleton.ContainsKey(Device))
                return Singleton[Device] as WasapiPlaybackDevice;
            else
            {
                WasapiDeviceInfo info;
                if (!BassWasapi.GetDeviceInfo(Device, out info) || info.IsInput)
                    throw new ArgumentException("Invalid WasapiPlaybackDevice Index");

                var Dev = new WasapiPlaybackDevice(Device);
                Singleton.Add(Device, Dev);

                return Dev;
            }
        }

        public static IEnumerable<WasapiPlaybackDevice> Devices
        {
            get
            {
                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (!dev.IsInput)
                        yield return Get(i);
            }
        }

        public bool Init(int Frequency = 44100, int Channels = 2, bool Shared = true, bool UseEventSync = false, int Buffer = 0, int Period = 0)
        {
            bool Result = base._Init(Frequency, Channels, Shared, UseEventSync, Buffer, Period);

            BassWasapi.CurrentDevice = DeviceIndex;
            var info = BassWasapi.Info;

            Bass.Init(0);

            MixerStream = BassMix.CreateMixerStream(info.Frequency, info.Channels, BassFlags.Float | BassFlags.Decode);

            return Result;
        }

        #region Callback
        int MixerStream;

        Dictionary<Action<BufferProvider>, Tuple<StreamProcedure, int>> dict = new Dictionary<Action<BufferProvider>, Tuple<StreamProcedure, int>>();

        public override int OnProc(IntPtr Buffer, int Length, IntPtr User)
        {
            return Bass.ChannelGetData(MixerStream, Buffer, Length);
        }

        /// <summary>
        /// Adds a Bass Channel to Wasapi Output Mixer.
        /// </summary>
        /// <returns>True on Success</returns>
        public bool AddOutputSource(int Channel)
        {
            ChannelInfo info;

            if (!Bass.ChannelGetInfo(Channel, out info)
                || (!info.IsDecodingChannel && info.ChannelType != ChannelType.Recording))
                return false;

            return BassMix.MixerAddChannel(MixerStream, Channel, BassFlags.Default);
        }

        /// <summary>
        /// Removes a Bass Channel from Wasapi Output Mixer.
        /// </summary>
        /// <returns>True on Success</returns>
        public bool RemoveOutputSource(int Channel) => BassMix.MixerRemoveChannel(Channel);

        public override event Action<BufferProvider> Callback
        {
            add
            {
                StreamProcedure sproc = (h, b, l, u) =>
                    {
                        value.Invoke(new BufferProvider(b, l));

                        return l;
                    };

                BassWasapi.CurrentDevice = DeviceIndex;
                var info = BassWasapi.Info;

                int handle = Bass.CreateStream(info.Frequency, info.Channels, BassFlags.Decode | BassFlags.Float, sproc);

                AddOutputSource(handle);

                dict.Add(value, new Tuple<StreamProcedure, int>(sproc, handle));
            }
            remove
            {
                var t = dict[value].Item2;

                RemoveOutputSource(t);

                Bass.StreamFree(t);
                dict.Remove(value);
            }
        }
        #endregion

        public static WasapiPlaybackDevice DefaultDevice => Devices.First((dev) => dev.DeviceInfo.IsDefault);

        public static int Count
        {
            get
            {
                int Count = 0;

                WasapiDeviceInfo dev;

                for (int i = 0; BassWasapi.GetDeviceInfo(i, out dev); ++i)
                    if (!dev.IsInput)
                        Count++;

                return Count;
            }
        }

        public WasapiLoopbackDevice LoopbackDevice
        {
            get
            {
                foreach (var dev in WasapiLoopbackDevice.Devices)
                    if (dev.DeviceInfo.ID == DeviceInfo.ID)
                        return dev;

                throw new Exception("Could not find a Loopback Device.");
            }
        }
    }
}
