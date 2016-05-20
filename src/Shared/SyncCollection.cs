using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ManagedBass
{
    public class SyncCollection : IDisposable, IEnumerable<SyncParameters>
    {
        readonly SynchronizationContext _syncContext;
        readonly Dictionary<int, SyncParameters> _syncs = new Dictionary<int, SyncParameters>();
        Channel _channel;

        public SyncCollection(Channel Channel)
        {
            _syncContext = SynchronizationContext.Current;
            _channel = Channel;
        }

        public void Add(SyncParameters Parameters)
        {
            var hsync = Bass.ChannelSetSync(_channel.Handle, Parameters.Type, Parameters.Parameter, Procedure);
            
            if (hsync == 0)
                throw new InvalidOperationException("Set Sync Failed");

            Parameters.SyncHandle = hsync;

            _syncs.Add(hsync, Parameters);
        }

        public void Remove(SyncParameters Parameters)
        {
            Bass.ChannelRemoveSync(_channel.Handle, Parameters.SyncHandle);

            _syncs.Remove(Parameters.SyncHandle);
        }

        void Procedure(int Handle, int Channel, int Data, IntPtr User)
        {
            var sync = _syncs[Handle];

            var handler = sync.Callback;

            if (handler == null)
                return;

            if (_syncContext == null)
                handler(sync, Data);
            else _syncContext.Post(State => handler(sync, Data), null);
        }

        public void Dispose()
        {
            foreach (var sync in _syncs)
                Bass.ChannelRemoveSync(_channel.Handle, sync.Key);

            _channel = null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<SyncParameters> GetEnumerator() => _syncs.Values.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}