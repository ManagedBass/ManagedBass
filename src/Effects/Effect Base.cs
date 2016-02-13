using System;
using ManagedBass.Dynamics;
using System.Runtime.InteropServices;

namespace ManagedBass.Effects
{
    public interface IEffectParameter { EffectType FXType { get; } }

    /// <summary>
    /// Wraps a Bass Effect such that you don't need to touch the Bass functions to Handle it.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="IEffectParameter"/></typeparam>
    public abstract class Effect<T> : IDisposable where T : class, IEffectParameter, new()
    {
        int Channel = 0, EffectHandle = 0;
        protected T Parameters = new T();
        GCHandle gch;

        protected Effect(int Handle)
        {
            this.Channel = Handle;

            gch = GCHandle.Alloc(Parameters, GCHandleType.Pinned);

            Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, (a, b, c, d) => Dispose());
        }

        /// <summary>
        /// A <see cref="FXChannelFlags" /> flag to define on which channels to apply the effect. Default: <see cref="FXChannelFlags.All"/>
        /// </summary>
        public FXChannelFlags Channels
        {
            get { return (FXChannelFlags)typeof(T).GetField("lChannel").GetValue(Parameters); }
            set { typeof(T).GetField("lChannel").SetValue(Parameters, value); }
        }

        /// <summary>
        /// Removes the effect from the Channel and frees allocated memory.
        /// </summary>
        public void Dispose()
        {
            if (IsActive) IsActive = false;
            Parameters = null;
            Channel = EffectHandle = 0;
            gch.Free();
        }

        /// <summary>
        /// Update Effect Parameters if effect is active.
        /// </summary>
        protected void Update()
        {
            if (IsActive) Bass.FXSetParameters(EffectHandle, gch.AddrOfPinnedObject());
        }

        /// <summary>
        /// Sets the effect parameters to default by initialising a new instance of <typeparamref name="T"/>.
        /// </summary>
        public void Default()
        {
            // Reallocate memory for Parameters
            gch.Free();
            Parameters = new T();
            gch = GCHandle.Alloc(Parameters, GCHandleType.Pinned);

            Update();
        }

        /// <summary>
        /// Checks whether the effect is currently enabled and active.
        /// </summary>
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
            get { return Channel != 0 && EffectHandle != 0; }
        }
    }
}