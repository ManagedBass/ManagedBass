using ManagedBass.Dynamics;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class PhaserParameters : IEffectParameter
    {
        public float fDryMix = 0.999f;
        public float fWetMix = 0.999f;
        public float fFeedback = 0;
        public float fRate = 1;
        public float fRange = 4.3f;
        public float fFreq = 50;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Phaser;
    }

    public sealed class PhaserEffect : Effect<PhaserParameters>
    {
        public PhaserEffect(int Handle) : base(Handle) { }

        #region Presets
        public void PhaseShift()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        public void SlowInvertPhaseShiftWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.6f;
            Parameters.fRate = 0.2f;
            Parameters.fRange = 6;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        public void BasicPhase()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 50;

            OnPropertyChanged("");
            Update();
        }

        public void PhaseWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 40;

            OnPropertyChanged("");
            Update();
        }

        public void Medium()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        public void Fast()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 400;

            OnPropertyChanged("");
            Update();
        }

        public void InvertWithInvertFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.2f;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 200;

            OnPropertyChanged("");
            Update();
        }

        public void TremoloWah()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 60;

            OnPropertyChanged("");
            Update();
        }
        #endregion

        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Frequency
        {
            get { return Parameters.fFreq; }
            set
            {
                Parameters.fFreq = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Range
        {
            get { return Parameters.fRange; }
            set
            {
                Parameters.fRange = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}