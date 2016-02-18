using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXChorusParameters : IEffectParameter
    {
        public float fWetDryMix;
        public float fDepth;
        public float fFeedback;
        public float fFrequency;
        public DXWaveform lWaveform;
        public float fDelay;
        public DXPhase lPhase;

        public EffectType FXType { get { return EffectType.DXChorus; } }
    }

    public sealed class DXChorusEffect : Effect<DXChorusParameters>
    {
        public DXChorusEffect(int Handle) : base(Handle) { }

        public DXWaveform Waveform
        {
            get { return Parameters.lWaveform; }
            set
            {
                Parameters.lWaveform = value;
                Update();
            }
        }

        public double WetDryMix
        {
            get { return Parameters.fWetDryMix; }
            set
            {
                Parameters.fWetDryMix = (float)value;
                Update();
            }
        }

        public double Depth
        {
            get { return Parameters.fDepth; }
            set
            {
                Parameters.fDepth = (float)value;
                Update();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;
                Update();
            }
        }

        public double Frequency
        {
            get { return Parameters.fFrequency; }
            set
            {
                Parameters.fFrequency = (float)value;
                Update();
            }
        }

        public double Delay
        {
            get { return Parameters.fDelay; }
            set
            {
                Parameters.fDelay = (float)value;
                Update();
            }
        }

        public DXPhase Phase
        {
            get { return Parameters.lPhase; }
            set
            {
                Parameters.lPhase = value;
                Update();
            }
        }
    }
}