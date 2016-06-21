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
        public Mixer(int Frequency = 44100, int Channels = 2, BassFlags Flags = BassFlags.Default)
        {
            Handle = BassMix.CreateMixerStream(Frequency, Channels, Flags);
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
        public bool AddChannel(Channel Channel, BassFlags Flags = BassFlags.Default) => BassMix.MixerAddChannel(Handle, Channel.Handle, Flags);
        
        /// <summary>
        /// Removes a Channel from the Mixer.
        /// </summary>
        public bool RemoveChannel(Channel Channel) => BassMix.MixerRemoveChannel(Channel.Handle);
    }
}