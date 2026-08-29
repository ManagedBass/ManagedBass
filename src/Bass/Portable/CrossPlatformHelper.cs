using System;
using System.Runtime.CompilerServices;
#if NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif

namespace ManagedBass;

/// <summary>
/// Runtime feature and operating-system detection.
/// </summary>
/// <remarks>
/// On .NET 5+ every predicate delegates to <c>OperatingSystem</c>. <c>netstandard2.0</c> can only tell
/// Windows, Linux and macOS apart via <c>RuntimeInformation</c>, so the Apple-mobile and Android predicates
/// are always <see langword="false"/> there; <c>net45</c> is Windows-only by construction.
/// </remarks>
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
#else
        true;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on iOS, excluding Mac Catalyst.
    /// </summary>
    /// <remarks>
    /// Deliberately deviates from the BCL: <c>OperatingSystem.IsIOS()</c> is <see langword="true"/> on
    /// Mac Catalyst while <c>IsMacOS()</c> is <see langword="false"/> there. Excluding Catalyst keeps
    /// <see cref="IsIOS"/>, <see cref="IsTvOS"/>, <see cref="IsMacCatalyst"/> and <see cref="IsMacOS"/>
    /// mutually exclusive. Always <see langword="false"/> on <c>netstandard2.0</c> and <c>net45</c>.
    /// </remarks>
    public static bool IsIOS =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsIOS() && !OperatingSystem.IsMacCatalyst();
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on tvOS.
    /// Always <see langword="false"/> on <c>netstandard2.0</c> and <c>net45</c>.
    /// </summary>
    public static bool IsTvOS =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsTvOS();
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on Mac Catalyst.
    /// Always <see langword="false"/> on <c>netstandard2.0</c> and <c>net45</c>.
    /// </summary>
    public static bool IsMacCatalyst =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsMacCatalyst();
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on macOS, excluding Mac Catalyst.
    /// </summary>
    public static bool IsMacOS =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsMacOS();
#elif NETSTANDARD2_0
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on Windows.
    /// </summary>
    public static bool IsWindows =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsWindows();
#elif NETSTANDARD2_0
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
        true;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on Linux.
    /// </summary>
    /// <remarks>
    /// On <c>netstandard2.0</c> this is also <see langword="true"/> on Android, which
    /// <c>RuntimeInformation</c> cannot distinguish from Linux; see <see cref="IsAndroid"/>.
    /// </remarks>
    public static bool IsLinux =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsLinux();
#elif NETSTANDARD2_0
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on Android.
    /// Always <see langword="false"/> on <c>netstandard2.0</c> (where Android reports as Linux) and <c>net45</c>.
    /// </summary>
    public static bool IsAndroid =>
#if NET5_0_OR_GREATER
        OperatingSystem.IsAndroid();
#else
        false;
#endif

    /// <summary>
    /// Gets a value indicating whether the current process is running on any Apple platform
    /// (iOS, tvOS, Mac Catalyst or macOS).
    /// </summary>
    public static bool IsApple => IsIOS || IsTvOS || IsMacCatalyst || IsMacOS;
}
