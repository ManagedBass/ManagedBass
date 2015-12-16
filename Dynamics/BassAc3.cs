namespace ManagedBass.Dynamics
{
    public static class BassAc3
    {
        const string DllName = "bassac3.dll";

        public static void Load(string folder = null) { Extensions.Load(DllName, folder); }

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