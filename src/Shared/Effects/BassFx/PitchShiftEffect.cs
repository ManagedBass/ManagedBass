namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx Pitch shift Effect.
    /// </summary>
    /// <remarks>
    /// This effect uses FFT for its pitch shifting while maintaining duration.
    /// </remarks>
    public sealed class PitchShiftEffect : Effect<PitchShiftParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="PitchShiftEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public PitchShiftEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="PitchShiftEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public PitchShiftEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// A factor value which is between 0.5 (one octave down) and 2 (one octave up) (1 won't change the pitch, default).
        /// </summary>
        public double PitchShift
        {
            get { return Parameters.fPitchShift; }
            set
            {
                Parameters.fPitchShift = (float)value;

                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Semitones (0 won't change the pitch). Default = 0.
        /// </summary>
        public double Semitones
        {
            get { return Parameters.fSemitones; }
            set
            {
                Parameters.fSemitones = (float)value;

                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Defines the FFT frame size used for the processing. Typical values are 1024, 2048 (default) and 4096, max is 8192.
        /// </summary>
        /// <remarks>It may be any value up to 8192 but it MUST be a power of 2.</remarks>
        public long FFTFrameSize
        {
            get { return Parameters.lFFTsize; }
            set
            {
                Parameters.lFFTsize = value;

                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Is the STFT oversampling factor which also determines the overlap between adjacent STFT frames. Default = 8.
        /// </summary>
        /// <remarks>It should at least be 4 for moderate scaling ratios. A value of 32 is recommended for best quality (better quality = higher CPU usage).</remarks>
        public long OversamplingFactor
        {
            get { return Parameters.lOsamp; }
            set
            {
                Parameters.lOsamp = value;

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