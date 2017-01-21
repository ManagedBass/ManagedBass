using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagedBass
{
    /// <summary>
    /// Holds References to Channel Items like <see cref="SyncProcedure"/> and <see cref="FileProcedures"/>.
    /// </summary>
    public static class ChannelReferences
    {
        static readonly Dictionary<Tuple<int, int>, object> Procedures = new Dictionary<Tuple<int, int>, object>();
        static readonly SyncProcedure Freeproc = Callback;
        
        /// <summary>
        /// Adds a Reference.
        /// </summary>
        public static void Add(int Handle, int SpecificHandle, object proc)
        {
            if (proc.Equals(Freeproc))
                return;

            var key = Tuple.Create(Handle, SpecificHandle);

            var contains = Procedures.ContainsKey(key);
            
            if (Freeproc != null && Procedures.All(pair => pair.Key.Item1 != Handle))
                Bass.ChannelSetSync(Handle, SyncFlags.Free, 0, Freeproc);

            if (contains)
                Procedures[key] = proc;
            else Procedures.Add(key, proc);
        }

        /// <summary>
        /// Removes a Reference.
        /// </summary>
        public static void Remove(int Handle, int SpecialHandle)
        {
            var key = Tuple.Create(Handle, SpecialHandle);
            
            if (Procedures.ContainsKey(key))
                Procedures.Remove(key);
        }
        
        static void Callback(int Handle, int Channel, int Data, IntPtr User)
        {
            // ToArray is necessary because the object iterated on should not be modified.
            var toRemove = Procedures.Where(Pair => Pair.Key.Item1 == Channel).Select(Pair => Pair.Key).ToArray();
            
            foreach (var key in toRemove)
                Procedures.Remove(key);
        }
    }
}
