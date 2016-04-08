using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXChorusParameters : IEffectParameter
    {
        public float fWetDryMix = 50f;
        public float fDepth = 25f;
        public float fFeedback;
        public float fFrequency;
        public DXWaveform lWaveform = DXWaveform.Triangle;
        public float fDelay;
        public DXPhase lPhase = DXPhase.Zero;

        public EffectType FXType => EffectType.DXChorus;
    }

    public sealed class DXChorusEffect : Effect<DXChorusParameters>
    {
        public DXChorusEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXChorusEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public DXWaveform Waveform
        {
            get { return Parameters.lWaveform; }
            set
            {
                Parameters.lWaveform = value;

                OnPropertyChanged();
                Update();
            }
        }

        public double WetDryMix
        {
            get { return Parameters.fWetDryMix; }
            set
            {
                Parameters.fWetDryMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Depth
        {
            get { return Parameters.fDepth; }
            set
            {
                Parameters.fDepth = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Feedback
        {
            get { return Parameters.fFeedback; }
            set
            {
                Parameters.fFeedback = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Frequency
        {
            get { return Parameters.fFrequency; }
            set
            {
                Parameters.fFrequency = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Delay
        {
            get { return Parameters.fDelay; }
            set
            {
                Parameters.fDelay = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public DXPhase Phase
        {
            get { return Parameters.lPhase; }
            set
            {
                Parameters.lPhase = value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}