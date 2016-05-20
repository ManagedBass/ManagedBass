namespace ManagedBass.DirectX8
{
    public sealed class DXDistortionEffect : Effect<DXDistortionParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXDistortionEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXDistortionEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXDistortionEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXDistortionEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Amount of signal change after distortion, in the range from -60 through 0. The default value is 0 dB.
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
        /// Percentage of distortion intensity, in the range in the range from 0 through 100. The default value is 50 percent.
        /// </summary>
        public double Edge
        {
            get { return Parameters.fEdge; }
            set
            {
                Parameters.fEdge = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Center frequency of harmonic content addition, in the range from 100 through 8000. The default value is 4000 Hz.
        /// </summary>
        public double PostEQCenterFrequency
        {
            get { return Parameters.fPostEQCenterFrequency; }
            set
            {
                Parameters.fPostEQCenterFrequency = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Width of frequency band that determines range of harmonic content addition, in the range from 100 through 8000. The default value is 4000 Hz.
        /// </summary>
        public double PostEQBandwidth
        {
            get { return Parameters.fPostEQBandwidth; }
            set
            {
                Parameters.fPostEQBandwidth = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Filter cutoff for high-frequency harmonics attenuation, in the range from 100 through 8000. The default value is 4000 Hz.
        /// </summary>
        public double PreLowpassCutoff
        {
            get { return Parameters.fPreLowpassCutoff; }
            set
            {
                Parameters.fPreLowpassCutoff = (float)value;

                OnPropertyChanged();
            }
        }
    }
}