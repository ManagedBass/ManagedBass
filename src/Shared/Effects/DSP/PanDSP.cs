using System;

namespace ManagedBass
{
    /// <summary>
    /// Pan DSP.
    /// Currently implemented only for Floating-point streams.
    /// </summary>
    public class PanDSP : DSP
    {
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

        protected override unsafe void Callback(IntPtr Buffer, int Length)
        {
            if (_pan == 0)
                return;

            var ptr = (float*)Buffer;

            for (var i = Length / 4; i > 0; i -= 2, ptr += 2)
                if (_pan > 0)
                    ptr[0] *= 1 - _pan;
                else ptr[1] *= 1 + _pan;
        }
    }
}
