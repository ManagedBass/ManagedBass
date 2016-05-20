namespace ManagedBass.DirectX8
{
    public sealed class DXEchoEffect : Effect<DXEchoParameters>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DXEchoEffect"/>.
        /// </summary>
        /// <param name="Channel">The <paramref name="Channel"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXEchoEffect(int Channel, int Priority = 0) : base(Channel, Priority) { }

        /// <summary>
        /// Creates a new instance of <see cref="DXEchoEffect"/> supporting <see cref="MediaPlayer"/>'s persistence.
        /// </summary>
        /// <param name="Player">The <see cref="MediaPlayer"/> to apply the effect on.</param>
        /// <param name="Priority">Priority of the Effect... default = 0.</param>
        public DXEchoEffect(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        /// <summary>
        /// Ratio of wet (processed) signal to dry (unprocessed) signal. Must be in the range from 0 (default) through 100 (all wet).
        /// </summary>
        public double WetDryMix
        {
            get { return Parameters.fWetDryMix; }
            set
            {
                Parameters.fWetDryMix = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Percentage of output fed back into input, in the range from 0 through 100. The default value is 0.
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
        /// Delay for left channel, in milliseconds, in the range from 1 through 2000. The default value is 333 ms.
        /// </summary>
        public double LeftDelay
        {
            get { return Parameters.fLeftDelay; }
            set
            {
                Parameters.fLeftDelay = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Delay for right channel, in milliseconds, in the range from 1 through 2000. The default value is 333 ms.
        /// </summary>
        public double RightDelay
        {
            get { return Parameters.fRightDelay; }
            set
            {
                Parameters.fRightDelay = (float)value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Value that specifies whether to swap left and right delays with each successive echo. The default value is <see langword="false" />, meaning no swap.
        /// </summary>
        public bool PanDelay
        {
            get { return Parameters.lPanDelay; }
            set
            {
                Parameters.lPanDelay = value;

                OnPropertyChanged();
            }
        }
    }
}