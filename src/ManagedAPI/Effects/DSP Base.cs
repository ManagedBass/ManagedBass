using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// Base class for DSPs.
    /// Sets <see cref="Bass.FloatingPointDSP"/> to <see langword="true"/> so you need to implement the DSP only for 32-bit float audio.
    /// </summary>
    public abstract class DSP : IDisposable, INotifyPropertyChanged
    {
        public int Channel { get; private set; }

        public int Handle { get; private set; }

        int _priority;
        public int Priority
        {
            get { return _priority; }
            set
            {
                if (Bass.FXSetPriority(Handle, value))
                    _priority = value;
            }
        }

        bool _isAssigned;
        public bool IsAssigned
        {
            get { return _isAssigned; }
            private set
            {
                _isAssigned = value;

                OnPropertyChanged();
            }
        }

        bool _bypass;
        public bool Bypass 
        {
            get { return _bypass; }
            set
            {
                _bypass = value;

                OnPropertyChanged();
            }
        }

        public Resolution Resolution { get; private set; }

        readonly DSPProcedure _dspProc;
        readonly SyncProcedure _freeproc;

        static DSP() { Bass.FloatingPointDSP = true; }

        protected DSP(int Channel, int Priority)
        {
            this.Channel = Channel;

            _dspProc = OnDsp;

            _priority = Priority;

            Handle = Bass.ChannelSetDSP(Channel, _dspProc, Priority: _priority);

            Resolution = Bass.ChannelGetInfo(Channel).Resolution;

            _freeproc = (a, b, c, d) => Dispose();

            Bass.ChannelSetSync(Channel, SyncFlags.Free, 0, _freeproc);

            if (Handle != 0) 
                IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        protected DSP(MediaPlayer player, int Priority)
        {
            _dspProc = OnDsp;

            _priority = Priority;

            Reassign(player.Handle);

            player.MediaLoaded += Reassign;
        }

        void Reassign(int h)
        {
            Channel = h;

            Handle = Bass.ChannelSetDSP(Channel, _dspProc, Priority: _priority);

            if (Channel != 0)
                Resolution = Bass.ChannelGetInfo(Channel).Resolution;

            if (Handle != 0) 
                IsAssigned = true;
        }

        void OnDsp(int handle, int channel, IntPtr Buffer, int Length, IntPtr User)
        {
            if (IsAssigned && !Bypass)
                Callback(new BufferProvider(Buffer, Length));
        }

        protected abstract void Callback(BufferProvider Buffer);

        public void Dispose()
        {
            Bass.ChannelRemoveDSP(Channel, Handle);
            IsAssigned = false;
        }

        protected void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
