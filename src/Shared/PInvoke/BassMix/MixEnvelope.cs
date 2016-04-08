using System;

namespace ManagedBass.Mix
{
    [Flags]
    public enum MixEnvelope
    {
        /// <summary>
        /// Sample rate.
        /// <para>Envelopes are applied on top of the channel's attributes.
        /// The final sample rate is the product of the channel attribute and the envelope.</para>
        /// </summary>
        Frequency = 1,

        /// <summary>
        /// Volume.
        /// <para>Envelopes are applied on top of the channel's attributes. 
        /// The final volume is the product of the channel attribute and the envelope.</para>
        /// </summary>
        Volume = 2,

        /// <summary>
        /// Panning/Balance.
        /// <para>Envelopes are applied on top of the channel's attributes. 
        /// The final panning is a sum of the channel attribute and envelope.</para>
        /// </summary>
        Pan = 3,

        /// <summary>
        /// Loop the envelope (flag).
        /// </summary>
        Loop = 65536
    }
}