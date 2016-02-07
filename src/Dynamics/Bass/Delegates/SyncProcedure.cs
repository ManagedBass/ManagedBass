using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// User defined synchronizer callback function (see <see cref="Bass.ChannelSetSync" /> for details).
    /// </summary>
    /// <param name="Handle">The sync Handle that has occured (as returned by <see cref="Bass.ChannelSetSync" />).</param>
    /// <param name="Channel">The channel that the sync occured on.</param>
    /// <param name="Data">Additional data associated with the sync's occurance.</param>
    /// <param name="User">The User instance data given when <see cref="Bass.ChannelSetSync" /> was called.</param>
    /// <remarks>
    /// <para>
    /// BASS creates a single thread dedicated to executing sync callback functions, so a callback function should be quick as other syncs cannot be processed until it has finished. 
    /// Attribute slides (<see cref="Bass.ChannelSlideAttribute" />) are also performed by the sync thread, so are also affected if a sync callback takes a long time.</para>
    /// <para>"Mixtime" syncs <see cref="SyncFlags.Mixtime"/> are not executed in the sync thread, but immediately in whichever thread triggers them.
    /// In most cases that will be an update thread, and so the same restrictions that apply to stream callbacks (<see cref="StreamProcedure" />) also apply here.</para>
    /// <para>
    /// <see cref="Bass.ChannelSetPosition" /> can be used in a mixtime sync to implement custom looping, 
    /// eg. set a <see cref="SyncFlags.Position"/> sync at the loop end position and seek to the loop start position in the callback.
    /// </para>
    /// <para>
    /// NOTE: When you pass an instance of a callback delegate to one of the BASS functions, this delegate object will not be reference counted. 
    /// This means .NET would not know, that it might still being used by BASS. 
    /// The Garbage Collector might (re)move the delegate instance, if the variable holding the delegate is not declared as global.
    /// So make sure to always keep your delegate instance in a variable which lives as long as BASS needs it, e.g. use a global variable or member.
    /// </para>
    /// </remarks>
    public delegate void SyncProcedure(int Handle, int Channel, int Data, IntPtr User);
}