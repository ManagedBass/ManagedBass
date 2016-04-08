using System;

namespace ManagedBass
{
    [Flags]
    public enum FXChannelFlags
    {
        /// <summary>
        /// All channels at once (as by default).
        /// </summary>
        All = -1,
        
        /// <summary>
        /// Disable an effect for all channels (resp. set the global volume of the Volume effect).
        /// </summary>
        None = 0,
        
        /// <summary>
        /// left-front channel
        /// </summary>
        Channel1 = 1,
        
        /// <summary>
        /// right-front channel
        /// </summary>
        Channel2 = 2,
        
        /// <summary>
        /// Channel 3: depends on the multi-channel source (see above info).
        /// </summary>
        Channel3 = 4,
        
        /// <summary>
        /// Channel 4: depends on the multi-channel source (see above info).
        /// </summary>
        Channel4 = 8,
        
        /// <summary>
        /// Channel 5: depends on the multi-channel source (see above info).
        /// </summary>
        Channel5 = 16,
        
        /// <summary>
        /// Channel 6: depends on the multi-channel source (see above info).
        /// </summary>
        Channel6 = 32,
        
        /// <summary>
        /// Channel 7: depends on the multi-channel source (see above info).
        /// </summary>
        Channel7 = 64,
        
        /// <summary>
        /// Channel 8: depends on the multi-channel source (see above info).
        /// </summary>
        Channel8 = 128,
        
        /// <summary>
        /// Channel 9: depends on the multi-channel source (see above info).
        /// </summary>
        Channel9 = 256,
        
        /// <summary>
        /// Channel 10: depends on the multi-channel source (see above info).
        /// </summary>
        Channel10 = 512,
        
        /// <summary>
        /// Channel 11: depends on the multi-channel source (see above info).
        /// </summary>
        Channel11 = 1024,
        
        /// <summary>
        /// Channel 12: depends on the multi-channel source (see above info).
        /// </summary>
        Channel12 = 2048,
        
        /// <summary>
        /// Channel 13: depends on the multi-channel source (see above info).
        /// </summary>
        Channel13 = 4096,
        
        /// <summary>
        /// Channel 14: depends on the multi-channel source (see above info).
        /// </summary>
        Channel14 = 8192,
        
        /// <summary>
        /// Channel 15: depends on the multi-channel source (see above info).
        /// </summary>
        Channel15 = 16384,
        
        /// <summary>
        /// Channel 16: depends on the multi-channel source (see above info).
        /// </summary>
        Channel16 = 32768,
        
        /// <summary>
        /// Channel 17: depends on the multi-channel source (see above info).
        /// </summary>
        Channel17 = 65536,
        
        /// <summary>
        /// Channel 18: depends on the multi-channel source (see above info).
        /// </summary>
        Channel18 = 131072,
        
        /// <summary>
        /// Channel 19: depends on the multi-channel source (see above info).
        /// </summary>
        Channel19 = 262144,
        
        /// <summary>
        /// Channel 20: depends on the multi-channel source (see above info).
        /// </summary>
        Channel20 = 524288,
        
        /// <summary>
        /// Channel 21: depends on the multi-channel source (see above info).
        /// </summary>
        Channel21 = 1048576,
        
        /// <summary>
        /// Channel 22: depends on the multi-channel source (see above info).
        /// </summary>
        Channel22 = 2097152,
        
        /// <summary>
        /// Channel 23: depends on the multi-channel source (see above info).
        /// </summary>
        Channel23 = 4194304,
        
        /// <summary>
        /// Channel 24: depends on the multi-channel source (see above info).
        /// </summary>
        Channel24 = 8388608,
        
        /// <summary>
        /// Channel 25: depends on the multi-channel source (see above info).
        /// </summary>
        Channel25 = 16777216,
        
        /// <summary>
        /// Channel 26: depends on the multi-channel source (see above info).
        /// </summary>
        Channel26 = 33554432,
        
        /// <summary>
        /// Channel 27: depends on the multi-channel source (see above info).
        /// </summary>
        Channel27 = 67108864,
        
        /// <summary>
        /// Channel 28: depends on the multi-channel source (see above info).
        /// </summary>
        Channel28 = 134217728,
        
        /// <summary>
        /// Channel 29: depends on the multi-channel source (see above info).
        /// </summary>
        Channel29 = 268435456,
        
        /// <summary>
        /// Channel 30: depends on the multi-channel source (see above info).
        /// </summary>
        Channel30 = 536870912
    }
}