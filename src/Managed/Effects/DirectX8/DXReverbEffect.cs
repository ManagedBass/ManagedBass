namespace ManagedBass.DirectX8
{
    public sealed class DXReverbEffect : Effect<DXReverbParameters>
    {
        public DXReverbEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXReverbEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public double InGain
        {
            get { return Parameters.fInGain; }
            set
            {
                Parameters.fInGain = (float)value;

                OnPropertyChanged();
            }
        }

        public double ReverbMix
        {
            get { return Parameters.fReverbMix; }
            set
            {
                Parameters.fReverbMix = (float)value;

                OnPropertyChanged();
            }
        }

        public double ReverbTime
        {
            get { return Parameters.fReverbTime; }
            set
            {
                Parameters.fReverbTime = (float)value;

                OnPropertyChanged();
            }
        }

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