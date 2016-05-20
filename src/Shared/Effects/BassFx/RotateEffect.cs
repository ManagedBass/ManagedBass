namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx Rotate Effect.
    /// </summary>
    /// <remarks>
    /// <para>This is a volume rotate effect between even channels, just like 2 channels playing ping-pong between each other.</para>
    /// <para>The <see cref="Rate"/> defines the speed in Hz.</para>
    /// </remarks>
    public sealed class RotateEffect : Effect<RotateParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RotateEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public RotateEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="RotateEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public RotateEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

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