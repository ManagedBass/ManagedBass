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
        /// <summary>
        /// Gets the Channel on which the DSP is applied.
        /// </summary>
        public int Channel { get; private set; }

        int _handle;

        int _priority;

        /// <summary>
        /// Gets or Sets the DSP priority.
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set
            {
                if (Bass.FXSetPriority(_handle, value))
                    _priority = value;
            }
        }

        bool _isAssigned;
        
        /// <summary>
        /// Gets whether the DSP is assigned.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets whether the DSP should be bypassed.
        /// </summary>
        public bool Bypass 
        {
            get { return _bypass; }
            set
            {
                _bypass = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the <see cref="Resolution"/> of the <see cref="Channel"/> on which the DSP is applied.
        /// </summary>
        public Resolution Resolution { get; private set; }
        
        /// <summary>
        /// Applies the DSP on a Channel.
        /// </summary>
        /// <param name="Channel">Channel to apply the DSP on.</param>
        /// <param name="Priority">Priority of the DSP in the DSP chain.</param>
        public void ApplyOn(int Channel, int Priority = 0)
        {
            this.Channel = Channel;
            
            _priority = Priority;

            _handle = Bass.ChannelSetDSP(Channel, OnDsp, Priority: _priority);

            Resolution = Bass.ChannelGetInfo(Channel).Resolution;
            
            Bass.ChannelSetSync(Channel, SyncFlags.Free, 0, (a, b, c, d) => Dispose());

            if (_handle != 0) 
                IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        /// <summary>
        /// Applies the DSP on a <see cref="MediaPlayer"/>.
        /// </summary>
        /// <param name="Player"><see cref="MediaPlayer"/> to apply the DSP on.</param>
        /// <param name="Priority">Priority of the DSP in the DSP chain.</param>
        public void ApplyOn(MediaPlayer Player, int Priority = 0)
        {
            _priority = Priority;

            Reassign(Player.Handle);

            Player.MediaLoaded += Reassign;
        }

        void Reassign(int h)
        {
            Channel = h;

            _handle = Bass.ChannelSetDSP(Channel, OnDsp, Priority: _priority);

            if (Channel != 0)
                Resolution = Bass.ChannelGetInfo(Channel).Resolution;

            if (_handle != 0) 
                IsAssigned = true;
        }

        void OnDsp(int handle, int channel, IntPtr Buffer, int Length, IntPtr User)
        {
            if (IsAssigned && !Bypass)
                Callback(Buffer, Length);
        }

        /// <summary>
        /// DSP Callback.
        /// </summary>
        /// <param name="Buffer">Pointer to Buffer allocated by Bass.</param>
        /// <param name="Length">No of bytes in buffer.</param>
        protected abstract void Callback(IntPtr Buffer, int Length);

        /// <summary>
        /// Frees all resources used by this instance.
        /// </summary>
        public virtual void Dispose()
        {
            Bass.ChannelRemoveDSP(Channel, _handle);
            IsAssigned = false;
        }

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Fired when a Property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
