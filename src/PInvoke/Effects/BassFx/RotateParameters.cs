using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="RotateEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class RotateParameters : IEffectParameter
    {
        public float fRate;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Rotate;
    }
}