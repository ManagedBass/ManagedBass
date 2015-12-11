using System;
using System.Runtime.InteropServices;

namespace ManagedBass.Dynamics
{
    public static class BassMidi
    {
        const string DllName = "bassmidi.dll";

        static BassMidi()
        {
            //BassManager.Load(DllName);
        }

        [DllImport(DllName, EntryPoint = "BASS_MIDI_StreamCreateFile")]
        public static extern int CreateStream(bool mem, string file, long offset, long length, BassFlags flags, int freq);
    }
}