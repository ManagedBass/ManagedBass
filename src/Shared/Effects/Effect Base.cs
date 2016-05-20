using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace ManagedBass
{
    /// <summary>
    /// Wraps a Bass Effect such that you don't need to touch the Bass functions to Handle it.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="IEffectParameter"/></typeparam>
    public abstract class Effect<T> : IDisposable, INotifyPropertyChanged where T : class, IEffectParameter, new()
    {
        int _channel, _effectHandle, _hfsync;
        GCHandle _gch;
        readonly SyncProcedure _freeproc;
        readonly bool _mediaPlayer;
        bool _wasActive;

        /// <summary>
        /// Effect's Parameters.
        /// </summary>
        protected T Parameters = new T();

        protected Effect(int Handle, int Priority)
        {
            _channel = Handle;
            _priority = Priority;

            _gch = GCHandle.Alloc(Parameters, GCHandleType.Pinned);

            _freeproc = (a, b, c, d) => Dispose();

            _hfsync = Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, _freeproc);
        }

        protected Effect(MediaPlayer player, int Priority) : this(player.Handle, Priority)
        {
            _mediaPlayer = true;

            player.MediaLoaded += NewHandle =>
                {
                    if (_wasActive)
                        IsActive = false;

                    Bass.ChannelRemoveSync(_channel, _hfsync);

                    _channel = NewHandle;
                    _hfsync = Bass.ChannelSetSync(NewHandle, SyncFlags.Free, 0, _freeproc);

                    IsActive = _wasActive;
                };
        }
        
        int _priority;

        /// <summary>
        /// Priority of the Effect in DSP chain.
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set
            {
                if (IsActive && Bass.FXSetPriority(_effectHandle, value))
                    _priority = value;
            }
        }
        
        /// <summary>
        /// Removes the effect from the Channel and frees allocated memory.
        /// </summary>
        public void Dispose()
        {
            if (IsActive)
            {
                IsActive = false;

                _wasActive = true;
            }
            else _wasActive = false;

            _channel = _effectHandle = 0;

            if (!_mediaPlayer)
                _gch.Free();
        }
        
        /// <summary>
        /// Sets the effect parameters to default by initialising a new instance of <typeparamref name="T"/>.
        /// </summary>
        public void Default()
        {
            // Reallocate memory for Parameters
            _gch.Free();
            Parameters = new T();
            _gch = GCHandle.Alloc(Parameters, GCHandleType.Pinned);

            OnPropertyChanged("");
        }

        /// <summary>
        /// Checks whether the effect is currently enabled and active.
        /// </summary>
        public bool IsActive
        {
            set
            {
                if (_channel == 0)
                    return;

                if (value && !IsActive)
                    _effectHandle = Bass.ChannelSetFX(_channel, Parameters.FXType, 1);
                
                else if (!value && IsActive && Bass.ChannelRemoveFX(_channel, _effectHandle))
                    _effectHandle = 0;

                OnPropertyChanged();
            }
            get { return _channel != 0 && _effectHandle != 0; }
        }

        /// <summary>
        /// Fire the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            // Update Effect Parameters if effect is active.
            if (IsActive)
                Bass.FXSetParameters(_effectHandle, _gch.AddrOfPinnedObject());

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Fired when a Property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}