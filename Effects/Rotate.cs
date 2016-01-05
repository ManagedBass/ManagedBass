using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RotateParameters : IEffectParameter
    {
        public float fRate;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Rotate; } }
    }

    public sealed class RotateEffect : Effect<RotateParameters>
    {
        public RotateEffect(int Handle) : base(Handle) { }

        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;
                Update();
            }
        }
    }
}