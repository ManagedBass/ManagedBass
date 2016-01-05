using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DampParameters : IEffectParameter
    {
        public float fTarget;
        public float fQuiet;
        public float fRate;
        public float fGain;
        public float fDelay;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Damp; } }
    }

    public sealed class DampEffect : Effect<DampParameters>
    {
        public DampEffect(int Handle) : base(Handle) { }

        /// <summary>
        /// Amplification level (0...1...n, linear). 
        /// </summary>
        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Amplification adjustment rate (0...1, linear).
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
        /// Target volume level (0&lt;...1, linear).
        /// </summary>
        public double Target
        {
            get { return Parameters.fTarget; }
            set
            {
                Parameters.fTarget = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Quiet volume level (0...1, linear). 
        /// </summary>
        public double Quiet
        {
            get { return Parameters.fQuiet; }
            set
            {
                Parameters.fQuiet = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Delay in seconds before increasing level (0...n, linear).
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