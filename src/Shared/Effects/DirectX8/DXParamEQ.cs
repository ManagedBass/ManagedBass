using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ManagedBass.DirectX8
{
    /// <summary>
    /// DX8 ParamEQ Effect.
    /// </summary>
    public sealed class DXParamEQ : IDisposable
    {
        readonly DXParamEQParameters _parameters;
        GCHandle _gch;
        readonly int _channel;
        readonly List<int> _handles = new List<int>();

        /// <summary>
        /// Creates a new instance of <see cref="DXParamEQ"/>.
        /// </summary>
        public DXParamEQ(int Channel, double Bandwidth = 18)
        {
            _parameters = new DXParamEQParameters
            {
                fBandwidth = (float)Bandwidth
            };

            _gch = GCHandle.Alloc(_parameters, GCHandleType.Pinned);
        }

        /// <summary>
        /// Adds a Band.
        /// </summary>
        public int AddBand(double CenterFrequency)
        {
            _parameters.fCenter = (float)CenterFrequency;
            _parameters.fGain = 0;

            var hfx = Bass.ChannelSetFX(_channel, EffectType.DXParamEQ, 0);

            Bass.FXSetParameters(hfx, _gch.AddrOfPinnedObject());

            _handles.Add(hfx);
            return _handles.Count - 1;
        }
        
        /// <summary>
        /// Updates a Band.
        /// </summary>
        public void UpdateBand(int Band, double Gain)
        {
            Bass.FXGetParameters(_handles[Band], _gch.AddrOfPinnedObject());

            _parameters.fGain = (float)Gain;

            Bass.FXSetParameters(_handles[Band], _gch.AddrOfPinnedObject());
        }

        /// <summary>
        /// Frees all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            _gch.Free();

            foreach (var fx in _handles)
                Bass.ChannelRemoveFX(_channel, fx);
        }
    }
}