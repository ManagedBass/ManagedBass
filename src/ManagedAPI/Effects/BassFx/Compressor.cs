using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Used with <see cref="CompressorEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class CompressorParameters : IEffectParameter
    {
        public float fGain = 5f;
        public float fThreshold = -15f;
        public float fRatio = 3f;
        public float fAttack = 20f;
        public float fRelease = 200f;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Compressor;
    }

    public sealed class CompressorEffect : Effect<CompressorParameters>
    {
        public CompressorEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        /// <summary>
        /// Time in ms before compression reaches its full value, in the range from 0.01 to 500. Default = 20.
        /// </summary>
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

        /// <summary>
        /// Time (speed) in ms at which compression is stopped after Input drops below <see cref="Threshold"/>, in the range from 50 to 3000. Default = 200.
        /// </summary>
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

        /// <summary>
        /// Point in dB at which compression begins, in decibels, in the range from -60 to 0. Default = -15.
        /// </summary>
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

        /// <summary>
        /// Output gain in dB of signal after compression, in the range from -60 to 60. Default = 5.
        /// </summary>
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

        /// <summary>
        /// Compression ratio, in the range from 1 to 100. Default = 3.
        /// </summary>
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
    }
}