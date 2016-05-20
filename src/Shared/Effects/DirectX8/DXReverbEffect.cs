namespace ManagedBass.DirectX8
{
    public sealed class DXReverbEffect : Effect<DXReverbParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXReverbEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXReverbEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXReverbEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXReverbEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Input gain of signal, in decibels (dB), in the range from -96 through 0. The default value is 0 dB.
        /// </summary>
        public double InGain
        {
            get { return Parameters.fInGain; }
            set
            {
                Parameters.fInGain = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Reverb mix, in dB, in the range from -96 through 0. The default value is 0 dB.
        /// </summary>
        public double ReverbMix
        {
            get { return Parameters.fReverbMix; }
            set
            {
                Parameters.fReverbMix = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Reverb time, in milliseconds, in the range from 0.001 through 3000. The default value is 1000.
        /// </summary>
        public double ReverbTime
        {
            get { return Parameters.fReverbTime; }
            set
            {
                Parameters.fReverbTime = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// In the range from 0.001 through 0.999. The default value is 0.001.
        /// </summary>
        public double HighFreqRTRatio
        {
            get { return Parameters.fHighFreqRTRatio; }
            set
            {
                Parameters.fHighFreqRTRatio = (float)value;

                OnPropertyChanged();
            }
        }
    }
}