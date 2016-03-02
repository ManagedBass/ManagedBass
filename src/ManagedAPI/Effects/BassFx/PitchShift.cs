using System.Runtime.InteropServices;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
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

    public sealed class PitchShiftEffect : Effect<PitchShiftParameters>
    {
        public PitchShiftEffect(int Handle) : base(Handle) { }

        public double PitchShift
        {
            get { return Parameters.fPitchShift; }
            set
            {
                Parameters.fPitchShift = (float)value;
                Update();
            }
        }

        public double Semitones
        {
            get { return Parameters.fSemitones; }
            set
            {
                Parameters.fSemitones = (float)value;
                Update();
            }
        }

        public long FFTFrameSize
        {
            get { return Parameters.lFFTsize; }
            set
            {
                Parameters.lFFTsize = value;
                Update();
            }
        }

        public long OversamplingFactor
        {
            get { return Parameters.lOsamp; }
            set
            {
                Parameters.lOsamp = value;
                Update();
            }
        }
    }
}