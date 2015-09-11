using System;

namespace ManagedBass.Effects
{
    public interface IEffectAssignable
    {
        int Handle { get; }
        event EventHandler Disposed;
    }
}
