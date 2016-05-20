namespace ManagedBass
{
    public struct SyncParameters
    {
        public SyncFlags Type;
        public long Parameter;
        public SyncHandler Callback;
        internal int SyncHandle;
    }
}