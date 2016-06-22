using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// Base class for DSPs.
    /// </summary>
    public abstract class DSP : IDisposable, INotifyPropertyChanged
    {
        public int Channel { get; private set; }

        int Handle;

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
        
        public void ApplyOn(int Channel, int Priority = 0)
        {
            this.Channel = Channel;
            
            _priority = Priority;

            Handle = Bass.ChannelSetDSP(Channel, OnDsp, Priority: _priority);

            Resolution = Bass.ChannelGetInfo(Channel).Resolution;
            
            Bass.ChannelSetSync(Channel, SyncFlags.Free, 0, (a, b, c, d) => Dispose());

            if (Handle != 0) 
                IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        public void ApplyOn(Channel Channel, int Priority = 0) => ApplyOn(Channel.Handle, Priority);

        public void ApplyOn(MediaPlayer Player, int Priority)
        {
            _priority = Priority;

            Reassign(Player.Handle);

            Player.MediaLoaded += Reassign;
        }

        void Reassign(int h)
        {
            Channel = h;

            Handle = Bass.ChannelSetDSP(Channel, OnDsp, Priority: _priority);

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
