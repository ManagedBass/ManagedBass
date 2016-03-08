using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class RotateParameters : IEffectParameter
    {
        public float fRate = 0;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Rotate;
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

                OnPropertyChanged();
                Update();
            }
        }
    }
}