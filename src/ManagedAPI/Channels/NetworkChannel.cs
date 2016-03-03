using System;
using ManagedBass.Dynamics;

namespace ManagedBass
{
    /// <summary>
    /// Streams an audio file from a Url
    /// </summary>
    public class NetworkChannel : Channel
    {
        DownloadProcedure proc;
        Action<BufferProvider> call;

        public string Url { get; }
        
        public NetworkChannel(string Url, bool IsDecoder = false, Resolution Resolution = Resolution.Short, Action<BufferProvider> callback = null, int Offset = 0)
        {
            this.Url = Url;

            if (callback != null)
            {
                call = callback;
                proc = new DownloadProcedure(Callback);
            }

            Down_Handler = new SyncProcedure(OnDownloadCompleted);

            var flags = Resolution.ToBassFlag() | BassFlags.Unicode;
            if (IsDecoder) flags |= BassFlags.Decode;

            Handle = Bass.CreateStream(Url, Offset, flags, (callback != null ? proc : null), IntPtr.Zero);

            Bass.ChannelSetSync(Handle, SyncFlags.Downloaded, 0, Down_Handler, IntPtr.Zero);
        }

        void Callback(IntPtr Buffer, int Length, IntPtr User) => call.Invoke(new BufferProvider(Buffer, Length));

        public long DownloadProgress => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Download);

        public bool IsStalled => Bass.ChannelIsActive(Handle) == PlaybackState.Stalled;

        public bool IsConnected => Bass.StreamGetFilePosition(Handle, FileStreamPosition.Connected) == 1;

        #region Download Completed
        SyncProcedure Down_Handler;

        void OnDownloadCompleted(int handle, int channel, int data, IntPtr User) => DownloadComplete?.Invoke();

        public event Action DownloadComplete;
        #endregion
    }
}