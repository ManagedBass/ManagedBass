using System;

namespace ManagedBass
{
    public class SineWave : Playable
    {
        double freq, amp, rate, sangle;
        int length;
        AudioSample AS;

        public double Frequency
        {
            get { return freq; }
            set
            {
                freq = value;
                Regenerate();
            }
        }

        public double Amplitude
        {
            get { return amp; }
            set
            {
                amp = value;
                Regenerate();
            }
        }

        public double SampleRate
        {
            get { return rate; }
            set
            {
                rate = value;
                Regenerate();
            }
        }

        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                Regenerate();
            }
        }

        public double StartAngle
        {
            get { return sangle; }
            set
            {
                sangle = value;
                Regenerate();
            }
        }

        public SineWave(double Frequency, double Amplitude, double SampleRate, int Length, double StartAngle = 0)
            : base(Resolution.Float)
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
            float[] Buffer = new float[Length];
            CreateSineWave(Buffer, StartAngle);

            if (AS != null) AS.Dispose();
            AS = new AudioSample(4 * Length, Resolution.Float);
            AS.Write(Buffer);
            Handle = AS.Handle;
        }

        void CreateSineWave(float[] Buffer, double StartAngle = 0)
        {
            double AngleStep = Frequency / SampleRate * Math.PI * 2;
            double CurrentAngle = StartAngle;

            for (int i = 0; i < Length; ++i)
            {
                Buffer[i] = (float)Math.Sin(CurrentAngle) * (float)Amplitude;

                CurrentAngle += AngleStep;

                while (CurrentAngle > Math.PI) CurrentAngle -= Math.PI * 2;
            }
        }
    }
}
