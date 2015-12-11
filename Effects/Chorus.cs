using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ChorusParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fMinSweep;
        public float fMaxSweep;
        public float fRate;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Chorus; } }
    }

    public sealed class ChorusEffect : Effect<ChorusParameters>
    {
        public ChorusEffect(IEffectAssignable Stream) : base(Stream) { }

        public void Exaggerated()
        {
            Parameters.fDryMix = 0.7f;
            Parameters.fWetMix = 0.25f;
            Parameters.fFeedback = 0.5f;
            Parameters.fMinSweep = 1;
            Parameters.fMaxSweep = 200;
            Parameters.fRate = 50;

            if (IsActive) Update();
        }

        public void ManyVoices()
        {
            Parameters.fDryMix = 0.9f;
            Parameters.fWetMix = 0.35f;
            Parameters.fFeedback = 0.5f;
            Parameters.fMinSweep = 1;
            Parameters.fMaxSweep = 400;
            Parameters.fRate = 200;

            if (IsActive) Update();
        }

        /// <summary>
        /// Dry (unaffected) signal mix (-2...+2).
        /// </summary>
        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Feedback (-1...+1).
        /// </summary>
        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Maximum delay in ms (0&lt;...6000).
        /// </summary>
        public double MaxSweep
        {
            get { return Parameters.fMaxSweep; }
            set
            {
                Parameters.fMaxSweep = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Minimum delay in ms (0&lt;...6000).
        /// </summary>
        public double MinSweep
        {
            get { return Parameters.fMinSweep; }
            set
            {
                Parameters.fMinSweep = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Rate in ms/s (0&lt;...1000).
        /// </summary>
        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Wet (affected) signal mix (-2...+2).
        /// </summary>
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