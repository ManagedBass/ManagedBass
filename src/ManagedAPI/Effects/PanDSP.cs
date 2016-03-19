namespace ManagedBass.Effects
{
    public class PanDSP : DSP
    {
        int n;

        public PanDSP(int Channel, int Priority = 0) : base(Channel, Priority) { }

        public PanDSP(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        float pan = 0;
        public double Pan
        {
            get { return pan; }
            set 
            {
                pan = (float)value.Clip(-1, 1);
                
                OnPropertyChanged();
            }
        }

        protected override unsafe void Callback(BufferProvider buffer)
        {
            if (pan == 0)
                return;

            var ptr = (float*)buffer.Pointer;

            for (n = buffer.FloatLength; n > 0; n -= 2, ptr += 2)
                if (pan > 0) ptr[0] *= (1 - pan);
                else ptr[1] *= (1 + pan);
        }
    }
}
