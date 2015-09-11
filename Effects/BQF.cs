using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BQFParameters : IEffectParameter
    {
        public BQFType lFilter;
        public float fCenter;
        public float fGain;
        public float fBandwidth;
        public float fQ;
        public float fS;
        public FXChannelFlags lChannel;

        public EffectType FXType { get { return EffectType.BQF; } }
    }

    /// <summary>
    /// Bi-Quad Filter Effect Manipulator.
    /// </summary>
    public abstract class BQFEffect : Effect<BQFParameters>
    {
        public BQFEffect(IEffectAssignable Stream, BQFType BQFType) : base(Stream) { Parameters.lFilter = BQFType; }

        /// <summary>
        /// Gain in dB (-15...0...+15). Default 0dB (used only for PEAKINGEQ and Shelving filters).
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
        /// Bandwidth in octaves (0.1...4...n), Q is not in use (fBandwidth has priority over fQ). Default = 1 (0=not in use).
        /// The bandwidth in octaves (between -3 dB frequencies for for BANDPASS and NOTCH or between midpoint (dBgain/2) gain frequencies for PEAKINGEQ).
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
        /// Cut-off frequency (Center in PEAKINGEQ and Shelving filters) in Hz (1...info.freq/2). Default = 200Hz.
        /// </summary>
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
        /// EE kinda definition of Q (0.1...1...n), if bandwidth is not in use. Default = 0.0 (0=not in use).
        /// </summary>
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
        /// A shelf slope parameter (linear, used only with Shelving filters) (0...1...n). Default = 0.0.
        /// When fS=1, the shelf slope is as steep as you can get it and remain monotonically increasing or decreasing gain with frequency.
        /// </summary>
        public double S
        {
            get { return Parameters.fS; }
            set
            {
                Parameters.fS = (float)value;
                Update();
            }
        }
    }

    /// <summary>
    /// All Pass Filter Effect Manipulator.
    /// </summary>
    public sealed class APFEffect : BQFEffect { public APFEffect(IEffectAssignable Stream) : base(Stream, BQFType.AllPass) { } }

    /// <summary>
    /// Low Pass Filter Effect Manipulator.
    /// </summary>
    public sealed class LPFEffect : BQFEffect { public LPFEffect(IEffectAssignable Stream) : base(Stream, BQFType.LowPass) { } }
}