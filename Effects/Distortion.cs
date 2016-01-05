using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DistortionParameters : IEffectParameter
    {
        public float fDrive;
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fVolume;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Distortion; } }
    }

    public sealed class DistortionEffect : Effect<DistortionParameters>
    {
        public DistortionEffect(int Handle) : base(Handle) { }

        /// <summary>
        /// Distortion Drive (0...5).
        /// </summary>
        public double Drive
        {
            get { return Parameters.fDrive; }
            set
            {
                Parameters.fDrive = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Dry (unaffected) signal mix (-5...+5).
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
        /// Distortion volume (0...+2).
        /// </summary>
        public double Volume
        {
            get { return Parameters.fVolume; }
            set
            {
                Parameters.fVolume = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Wet (affected) signal mix (-5...+5).
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