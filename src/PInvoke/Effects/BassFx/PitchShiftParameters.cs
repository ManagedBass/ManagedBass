using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Parameters for PitchShift Effect.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PitchShiftParameters : IEffectParameter
    {
        public float fPitchShift;
        public float fSemitones;
        public long lFFTsize;
        public long lOsamp;
        
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.PitchShift;
    }
}