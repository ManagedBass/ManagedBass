namespace ManagedBass.DirectX8
{
    public sealed class DXEchoEffect : Effect<DXEchoParameters>
    {
        public DXEchoEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXEchoEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public double WetDryMix
        {
            get { return Parameters.fWetDryMix; }
            set
            {
                Parameters.fWetDryMix = (float)value;

                OnPropertyChanged();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;

                OnPropertyChanged();
            }
        }

        public double LeftDelay
        {
            get { return Parameters.fLeftDelay; }
            set
            {
                Parameters.fLeftDelay = (float)value;

                OnPropertyChanged();
            }
        }

        public double RightDelay
        {
            get { return Parameters.fRightDelay; }
            set
            {
                Parameters.fRightDelay = (float)value;

                OnPropertyChanged();
            }
        }

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