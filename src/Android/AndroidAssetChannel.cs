using Android.Content.Res;

namespace ManagedBass
{
    /// <summary>
    /// Streams an Asset file using <see cref="FileProcedures"/>.
    /// </summary>
    public sealed class AndroidAssetChannel : StreamChannel
    {
        /// <summary>
        /// Creates a new instance of <see cref="AndroidAssetChannel"/>.
        /// </summary>
        public AndroidAssetChannel(string FileName, AssetManager AssetManager, bool IsDecoder = false, Resolution Resolution = Resolution.Short)
            : base(AssetManager.Open(FileName, Access.Random), IsDecoder, Resolution) { }
    }
}