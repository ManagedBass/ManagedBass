using ManagedBass.Dynamics;
using System;

namespace ManagedBass.Effects
{
    /// <summary>
    /// Base class for DSPs.
    /// Sets <see cref="Bass.FloatingPointDSP"/> to <see langword="true"/> so you need to implement the DSP only for 32-bit float audio.
    /// </summary>
    public abstract class DSP : IDisposable
    {
        public int Channel { get; }

        public int Handle { get; }

        public int Priortity { get; }

        public bool IsAssigned { get; private set; }

        public Resolution Resolution { get; }

        DSPProcedure DSPProc;

        static DSP() { Bass.FloatingPointDSP = true; }

        public DSP(int Channel, int Priority = 0)
        {
            this.Channel = Channel;

            DSPProc = new DSPProcedure(OnDSP);

            this.Priortity = Priortity;

            Handle = Bass.ChannelSetDSP(Channel, DSPProc, IntPtr.Zero, Priority);

            Resolution = Bass.ChannelGetInfo(Channel).Resolution;

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
        }
    }
}
