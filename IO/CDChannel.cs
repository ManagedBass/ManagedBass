using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Stream audio from a CD file or track
    /// </summary>
    public class CDChannel : Channel
    {
        public CDChannel(string FileName, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassCd.CreateStream(FileName, FlagGen(IsDecoder, Resolution));
        }

        public CDChannel(CDDrive Drive, int Track, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
            : this(Drive.DriveIndex, Track, IsDecoder, Resolution) { }

        public CDChannel(int Drive, int Track, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
        {
            Handle = BassCd.CreateStream(Drive, Track, FlagGen(IsDecoder, Resolution));
        }
    }
}