using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public class DXCompressorParameters : IEffectParameter
    {
        public float fGain;
        public float fAttack;
        public float fRelease;
        public float fThreshold;
        public float fRatio;
        public float fPredelay;

        public EffectType FXType => EffectType.DXCompressor;
    }

    public sealed class DXCompressorEffect : Effect<DXCompressorParameters>
    {
        public DXCompressorEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public DXCompressorEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Attack
        {
            get { return Parameters.fAttack; }
            set
            {
                Parameters.fAttack = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Release
        {
            get { return Parameters.fRelease; }
            set
            {
                Parameters.fRelease = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Threshold
        {
            get { return Parameters.fThreshold; }
            set
            {
                Parameters.fThreshold = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Ratio
        {
            get { return Parameters.fRatio; }
            set
            {
                Parameters.fRatio = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        public double Predelay
        {
            get { return Parameters.fPredelay; }
            set
            {
                Parameters.fPredelay = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
    }
}