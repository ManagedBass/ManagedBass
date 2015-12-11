﻿using System;
using ManagedBass.Dynamics;

namespace ManagedBass.Effects
{
    public abstract class DSP : IDisposable
    {
        int Channel;

        int DSPHandle;

        public int Priortity { get; set; }

        public bool IsAssigned { get; set; }

        DSPProcedure DSPProc;

        public DSP(IEffectAssignable IEA, int Priority = 0)
        {
            Channel = IEA.Handle;

            DSPProc = new DSPProcedure(OnDSP);

            this.Priortity = Priortity;

            DSPHandle = Bass.ChannelSetDSP(Channel, DSPProc, IntPtr.Zero, Priority);

            IEA.Disposed += (s, e) => Dispose();

            if (DSPHandle != 0) IsAssigned = true;
            else throw new InvalidOperationException("DSP Assignment Failed");
        }

        void OnDSP(int handle, int channel, IntPtr Buffer, int Length, IntPtr User)
        {
            if (IsAssigned) Callback(new BufferProvider(Buffer, Length, Bass.FloatingPointDSP ? BufferKind.Float : BufferKind.Short));
        }

        protected abstract void Callback(BufferProvider buffer);

        public void Dispose()
        {
            Bass.ChannelRemoveDSP(Channel, DSPHandle);
            IsAssigned = false;
            Channel = DSPHandle = Priortity = 0;
            DSPProc = null;
        }
    }
}
