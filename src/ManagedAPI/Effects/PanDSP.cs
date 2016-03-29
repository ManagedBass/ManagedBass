namespace ManagedBass
{
    public class PanDSP : DSP
    {
        public PanDSP(int Channel, int Priority = 0) : base(Channel, Priority) { }

        public PanDSP(MediaPlayer Player, int Priority = 0) : base(Player, Priority) { }

        float _pan;
        public double Pan
        {
            get { return _pan; }
            set 
            {
                _pan = (float)value.Clip(-1, 1);
                
                OnPropertyChanged();
            }
        }

        protected override unsafe void Callback(BufferProvider Buffer)
        {
            if (_pan == 0)
                return;

            var ptr = (float*)Buffer.Pointer;

            for (var i = Buffer.FloatLength; i > 0; i -= 2, ptr += 2)
                if (_pan > 0) ptr[0] *= 1 - _pan;
                else ptr[1] *= 1 + _pan;
        }
    }
}
