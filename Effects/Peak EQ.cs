using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PeakEQParameters : IEffectParameter
    {
        public int lBand;
        public float fBandwidth;
        public float fQ;
        public float fCenter;
        public float fGain;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.PeakEQ; } }
    }

    public sealed class PeakEQEffect : Effect<PeakEQParameters>
    {
        public PeakEQEffect(IEffectAssignable Stream) : base(Stream) { }

        /// <summary>
        /// Bandwidth in octaves (0.1...4...n), Q is not in use (fBandwidth has priority over fQ). Default = 1 (0=not in use).
        /// In most cases users should use the minimum of 0.5 octave.
        /// The bandwidth in octaves (between -3 dB frequencies for BPF and notch or between midpoint (dBgain/2) gain frequencies for peaking EQ).
        /// </summary>
        public double Bandwidth
        {
            get { return Parameters.fBandwidth; }
            set
            {
                Parameters.fBandwidth = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Center frequency in Hz (1Hz...nHz). Default = 1000 (max. is 1/2 of the samplerate).
        /// /// </summary>
        public double Center
        {
            get { return Parameters.fCenter; }
            set
            {
                Parameters.fCenter = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Gain in dB (-15...0...+15). Default 0dB.
        /// /// </summary>
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
        /// EE kinda definition of Q (0.1...1...n), if bandwidth is not in use. Default = 0.0 (0=not in use).
        /// /// </summary>
        public double Q
        {
            get { return Parameters.fQ; }
            set
            {
                Parameters.fQ = (float)value;
                Update();
            }
        }

        /// <summary>
        /// Number of bands (0...n), more bands means more memory and cpu usage. Default = 0.
        /// /// </summary>
        public int Band
        {
            get { return Parameters.lBand; }
            set
            {
                Parameters.lBand = value;
                Update();
            }
        }
    }
}