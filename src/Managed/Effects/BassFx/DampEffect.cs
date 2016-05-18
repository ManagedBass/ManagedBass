namespace ManagedBass.Fx
{
    public sealed class DampEffect : Effect<DampParameters>
    {
        public DampEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DampEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        /// <summary>
        /// Amplification level (0...1...n, linear). 
        /// </summary>
        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Amplification adjustment rate (0...1, linear).
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
        /// Target volume level (0&lt;...1, linear).
        /// </summary>
        public double Target
        {
            get { return Parameters.fTarget; }
            set
            {
                Parameters.fTarget = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Quiet volume level (0...1, linear). 
        /// </summary>
        public double Quiet
        {
            get { return Parameters.fQuiet; }
            set
            {
                Parameters.fQuiet = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Delay in seconds before increasing level (0...n, linear).
        /// </summary>
        public double Delay
        {
            get { return Parameters.fDelay; }
            set
            {
                Parameters.fDelay = (float)value;

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