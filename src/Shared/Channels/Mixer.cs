using System;

namespace ManagedBass.Mix
{
    /// <summary>
    /// Represents a Mixer Stream.
    /// </summary>
    public sealed class Mixer : Channel
    {
        /// <summary>
        /// Creates a new instance of <see cref="Mixer"/>.
        /// </summary>
        public Mixer(PCMFormat Format, BassFlags Flags = BassFlags.Default)
        {
            Handle = BassMix.CreateMixerStream(Format.Frequency, Format.Channels, Flags);
        }

        #region Read
        public override int GetData(IntPtr Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int GetData(byte[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);

        public override int GetData(short[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);
        
        public override int GetData(float[] Buffer, int Length) => BassMix.ChannelGetData(Handle, Buffer, Length);
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