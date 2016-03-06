using ManagedBass.Dynamics;
using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    public class MixerStream : Channel
    {
        public MixerStream(int Frequency = 44100, int NoOfChannels = 2, bool IsDecoder = true, Resolution Resolution = Resolution.Short)
        {
            Handle = BassMix.CreateMixerStream(Frequency, NoOfChannels, FlagGen(IsDecoder, Resolution));
        }

        #region Read
        public override int Read(IntPtr Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(byte[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(short[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(int[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(float[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);
        #endregion

        public bool AddChannel(Channel channel) => AddChannel(channel.Handle);

        public bool AddChannel(int channel) => BassMix.MixerAddChannel(Handle, channel, BassFlags.Default);

        public bool RemoveChannel(Channel channel) => RemoveChannel(channel.Handle);

        public bool RemoveChannel(int channel) => BassMix.MixerRemoveChannel(channel);
    }
}