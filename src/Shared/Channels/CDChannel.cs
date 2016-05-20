#if WINDOWS || LINUX
namespace ManagedBass.Cd
{
    /// <summary>
    /// Stream audio from a CD file or track
    /// </summary>
    public sealed class CDChannel : Channel
    {
        public CDChannel(string FileName, BassFlags Flags = BassFlags.Default)
        {
            Handle = BassCd.CreateStream(FileName, Flags);
        }

        public CDChannel(CDDrive Drive, int Track, BassFlags Flags = BassFlags.Default)
            : this(Drive.Index, Track, Flags) { }

        public CDChannel(int Drive, int Track, BassFlags Flags = BassFlags.Default)
        {
            Handle = BassCd.CreateStream(Drive, Track, Flags);
        }
    }
}
#endif