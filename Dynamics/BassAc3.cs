namespace ManagedBass.Dynamics
{
    public static class BassAc3
    {
        const string DllName = "bassac3.dll";

        static BassAc3() { BassManager.Load(DllName); }

        /// <summary>
        /// Dynamic range compression option
        /// dynrng (bool): If true dynamic range compression is enbaled (default is false).
        /// </summary>
        public static bool DRC
        {
            get { return Bass.GetConfigBool(Configuration.AC3DynamicRangeCompression); }
            set { Bass.Configure(Configuration.AC3DynamicRangeCompression, value); }
        }
    }
}