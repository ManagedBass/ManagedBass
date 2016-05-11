using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for Damp Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class DampParameters : IEffectParameter
    {
        public float fTarget = 1f;
        public float fQuiet;
        public float fRate;
        public float fGain;
        public float fDelay;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Damp;
    }
}