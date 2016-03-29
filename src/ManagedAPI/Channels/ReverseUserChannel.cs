using System;
using System.Runtime.InteropServices;

namespace ManagedBass
{
    /// <summary>
    /// Streams audio from a Decoder allowing manipulation of Playback Direction without BassFx.
    /// </summary>
    public sealed class ReverseUserChannel : Channel
    {
        Channel _decoder;
        readonly int _dHandle;
        int _qLength, _bytesRead;
        long _pos, _dur, _diff;
        readonly StreamProcedure _proc;
        float[] _buffer;

        public ReverseUserChannel(Channel DecodingSource)
        {
			_decoder = DecodingSource;
            _dHandle = DecodingSource.Handle;
            _proc = Callback;

			if (!_decoder.IsDecodingChannel)
                throw new ArgumentException("Not a Decoding Channel!");
			
			Handle = Bass.CreateStream((int)_decoder.Frequency, _decoder.ChannelCount, BassFlags.Float, _proc);
                        
            Bass.ChannelSetPosition(_dHandle, Bass.ChannelGetLength(_dHandle) - 1);
        }

        public bool Reverse { get; set; } = true;

        public override bool Loop { get; set; } = false;

        unsafe int Callback(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            _pos = Bass.ChannelGetPosition(_dHandle);
            _dur = Bass.ChannelGetLength(_dHandle);

            _qLength = Length / 4;

            if (_buffer == null || _buffer.Length < _qLength)
                _buffer = new float[_qLength];

            // Looping
            if (Reverse)
            {
                _diff = _pos - Length;

                if (_diff <= 0) 
                {
                    if (Loop)
                        Bass.ChannelSetPosition(_dHandle, _dur + _diff - 1);
                    else Bass.ChannelStop(Handle);
                }
                else Bass.ChannelSetPosition(_dHandle, _diff);
            }
            else if (_pos >= _dur)
            {
                if (Loop)
                    Bass.ChannelSetPosition(_dHandle, 0);
                else Bass.ChannelStop(Handle);
            }

            _bytesRead = Bass.ChannelGetData(_dHandle, _buffer, Length | (int)DataFlags.Float);
            
            _qLength  = _bytesRead / 4;

            if (Reverse)
            {
                var b = (float*)Buffer;

                for (var i = 0; i < _qLength; ++i)
                    b[i] = _buffer[_qLength - i - 1];
            }
            else Marshal.Copy(_buffer, 0, Buffer, _qLength);

            return _bytesRead;
        }

        public override double Position
        {
	        get { return Bass.ChannelBytes2Seconds(_dHandle, Bass.ChannelGetPosition(_dHandle)); }
            set { Bass.ChannelSetPosition(_dHandle, Bass.ChannelSeconds2Bytes(_dHandle, value)); }
        }

        public override double Duration => Bass.ChannelBytes2Seconds(_dHandle, Bass.ChannelGetLength(_dHandle));

        public override void Dispose()
        {
            base.Dispose();
            _decoder = null;
        }
    }
}