namespace ManagedBass.DirectX8
{
    public sealed class DXParamEQEffect : Effect<DXParamEQParameters>
    {
        public DXParamEQEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXParamEQEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public double Center
        {
            get { return Parameters.fCenter; }
            set
            {
                Parameters.fCenter = (float)value;

                OnPropertyChanged();
            }
        }

        public double Bandwidth
        {
            get { return Parameters.fBandwidth; }
            set
            {
                Parameters.fBandwidth = (float)value;

                OnPropertyChanged();
            }
        }

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