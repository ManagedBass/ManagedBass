using System;

namespace ManagedBass.Mix
{
    /// <summary>
    /// Represents a Mixer Stream.
    /// </summary>
    public sealed class MixerStream : Channel
    {
        public MixerStream(PCMFormat Format, bool IsDecoder = true)
        {
            Handle = BassMix.CreateMixerStream(Format.Frequency, Format.Channels, FlagGen(IsDecoder, Format.Resolution));
        }

        #region Read
        public override int Read(IntPtr Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(byte[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(short[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(int[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int Read(float[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);
        #endregion

        public bool AddChannel(Channel Channel) => AddChannel(Channel.Handle);

        public bool AddChannel(int Channel) => BassMix.MixerAddChannel(Handle, Channel, BassFlags.Default);

        public bool RemoveChannel(Channel Channel) => RemoveChannel(Channel.Handle);

        public bool RemoveChannel(int Channel) => BassMix.MixerRemoveChannel(Channel);
    }
}