using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class EchoParameters : IEffectParameter
    {
        public float fDryMix = 0;
        public float fWetMix = 0;
        public float fFeedback = 0;
        public float fDelay = 0;
        public bool bStereo = false;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType { get { return EffectType.Echo; } }
    }

    public sealed class EchoEffect : Effect<EchoParameters>
    {
        public EchoEffect(int Handle) : base(Handle) { }

        public void Small()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fDelay = 0.2f;

            Update();
        }

        public void ManyEchoes()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.7f;
            Parameters.fDelay = 0.5f;

            Update();
        }

        public void ReverseEchoes()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = -0.7f;
            Parameters.fDelay = 0.8f;

            Update();
        }

        public void RoboticVoice()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 0.8f;
            Parameters.fFeedback = 0.5f;
            Parameters.fDelay = 0.1f;

            Update();
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
        /// Delay in seconds (0+...6).
        /// </summary>
        public double Delay
        {
            get { return Parameters.fDelay; }
            set
            {
                Parameters.fDelay = (float)value;
                Update();
            }
        }
    }
}