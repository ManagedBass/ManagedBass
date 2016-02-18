using System;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    public abstract class DSP : IDisposable
    {
        int Channel;

        public int Handle { get; }

        public int Priortity { get; }

        public bool IsAssigned { get; private set; }

        DSPProcedure DSPProc;

        static DSP() { Bass.FloatingPointDSP = true; }

        public DSP(int Channel, int Priority = 0)
        {
            this.Channel = Channel;

            DSPProc = new DSPProcedure(OnDSP);

            this.Priortity = Priortity;

            Handle = Bass.ChannelSetDSP(Channel, DSPProc, IntPtr.Zero, Priority);

            Bass.ChannelSetSync(Channel, SyncFlags.Free, 0, (a, b, c, d) => Dispose());

            if (Handle != 0) IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        void OnDSP(int handle, int channel, IntPtr Buffer, int Length, IntPtr User)
        {
            if (IsAssigned) Callback(new BufferProvider(Buffer, Length));
        }

        protected abstract void Callback(BufferProvider buffer);

        public void Dispose()
        {
            Bass.ChannelRemoveDSP(Channel, Handle);
            IsAssigned = false;
            Channel = 0;
            DSPProc = null;
        }
    }
}
