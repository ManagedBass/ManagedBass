using System;
using System.Runtime.CompilerServices;

namespace ManagedBass
{
    /// <summary>
    /// Provides cross-platform helper methods for runtime feature detection.
    /// </summary>
    public static class CrossPlatformHelper
    {
        /// <summary>
        /// Gets a value indicating whether dynamic code generation is supported on the current platform.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if dynamic code generation is supported; otherwise, <see langword="false"/>.
        /// Returns <see langword="false"/> on iOS and other platforms with AOT-only compilation.
        /// </value>
        public static bool IsDynamicCodeSupported =>
#if NETCOREAPP3_0_OR_GREATER
            RuntimeFeature.IsDynamicCodeSupported;
#elif __IOS__
            false;
#else
            true;
#endif
        
    }
}