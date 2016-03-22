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

        int priority;
        public int Priority
        {
            get { return priority; }
            set
            {
                if (Bass.FXSetPriority(Handle, value))
                    priority = value;
            }
        }

        bool isAssigned;
        public bool IsAssigned
        {
            get { return isAssigned; }
            private set
            {
                isAssigned = value;

                OnPropertyChanged();
            }
        }

        bool bypass = false;
        public bool Bypass 
        {
            get { return bypass; }
            set
            {
                bypass = value;

                OnPropertyChanged();
            }
        }

        public Resolution Resolution { get; private set; }

        DSPProcedure DSPProc;
        SyncProcedure freeproc;

        static DSP() { Bass.FloatingPointDSP = true; }

        protected DSP(int Channel, int Priority)
        {
            this.Channel = Channel;

            DSPProc = new DSPProcedure(OnDSP);

            priority = Priority;

            Handle = Bass.ChannelSetDSP(Channel, DSPProc, Priority: priority);

            Resolution = Bass.ChannelGetInfo(Channel).Resolution;

            freeproc = (a, b, c, d) => Dispose();

            Bass.ChannelSetSync(Channel, SyncFlags.Free, 0, freeproc);

            if (Handle != 0) 
                IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        protected DSP(MediaPlayer player, int Priority)
        {
            DSPProc = new DSPProcedure(OnDSP);

            priority = Priority;

            Reassign(player.Handle);

            player.MediaLoaded += Reassign;
        }

        void Reassign(int h)
        {
            Channel = h;

            Handle = Bass.ChannelSetDSP(Channel, DSPProc, Priority: priority);

            if (Channel != 0)
                Resolution = Bass.ChannelGetInfo(Channel).Resolution;

            if (Handle != 0) 
                IsAssigned = true;
        }

        void OnDSP(int handle, int channel, IntPtr Buffer, int Length, IntPtr User)
        {
            if (IsAssigned && !Bypass)
                Callback(new BufferProvider(Buffer, Length));
        }

        protected abstract void Callback(BufferProvider buffer);

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
