using System;

namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx add-on: An effect that controls the volume level of a channel with optional transition time and curve.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The volume effect allows smooth transitions between volume levels over a specified time period.
    /// It can be applied to specific channels and supports both linear and logarithmic transition curves.
    /// </para>
    /// <para>
    /// The volume level ranges from 0 (silent) to 1.0 (normal), with values above 1.0 providing amplification.
    /// </para>
    /// </remarks>
    public sealed class Volume : Effect<VolumeParameters>
    {
        /// <summary>
        /// The new volume level... 0 = silent, 1.0 = normal, above 1.0 = amplification. The default value is 1.
        /// </summary>
        public float Target
        {
            get => Parameters.fTarget;
            set
            {
                Parameters.fTarget = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The current volume level... -1 = leave existing current level when setting parameters. The default value is 1.
        /// </summary>
        public float Current
        {
            get => Parameters.fCurrent;
            set
            {
                Parameters.fCurrent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The time to take to transition from the current level to the new level, in seconds. The default value is 0.
        /// </summary>
        public float Time
        {
            get => Parameters.fTime;
            set
            {
                Parameters.fTime = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The curve to use in the transition... False for linear, true for logarithmic. The default value is false.
        /// </summary>
        public bool Curve
        {
            get => Convert.ToBoolean(Parameters.lCurve);
            set
            {
                Parameters.lCurve = (uint)(value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// A <see cref="FXChannelFlags" /> flag to define on which channels to apply the effect. Default: <see cref="FXChannelFlags.All"/>
        /// </summary>
        public FXChannelFlags Channels
        {
            get => Parameters.lChannel;
            set
            {
                Parameters.lChannel = value;

                OnPropertyChanged();
            }
        }
    }
}
