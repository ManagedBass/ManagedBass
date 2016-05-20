namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx Echo Effect.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is an echo effect that replays what you have played one or more times after a period of time.
    /// It's something like the echoes you might hear shouting against a canyon wall.
    /// For reverb effect enable feedback.
    /// </para>
    /// <para>
    /// The <see cref="DryMix"/> is the volume of input signal and the <see cref="WetMix"/> is the volume of delayed signal.
    /// The <see cref="Delay"/> is the delay time in sec.
    /// The <see cref="Feedback"/> sets how much delay is feed back to the input (for repeating delays).
    /// If <see cref="Stereo"/> is enabled and a stream has an even number of channels then, each even channels will be echoed to each other.
    /// </para>
    /// </remarks>
    public sealed class EchoEffect : Effect<EchoParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="EchoEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public EchoEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="EchoEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public EchoEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        #region Presets
        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Small()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fDelay = 0.2f;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void ManyEchoes()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.7f;
            Parameters.fDelay = 0.5f;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void ReverseEchoes()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = -0.7f;
            Parameters.fDelay = 0.8f;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void RoboticVoice()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 0.8f;
            Parameters.fFeedback = 0.5f;
            Parameters.fDelay = 0.1f;

            OnPropertyChanged("");
        }
        #endregion

        /// <summary>
        /// Dry (unaffected) signal mix (-2...+2). 
        /// </summary>
        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Wet (affected) signal mix (-2...+2). 
        /// </summary>
        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Feedback (-1...+1).
        /// </summary>
        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Delay in seconds (0+...6).
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
        /// Echo adjoining channels to each other? Default is disabled.
        /// </summary>
        /// <remarks>Only allowed with even number of channels!
        /// <para>If enabled and a stream has an even number of channels then, each even channels will be echoed to each other.</para>
        /// </remarks>
        public bool Stereo
        {
            get { return Parameters.bStereo != 0; }
            set
            {
                Parameters.bStereo = value ? 1 : 0;

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