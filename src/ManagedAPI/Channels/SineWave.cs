using System;

namespace ManagedBass
{
    /// <summary>
    /// Plays a Sine Wave
    /// </summary>
    public class SineWave : Channel
    {
        double _freq, _amp, _rate, _sangle;
        int _length, _sample;

        public new double Frequency
        {
            get { return _freq; }
            set
            {
                _freq = value;
                Regenerate();
            }
        }

        public double Amplitude
        {
            get { return _amp; }
            set
            {
                _amp = value;
                Regenerate();
            }
        }

        public double SampleRate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                Regenerate();
            }
        }

        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                Regenerate();
            }
        }

        public double StartAngle
        {
            get { return _sangle; }
            set
            {
                _sangle = value;
                Regenerate();
            }
        }

        public SineWave(double Frequency, double Amplitude, double SampleRate, int Length, double StartAngle = 0)
        {
            this.Frequency = Frequency;
            this.Amplitude = Amplitude;
            this.SampleRate = SampleRate;
            this.Length = Length;
            this.StartAngle = StartAngle;

            Regenerate();
        }

        void Regenerate()
        {
            var buffer = new float[Length];
            CreateSineWave(buffer);

            if (_sample != 0)
                Bass.SampleFree(_sample);

            _sample = Bass.CreateSample(4 * Length, 44100, 2, 1, BassFlags.Float);
            Bass.SampleSetData(_sample, buffer);
            Handle = Bass.SampleGetChannel(_sample);
        }

        void CreateSineWave(float[] Buffer)
        {
            var angleStep = Frequency / SampleRate * Math.PI * 2;
            var currentAngle = StartAngle;

            for (var i = 0; i < Length; ++i)
            {
                Buffer[i] = (float)Math.Sin(currentAngle) * (float)Amplitude;

                currentAngle += angleStep;

                while (currentAngle > Math.PI) currentAngle -= Math.PI * 2;
            }
        }
    }
}
