namespace ManagedBass.DirectX8
{
    public sealed class DXGargleEffect : Effect<DXGargleParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXGargleEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXGargleEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXGargleEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXGargleEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Rate of modulation, in Hertz. Must be in the range from 1 through 1000. Default 500 Hz.
        /// </summary>
        public int Rate
        {
            get { return Parameters.dwRateHz; }
            set
            {
                Parameters.dwRateHz = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Shape of the modulation wave. Default = <see cref="DXWaveform.Sine"/>.
        /// </summary>
        public DXWaveform WaveShape
        {
            get { return Parameters.dwWaveShape; }
            set
            {
                Parameters.dwWaveShape = value;

                OnPropertyChanged();
            }
        }
    }
}