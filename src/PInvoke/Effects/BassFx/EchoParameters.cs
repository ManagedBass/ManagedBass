using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for Echo Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class EchoParameters : IEffectParameter
    {
        public float fDryMix;
        public float fWetMix;
        public float fFeedback;
        public float fDelay;
        public int bStereo;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Echo;
    }
}