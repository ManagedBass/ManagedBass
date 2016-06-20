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

        public CDDrive Drive => CDDrive.GetByIndex(BassCd.StreamGetTrack(Handle).HiWord());

        public int TrackNumber
        {
            get { return BassCd.StreamGetTrack(Handle).LoWord(); }
            set { BassCd.StreamSetTrack(Handle, value); }
        }
    }
}
#endif