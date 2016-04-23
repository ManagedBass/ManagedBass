using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// Used with <see cref="AutoWahEffect"/>.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AutoWahParameters : IEffectParameter
    {
        public float fDryMix = 0.5f;
        public float fWetMix = 1.5f;
        public float fFeedback = 0.5f;
        public float fRate = 2;
        public float fRange = 4.3f;
        public float fFreq = 50;
        public FXChannelFlags lChannel = FXChannelFlags.All;

        public EffectType FXType => EffectType.AutoWah;
    }

    /// <summary>
    /// Used with <see cref="Bass.ChannelSetFX" />, <see cref="Bass.FXSetParameters" /> and <see cref="Bass.FXGetParameters" /> to retrieve and set the parameters of the DSP effect Auto wah.
    /// </summary>
    /// <remarks>
    /// <para>The effect implements the auto-wah by using 4-stage phaser effect which moves a peak in the frequency response up and down the frequency spectrum by amplitude of Input signal.</para>
    /// <para>The <see cref="DryMix"/> is the volume of Input signal and the <see cref="WetMix"/> is the volume of delayed signal.
    /// The <see cref="Feedback"/> sets feedback of auto wah (phaser).
    /// The <see cref="Rate"/> and <see cref="Range"/> control how fast and far the frequency notches move.
    /// The <see cref="Rate"/> is the rate of sweep in cycles per second, <see cref="Range"/> is the width of sweep in octaves.
    /// And the the <see cref="Frequency"/> is the base frequency of sweep.</para>
    /// </remarks>
    public sealed class AutoWahEffect : Effect<AutoWahParameters>
    {
        public AutoWahEffect(int Handle, int Priority = 0) : base(Handle, Priority) { }

        public AutoWahEffect(MediaPlayer player, int Priority = 0) : base(player, Priority) { }

        #region Presets
        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Slow()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 2;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 50;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void Fast()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 5;
            Parameters.fRange = 5.3f;
            Parameters.fFreq = 50;

            OnPropertyChanged("");
            Update();
        }

        /// <summary>
        /// Set up a Preset.
        /// </summary>
        public void HiFast()
        {
            Parameters.fDryMix = 0.5f;
            Parameters.fWetMix = 1.5f;
            Parameters.fFeedback = 0.5f;
            Parameters.fRate = 5;
            Parameters.fRange = 4.3f;
            Parameters.fFreq = 500;

            OnPropertyChanged("");
            Update();
        }
        #endregion

        /// <summary>
        /// Dry (unaffected) signal mix (-2...+2). Default = 0.5.
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
        /// Feedback (-1...+1). Default = 0.5.
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
        /// Base frequency of sweep range (0&lt;...1000). Default = 50.
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
        /// Sweep range in octaves (0&lt;...&lt;10). Default = 4.3.
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
        /// Rate of sweep in cycles per second (0&lt;...&lt;10). Default = 2.
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
        /// Wet (affected) signal mix (-2...+2). Default = 1.5.
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