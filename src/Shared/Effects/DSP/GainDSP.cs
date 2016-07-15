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

        /// <summary>
        /// Gain value (0 ... 1 (no gain) ... 1024). Values outside the range are automatically clipped.
        /// </summary>
        public double Gain
        {
            get { return _gain; }
            set
            {
                _gain = (float)value.Clip(0, 1024);

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// DSP Callback.
        /// </summary>
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
