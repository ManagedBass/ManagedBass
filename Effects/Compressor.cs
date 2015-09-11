using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CompressorParameters : IEffectParameter
    {
        public float fGain;
        public float fThreshold;
        public float fRatio;
        public float fAttack;
        public float fRelease;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.Compressor; } }
    }

    public sealed class CompressorEffect : Effect<CompressorParameters>
    {
        public CompressorEffect(IEffectAssignable Stream) : base(Stream) { }

        /// <summary>
        /// Time in ms before compression reaches its full value, in the range from 0.01 to 500.
        /// </summary>
        public double Attack
        {
            get { return Parameters.fAttack; }
            set
            {
                Parameters.fAttack = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Time (speed) in ms at which compression is stopped after input drops below fThreshold,
        /// in the range from 50 to 3000.
        /// </summary>
        public double Release
        {
            get { return Parameters.fRelease; }
            set
            {
                Parameters.fRelease = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Point in dB at which compression begins, in decibels, in the range from -60 to 0.
        /// </summary>
        public double Threshold
        {
            get { return Parameters.fThreshold; }
            set
            {
                Parameters.fThreshold = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Output gain in dB of signal after compression, in the range from -60 to 60.
        /// </summary>
        public double Gain
        {
            get { return Parameters.fGain; }
            set
            {
                Parameters.fGain = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Compression ratio, in the range from 1 to 100.
        /// </summary>
        public double Ratio
        {
            get { return Parameters.fRatio; }
            set
            {
                Parameters.fRatio = (float)value;
                Update();
            }
        }
    }
}