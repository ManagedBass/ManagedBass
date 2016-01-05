using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AutoWahParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fRate;
        public float fRange;
        public float fFreq;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.AutoWah; } }
    }

    public sealed class AutoWahEffect : Effect<AutoWahParameters>
    {
        public AutoWahEffect(int Handle) : base(Handle) { }

        public void Slow()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 2;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 50;
            if (IsActive) Update();
        }

        public void Fast()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 5;
            Parameters.fRange = 5.3f;
            Parameters.fFreq = 50;
            if (IsActive) Update();
        }

        public void HiFast()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 5;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 500;
            if (IsActive) Update();
        }

        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;
                Update();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;
                Update();
            }
        }

        public double Frequency
        {
            get { return Parameters.fFreq; }
            set
            {
                Parameters.fFreq = (float)value;
                Update();
            }
        }

        public double Range
        {
            get { return Parameters.fRange; }
            set
            {
                Parameters.fRange = (float)value;
                Update();
            }
        }

        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;
                Update();
            }
        }

        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;
                Update();
            }
        }
    }
}