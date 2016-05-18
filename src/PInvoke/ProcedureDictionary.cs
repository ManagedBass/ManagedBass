using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    class ReferenceHolder
    {
        readonly Dictionary<Tuple<int, object>, object> _procedures = new Dictionary<Tuple<int, object>, object>();
        readonly SyncProcedure _freeproc;

        public ReferenceHolder(bool Free = true)
        {
            if (Free)
                _freeproc = Callback; 
        }

        public void Add(int Handle, object SpecificHandle, object proc)
        {
            var key = new Tuple<int, object>(Handle, SpecificHandle);

            var contains = _procedures.ContainsKey(key);

            if (proc == null)
            {
                if (contains)
                    _procedures.Remove(key);

                return;
            }

            if (contains)
                _procedures[key] = proc;
            else _procedures.Add(key, proc);

            if (_freeproc == null)
                return;

            if (_procedures.Any(pair => pair.Key.Item1 == Handle))
                return;
            
            Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, _freeproc);
        }

        public void Remove<T>(int Handle, object SpecialHandle)
        {
            foreach (var pair in _procedures.Where(Pair => Pair.Key.Item1 == Handle
                                                          && Pair.Key.Item2 == SpecialHandle
                                                          && Pair.Value.GetType() == typeof(T)))
            {
                _procedures.Remove(pair.Key);
                break;
            }
        }

        void Callback(int Handle, int Channel, int Data, IntPtr User)
        {
            foreach (var pair in _procedures.Where(Pair => Pair.Key.Item1 == Handle))
                _procedures.Remove(pair.Key);
        }
    }
}
