namespace ManagedBass.DirectX8
{
    public sealed class DXParamEQEffect : Effect<DXParamEQParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXParamEQEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXParamEQEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXParamEQEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXParamEQEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Center frequency, in hertz, in the range from 80 to 16000. This value cannot exceed one-third of the frequency of the channel. Default 100 Hz.
        /// </summary>
        public double Center
        {
            get { return Parameters.fCenter; }
            set
            {
                Parameters.fCenter = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Bandwidth, in semitones, in the range from 1 to 36. Default 18 semitones.
        /// </summary>
        public double Bandwidth
        {
            get { return Parameters.fBandwidth; }
            set
            {
                Parameters.fBandwidth = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gain, in the range from -15 to 15. Default 0 dB.
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
    }
}