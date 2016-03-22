namespace ManagedBass
{
    public class GainDSP : DSP
    {
        int n;

        public GainDSP(int Channel, int Priority = 0) : base(Channel, Priority) { }

        public GainDSP(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        float gain = 1;
        public double Gain
        {
            get { return gain; }
            set
            {
                gain = (float)value.Clip(0, 1024);

                OnPropertyChanged();
            }
        }

        protected override unsafe void Callback(BufferProvider buffer)
        {
            if (gain == 0)
                return;

            var ptr = (float*)buffer.Pointer;

            for (n = buffer.FloatLength; n > 0; --n, ++ptr)
                *ptr *= gain;
        }
    }
}
