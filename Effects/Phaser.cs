using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PhaserParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fRate;
        public float fRange;
        public float fFreq;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Phaser; } }
    }

    public sealed class PhaserEffect : Effect<PhaserParameters>
    {
        public PhaserEffect(int Handle) : base(Handle) { }

        public void PhaseShift()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 100;
            if (IsActive) Update();
        }

        public void SlowInvertPhaseShiftWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.6f;
            Parameters.fRate = 0.2f;
            Parameters.fRange = 6;
            Parameters.fFreq = 100;
            if (IsActive) Update();
        }

        public void BasicPhase()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 50;
            if (IsActive) Update();
        }

        public void PhaseWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 40;
            if (IsActive) Update();
        }

        public void Medium()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 100;
            if (IsActive) Update();
        }

        public void Fast()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 400;
            if (IsActive) Update();
        }

        public void InvertWithInvertFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.2f;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 200;
            if (IsActive) Update();
        }

        public void TremoloWah()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 60;
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