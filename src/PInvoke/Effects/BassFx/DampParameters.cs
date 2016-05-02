using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="DampEffect"/>
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