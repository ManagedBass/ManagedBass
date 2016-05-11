using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for Rotate Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class RotateParameters : IEffectParameter
    {
        public float fRate;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Rotate;
    }
}