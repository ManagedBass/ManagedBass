using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Used with <see cref="DampEffect"/>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DampParameters : IEffectParameter
    {
        public float fTarget = 1f;
        public float fQuiet = 0;
        public float fRate = 0;
        public float fGain = 0;
        public float fDelay = 0;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Damp;
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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
                Update();
            }
        }
    }
}