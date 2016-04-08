namespace ManagedBass
{
    public class GainDSP : DSP
    {
        public GainDSP(int Channel, int Priority = 0) : base(Channel, Priority) { }

        public GainDSP(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        float _gain = 1;
        public double Gain
        {
            get { return _gain; }
            set
            {
                _gain = (float)value.Clip(0, 1024);

                OnPropertyChanged();
            }
        }

        protected override unsafe void Callback(BufferProvider Buffer)
        {
            if (_gain == 0)
                return;

            var ptr = (float*)Buffer.Pointer;

            for (var i = Buffer.FloatLength; i > 0; --i, ++ptr)
                *ptr *= _gain;
        }
    }
}
