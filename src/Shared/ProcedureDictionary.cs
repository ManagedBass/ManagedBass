using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    class ReferenceHolder
    {
        Dictionary<Tuple<int, object>, object> Procedures = new Dictionary<Tuple<int, object>, object>();
        readonly SyncProcedure freeproc;

        public ReferenceHolder(bool Free = true)
        {
            if (Free)
                freeproc = Callback; 
        }

        public void Add(int Handle, object SpecificHandle, object proc)
        {
            var key = new Tuple<int, object>(Handle, SpecificHandle);

            var contains = Procedures.ContainsKey(key);

            if (proc == null)
            {
                if (contains)
                    Procedures.Remove(key);

                return;
            }

            if (contains)
                Procedures[key] = proc;
            else Procedures.Add(key, proc);

            if (freeproc == null)
                return;

            if (Procedures.Any(pair => pair.Key.Item1 == Handle))
                return;
            
            Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, freeproc);
        }

        public void Remove<T>(int Handle, object SpecialHandle)
        {
            foreach (var pair in Procedures.Where(pair => pair.Key.Item1 == Handle
                                                          && pair.Key.Item2 == SpecialHandle
                                                          && pair.Value.GetType() == typeof(T)))
            {
                Procedures.Remove(pair.Key);
                break;
            }
        }

        void Callback(int Handle, int Channel, int Data, IntPtr User)
        {
            foreach (var pair in Procedures.Where(pair => pair.Key.Item1 == Handle))
                Procedures.Remove(pair.Key);
        }
    }
}
