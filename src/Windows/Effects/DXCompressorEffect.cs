namespace ManagedBass.DirectX8
{
    public sealed class DXCompressorEffect : Effect<DXCompressorParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXCompressorEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXCompressorEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXCompressorEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXCompressorEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Output gain of signal in dB after compression, in the range from -60 to 60. The default value is 0 dB.
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
        /// Time in ms before compression reaches its full value, in the range from 0.01 to 500. The default value is 10 ms.
        /// </summary>
        public double Attack
        {
            get { return Parameters.fAttack; }
            set
            {
                Parameters.fAttack = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Time (speed) in ms at which compression is stopped after input drops below <see cref="Threshold"/>, in the range from 50 to 3000. The default value is 200 ms.
        /// </summary>
        public double Release
        {
            get { return Parameters.fRelease; }
            set
            {
                Parameters.fRelease = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Point at which compression begins, in dB, in the range from -60 to 0. The default value is -20 dB.
        /// </summary>
        public double Threshold
        {
            get { return Parameters.fThreshold; }
            set
            {
                Parameters.fThreshold = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Compression ratio, in the range from 1 to 100. The default value is 3, which means 3:1 compression.
        /// </summary>
        public double Ratio
        {
            get { return Parameters.fRatio; }
            set
            {
                Parameters.fRatio = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Time in ms after <see cref="Threshold"/> is reached before attack phase is started, in milliseconds, in the range from 0 to 4. The default value is 4 ms.
        /// </summary>
        public double Predelay
        {
            get { return Parameters.fPredelay; }
            set
            {
                Parameters.fPredelay = (float)value;

                OnPropertyChanged();
            }
        }
    }
}