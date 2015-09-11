using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// User defined DSP callback function (to be used with <see cref="Bass.ChannelSetDSP" />).
    /// </summary>
    /// <param name="Handle">The DSP Handle (as returned by <see cref="Bass.ChannelSetDSP" />).</param>
    /// <param name="Channel">Channel that the DSP is being applied to.</param>
    /// <param name="Buffer">
    /// The pointer to the Buffer to apply the DSP to. 
    /// The sample data is in standard Windows PCM format - 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to 1 (not clipped, so can actually be outside this range).
    /// </param>
    /// <param name="Length">The number of bytes to process.</param>
    /// <param name="User">The User instance data given when <see cref="Bass.ChannelSetDSP" /> was called.</param>
    /// <remarks>
    /// <para>A DSP function should obviously be as quick as possible... playing streams, MOD musics and other DSP functions can not be processed until it has finished.</para>
    /// <para>
    /// Some functions can cause problems if called from within a DSP (or stream) function.
    /// Do not call these functions from within a DSP callback:
    /// </para>
    /// <para>
    /// <see cref="Bass.Stop" />, <see cref="Bass.Free" />, <see cref="Bass.MusicLoad(string,long,int,BassFlags,int)" />,
    /// <see cref="Bass.CreateStream(int,int,BassFlags,StreamProcedure,IntPtr)" /> (or any other stream creation functions).
    /// </para>
    /// <para>Also, do not call <see cref="Bass.ChannelRemoveDSP" /> with the same DSP Handle as received by the callback, 
    /// or <see cref="Bass.ChannelStop" />, <see cref="Bass.MusicFree" />, <see cref="Bass.StreamFree" /> with the same channel Handle as received by the callback.</para>
    /// <para>If the <see cref="Bass.FloatingPointDSP"/> config option is set, then DSP callback functions will always be passed 32-bit floating-point sample data, regardless of what the channels' actual sample format is.</para>
    /// <para>
    /// It is clever to NOT alloc Buffer data (e.g. a float[]) everytime within the callback method, since ALL callbacks should be really fast!
    /// And if you would do a 'float[] data = new float[]' every time here...the GarbageCollector would never really clean up that memory.
    /// Sideeffects might occure, due to the fact, that BASS will call this callback too fast and too often...
    /// However, this is not always the case, so in most examples it'll work just fine - but if you got problems - try moving any memory allocation things outside any callbacks.
    /// </para>
    /// <para>NOTE: When you pass an instance of a callback delegate to one of the BASS functions, this delegate object will not be reference counted. 
    /// This means .NET would not know, that it might still being used by BASS. The Garbage Collector might (re)move the delegate instance, if the variable holding the delegate is not declared as global.
    /// So make sure to always keep your delegate instance in a variable which lives as long as BASS needs it, e.g. use a global variable or member.</para>
    /// </remarks>
    public delegate void DSPProcedure(int Handle, int Channel, IntPtr Buffer, int Length, IntPtr User);
}