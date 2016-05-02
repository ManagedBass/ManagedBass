using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="ReverbEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class ReverbParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix = 1f;
        public float fRoomSize = 0.5f;
        public float fDamp = 0.5f;
        public float fWidth = 1f;
        public int lMode;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Freeverb;
    }
}