namespace ManagedBass.Dsd
{
    public static partial class BassDsd
    {
        /// <summary>
        /// The default sample rate when converting to PCM.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This setting determines what sample rate is used by default when converting to PCM.
        /// The rate actually used may be different if the specified rate is not valid for a particular DSD rate, in which case it will be rounded up (or down if there are none higher) to the nearest valid rate;
        /// the valid rates are 1/8, 1/16, 1/32, etc. of the DSD rate down to a minimum of 44100 Hz.
        /// </para>
        /// <para>
        /// The default setting is 88200 Hz.
        /// Changes only affect subsequently created streams, not any that already exist.
        /// </para>
        /// </remarks>
        public static int DefaultFrequency
        {
            get { return Bass.GetConfig(Configuration.DSDFrequency); }
            set { Bass.Configure(Configuration.DSDFrequency, value); }
        }

        /// <summary>
        /// The default gain applied when converting to PCM.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This setting determines what gain is applied by default when converting to PCM.
        /// Changes only affect subsequently created streams, not any that already exist.
        /// An existing stream's gain can be changed via the <see cref="ChannelAttribute.DSDGain" /> attribute.
        /// </para>
        /// <para>The default setting is 6dB.</para>
        /// </remarks>
        public static int DefaultGain
        {
            get { return Bass.GetConfig(Configuration.DSDGain); }
            set { Bass.Configure(Configuration.DSDGain, value); }
        }
    }
}