using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="PhaserEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PhaserParameters : IEffectParameter
    {
        public float fDryMix = 0.999f;
        public float fWetMix = 0.999f;
        public float fFeedback;
        public float fRate = 1;
        public float fRange = 4.3f;
        public float fFreq = 50;

        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.Phaser;
    }

    public sealed class PhaserEffect : Effect<PhaserParameters>
    {
        public PhaserEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public PhaserEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        #region Presets
        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void PhaseShift()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void SlowInvertPhaseShiftWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.6f;
            Parameters.fRate = 0.2f;
            Parameters.fRange = 6;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void BasicPhase()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 50;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void PhaseWithFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 40;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Medium()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 100;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Fast()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 400;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void InvertWithInvertFeedback()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = -0.999f;
            Parameters.fFeedback = -0.2f;
            Parameters.fRate = 1;
            Parameters.fRange = 7;
            Parameters.fFreq = 200;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void TremoloWah()
        {
            Parameters.fDryMix = 0.999f;
            Parameters.fWetMix = 0.999f;
            Parameters.fFeedback = 0.6f;
            Parameters.fRate = 1;
            Parameters.fRange = 4;
            Parameters.fFreq = 60;

            OnPropertyChanged("");
            Update();
        }
        #endregion

        /// <summary>
		/// Dry (unaffected) signal mix (-2...+2). Default = 0.
		/// </summary>
        public double DryMix
        {
            get { return Parameters.fDryMix; }
            set
            {
                Parameters.fDryMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Feedback (-1...+1). Default = 0.
		/// </summary>
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
        
		/// <summary>
		/// Base frequency of sweep range (0&lt;...1000). Default = 0.
		/// </summary>
        public double Frequency
        {
            get { return Parameters.fFreq; }
            set
            {
                Parameters.fFreq = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Sweep range inoctaves (0&lt;...&lt;10). Default = 0.
		/// </summary>
        public double Range
        {
            get { return Parameters.fRange; }
            set
            {
                Parameters.fRange = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Rate of sweep in cycles per second (0&lt;...&lt;10). Default = 0.
		/// </summary>
        public double Rate
        {
            get { return Parameters.fRate; }
            set
            {
                Parameters.fRate = (float)value;

                OnPropertyChanged();
                Update();
            }
        }
        
		/// <summary>
		/// Wet (affected) signal mix (-2...+2). Default = 0.
		/// </summary>
        public double WetMix
        {
            get { return Parameters.fWetMix; }
            set
            {
                Parameters.fWetMix = (float)value;

                OnPropertyChanged();
                Update();
            }
        }

        /// <summary>
        /// A <see cref="FXChannelFlags" /> flag to define on which channels to apply the effect. Default: <see cref="FXChannelFlags.All"/>
        /// </summary>
        public FXChannelFlags Channels
        {
            get { return Parameters.lChannel; }
            set
            {
                Parameters.lChannel = value;

                OnPropertyChanged();
            }
        }
    }
}