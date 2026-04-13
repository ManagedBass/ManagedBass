using System.Runtime.InteropServices;

namespace ManagedBass.Fx
{
    /// <summary>
    /// BassFx add-on: Parameters for the volume effect.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This structure defines the parameters for controlling volume level transitions on a channel.
    /// It allows for smooth volume changes over a specified time period using either linear or logarithmic curves.
    /// </para>
    /// <para>
    /// The effect can be applied to specific channels using the <see cref="lChannel"/> field,
    /// and the transition characteristics are controlled by <see cref="fTime"/> and <see cref="lCurve"/>.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class VolumeParameters : IEffectParameter
    {
        /// <summary>
        /// The new volume level... 0 = silent, 1.0 = normal, above 1.0 = amplification. The default value is 1. 
        /// </summary>
        public float fTarget = 1.0f;

        /// <summary>
        /// The current volume level... -1 = leave existing current level when setting parameters. The default value is 1. 
        /// </summary>
        public float fCurrent = 1.0f;

        /// <summary>
        /// The time to take to transition from the current level to the new level, in seconds. The default value is 0. 
        /// </summary>
        public float fTime = 0;

        /// <summary>
        /// The curve to use in the transition... 0 = linear, 1 = logarithmic. The default value is 0. 
        /// </summary>
        public uint lCurve = 0;

        /// <summary>
        /// A <see cref="FXChannelFlags" /> flag to define on which channels to apply the effect. Default: <see cref="FXChannelFlags.All"/>
        /// </summary>
        public FXChannelFlags lChannel = FXChannelFlags.All;

        /// <summary>
        /// Gets the <see cref="EffectType"/>.
        /// </summary>
        public EffectType FXType => EffectType.Volume;
    }
}
