using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    public class NetworkChannel : Channel
    {
        DownloadProcedure proc;
        Action<BufferProvider> call;

        public string Url { get; set; }
        
        public NetworkChannel(string Url, bool IsDecoder = false, Resolution BufferKind = Resolution.Short, Action<BufferProvider> callback = null)
            : base(IsDecoder, BufferKind)
        {
            this.Url = Url;

            if (callback != null)
            {
                call = callback;
                proc = new DownloadProcedure(Callback);
            }

            Down_Handler = new SyncProcedure(OnDownloadCompleted);

            var flags = BufferKind.ToBassFlag() | BassFlags.Unicode;
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.CreateStream(Url, 0, flags, (callback != null ? proc : null), IntPtr.Zero);

            Bass.ChannelSetSync(Handle, SyncFlags.Downloaded, 0, Down_Handler, IntPtr.Zero);

            if (IsDecoder) Decoder = new BassDecoder(Handle, this);
            else Player = new BassPlayer(Handle, this);
        }

        public void Callback(IntPtr Buffer, int Length, IntPtr User) { call.Invoke(new BufferProvider(Buffer, Length, BufferKind)); }

        public long DownloadProgress { get { return Bass.StreamGetFilePosition(Handle, FileStreamPosition.Download); } }

        public bool IsStalled { get { return Bass.IsChannelActive(Handle) == PlaybackState.Stalled; } }

        public bool IsConnected { get { return Bass.StreamGetFilePosition(Handle, FileStreamPosition.Connected) == 1; } }

        #region Download Completed
        SyncProcedure Down_Handler;

        void OnDownloadCompleted(int handle, int channel, int data, IntPtr User)
        {
            if (DownloadComplete != null) DownloadComplete.Invoke();
        }

        public event Action DownloadComplete;
        #endregion
    }
}