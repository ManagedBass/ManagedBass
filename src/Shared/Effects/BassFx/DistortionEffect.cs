namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx Distortion Effect.
    /// </summary>
    public sealed class DistortionEffect : Effect<DistortionParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DistortionEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DistortionEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DistortionEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DistortionEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        #region Presets
        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Hard()
        {
            Parameters.fDrive = 1;
            Parameters.fDryMix = 0;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0;
            Parameters.fVolume = 1;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void VeryHard()
        {
            Parameters.fDrive = 5;
            Parameters.fDryMix = 0;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0.1f;
            Parameters.fVolume = 1;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Medium()
        {
            Parameters.fDrive = 0.2f;
            Parameters.fDryMix = 1;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0.1f;
            Parameters.fVolume = 1;

            OnPropertyChanged("");
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Soft()
        {
            Parameters.fDrive = 0;
            Parameters.fDryMix = -2.95f;
            Parameters.fWetMix = -0.05f;
            Parameters.fFeedback = -0.18f;
            Parameters.fVolume = 0.25f;

            OnPropertyChanged("");
        }
        #endregion

        /// <summary>
        /// Distortion Drive (0...5).
        /// </summary>
        public double Drive
        {
            get { return Parameters.fDrive; }
            set
            {
                Parameters.fDrive = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Dry (unaffected) signal mix (-5...+5).
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
        /// Distortion volume (0...+2).
        /// </summary>
        public double Volume
        {
            get { return Parameters.fVolume; }
            set
            {
                Parameters.fVolume = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Wet (affected) signal mix (-5...+5).
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