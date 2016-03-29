using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="PeakEQ"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PeakEQParameters : IEffectParameter
    {
        public int lBand;
        public float fBandwidth = 1f;
        public float fQ;
        public float fCenter = 1000f;
        public float fGain;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.PeakEQ;
    }

    [System.Obsolete("Use PeakEQ instead.")]
    public sealed class PeakEQEffect : Effect<PeakEQParameters>
    {
        public PeakEQEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public PeakEQEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        /// <summary>
        /// Bandwidth in octaves (0.1...4...n), Q is not in use (Bandwidth has priority over Q). Default = 1 (0=not in use).
        /// In most cases users should use the minimum of 0.5 octave.
        /// The bandwidth in octaves (between -3 dB frequencies for BPF and notch or between midpoint (dBgain/2) gain frequencies for peaking EQ).
        /// </summary>
        public double Bandwidth
        {
            get { return Parameters.fBandwidth; }
            set
            {
                Parameters.fBandwidth = (float)value;

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
                Update();
            }
        }
    }
    
    public sealed class PeakEQ
    {
        PeakEQParameters parameters;
        int Handle;
        int Channel;
        GCHandle gch;

        public PeakEQ(int Channel, double Q = 0, double Bandwith = 2.5)
        {
            this.Channel = Channel;

            Handle = Bass.ChannelSetFX(Channel, EffectType.PeakEQ, 0);

            parameters = new PeakEQParameters
            {
                lBand = -1,
                fQ = (float)Q,
                fBandwidth = (float)Bandwith
            };

            gch = GCHandle.Alloc(parameters, GCHandleType.Pinned);
        }

        public int AddBand(double CenterFrequency)
        {
            ++parameters.lBand;

            parameters.fCenter = (float)CenterFrequency;
            parameters.fGain = 0;

            Bass.FXSetParameters(Handle, gch.AddrOfPinnedObject());

            return parameters.lBand;
        }

        public void UpdateBand(int Band, double Gain)
        {
            parameters.lBand = Band;

            Bass.FXGetParameters(Handle, gch.AddrOfPinnedObject());

            parameters.fGain = (float)Gain;

            Bass.FXSetParameters(Handle, gch.AddrOfPinnedObject());
        }
    }
}