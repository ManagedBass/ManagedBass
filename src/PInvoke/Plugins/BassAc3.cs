namespace ManagedBass
{
    public static partial class BassAc3
    {   
        /// <summary>
        /// Enable Dynamic Range Compression (default is false).
        /// </summary>
        public static bool DRC
        {
            get { return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); }
            set { Bass.Configure(Configuration.AC3DynamicRangeCompression, value); }
        }
    }
}
