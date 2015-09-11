using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    public interface IEffectParameter { EffectType FXType { get; } }

    /// <summary>
    /// TODO: See Bass Fx Examples for Presets
    /// </summary>
    public abstract class Effect<T> : IDisposable where T : struct, IEffectParameter
    {
        public int Channel = 0, EffectHandle = 0;
        protected T Parameters;

        protected Effect(IEffectAssignable Stream)
        {
            this.Channel = Stream.Handle;

            Bass.GetFXParameters(Channel, Parameters);
            IsActive = true;

            Stream.Disposed += (s, e) => Dispose();
        }

        public void Dispose()
        {
            if (IsActive) IsActive = false;
            Parameters = default(T);
            Channel = EffectHandle = 0;
        }

        protected void Update() { if (IsActive)Bass.SetFXParameters(EffectHandle, Parameters); }

        public void Default()
        {
            Parameters = new T();
            if (IsActive) Update();
        }

        public bool IsActive
        {
            set
            {
                if (Channel != 0)
                {
                    if (value && !IsActive)
                    {
                        EffectHandle = Bass.ChannelSetFX(Channel, Parameters.FXType, 1);
                        Update();
                    }
                    else if (!value && IsActive) if (Bass.ChannelRemoveFX(Channel, EffectHandle)) EffectHandle = 0;
                }
            }
            get { return Channel != 0 ? EffectHandle != 0 : false; }
        }
    }
}