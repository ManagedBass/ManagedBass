using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DistortionParameters : IEffectParameter
    {
        public float fDrive = 0f;
        public float fDryMix = 5f;
        public float fWetMix = 0.1f;
        public float fFeedback = 0f;
        public float fVolume = 0.3f;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Distortion;
    }

    public sealed class DistortionEffect : Effect<DistortionParameters>
    {
        public DistortionEffect(int Handle) : base(Handle) { }

        public void Hard()
        {
            Parameters.fDrive = 1;
            Parameters.fDryMix = 0;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0;
            Parameters.fVolume = 1;

            Update();
        }

        public void VeryHard()
        {
            Parameters.fDrive = 5;
            Parameters.fDryMix = 0;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0.1f;
            Parameters.fVolume = 1;

            Update();
        }

        public void Medium()
        {
            Parameters.fDrive = 0.2f;
            Parameters.fDryMix = 1;
            Parameters.fWetMix = 1;
            Parameters.fFeedback = 0.1f;
            Parameters.fVolume = 1;

            Update();
        }

        public void Soft()
        {
            Parameters.fDrive = 0;
            Parameters.fDryMix = -2.95f;
            Parameters.fWetMix = -0.05f;
            Parameters.fFeedback = -0.18f;
            Parameters.fVolume = 0.25f;

            Update();
        }

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