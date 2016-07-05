using System;

namespace ManagedBass
{
    /// <summary>
    /// Gain DSP.
    /// Currently implemented only for Floating-point streams.
    /// </summary>
    public class GainDSP : DSP
    {
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

        protected override unsafe void Callback(IntPtr Buffer, int Length)
        {
            if (_gain == 1)
                return;

            var ptr = (float*)Buffer;

            for (var i = Length / 4; i > 0; --i, ++ptr)
                *ptr *= _gain;
        }
    }
}
