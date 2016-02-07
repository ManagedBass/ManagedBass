using System;

namespace ManagedBass.Dynamics
{
    /// <summary>
    /// User defined notification callback function (to be used with <see cref="BassAsio.SetNotify" />).
    /// </summary>
    /// <param name="Notify">The notification, one of the following:
    /// <list Type="table">
    /// <item><term><see cref="AsioNotify.Rate"/></term>
    /// <description>
    ///     The device's sample rate has changed.
    ///     The new rate is available from <see cref="BassAsio.Rate" />.
    /// </description>
    /// </item>
    /// <item><term><see cref="AsioNotify.Reset"/></term>
    /// <description>
    ///     The driver has requested a reset/reinitialization;
    ///     for example, following a change of the default Buffer size.
    ///     This request can be ignored, but if a reinitialization is performed, it should not be done within the callback.
    /// </description>
    /// </item>
    /// </list>
    /// </param>
    /// <param name="User">The User instance data given when <see cref="BassAsio.SetNotify" /> was called.</param>
    /// <remarks>
    /// When using multiple devices, <see cref="BassAsio.CurrentDevice" /> can be used to determine which the notification applies to.
    /// </remarks>
    public delegate void AsioNotifyProcedure(AsioNotify Notify, IntPtr User);
}