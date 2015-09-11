using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EchoParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fDelay;
        public bool bStereo;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Echo; } }
    }

    public sealed class EchoEffect : Effect<EchoParameters>
    {
        public EchoEffect(IEffectAssignable Stream) : base(Stream) { }

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