namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="Bass.ChannelSetFX" />, <see cref="Bass.FXSetParameters" /> and <see cref="Bass.FXGetParameters" /> to retrieve and set the parameters of the DSP effect Rotate.
    /// </summary>
    /// <remarks>
    /// <para>This is a volume rotate effect between even channels, just like 2 channels playing ping-pong between each other.</para>
    /// <para>The <see cref="Rate"/> defines the speed in Hz.</para>
    /// </remarks>
    public sealed class RotateEffect : Effect<RotateParameters>
    {
        public RotateEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }
        
        public RotateEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        /// <summary>
        /// Rotation rate/speed in Hz (A negative rate can be used for reverse direction).
        /// </summary>
        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A <see cref="FXChannelFlags" /> flag to define on which channels to apply the effect. Default: <see cref="FXChannelFlags.All"/>
        /// </summary>
        public FXChannelFlags Channels
        {
            get { return Parameters.lChannel; }
            set
            {
                Parameters.lChannel = value;

                OnPropertyChanged();
            }
        }
    }
}