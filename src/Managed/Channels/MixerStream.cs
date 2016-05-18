using System;

namespace ManagedBass.Mix
{
    /// <summary>
    /// Represents a Mixer Stream.
    /// </summary>
    public sealed class MixerStream : Channel
    {
        /// <summary>
        /// Creates a new instance of <see cref="MixerStream"/>.
        /// </summary>
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

        /// <summary>
        /// Adds a Channel to the Mixer.
        /// </summary>
        public bool AddChannel(Channel Channel) => AddChannel(Channel.Handle);

        /// <summary>
        /// Adds a Channel to the Mixer.
        /// </summary>
        public bool AddChannel(int Channel) => BassMix.MixerAddChannel(Handle, Channel, BassFlags.Default);

        /// <summary>
        /// Removes a Channel from the Mixer.
        /// </summary>
        public bool RemoveChannel(Channel Channel) => RemoveChannel(Channel.Handle);

        /// <summary>
        /// Removes a Channel from the Mixer.
        /// </summary>
        public bool RemoveChannel(int Channel) => BassMix.MixerRemoveChannel(Channel);
    }
}