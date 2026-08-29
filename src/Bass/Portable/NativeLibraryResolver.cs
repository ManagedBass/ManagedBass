using System;
using System.Reflection;
#if NET7_0_OR_GREATER
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
#endif

namespace ManagedBass;

/// <summary>
/// Resolves BASS <c>DllImport</c> lookups when the natives ship as a framework embedded in an Apple app bundle
/// (iOS, tvOS, Mac Catalyst), where the runtime's default probing does not find them.
/// </summary>
/// <remarks>
/// A no-op on every other platform and on <c>net45</c>/<c>netstandard2.0</c>. NuGet consumers do not call
/// this: <c>buildTransitive/ManagedBass.targets</c> injects a module initialiser into Apple app projects that
/// does. Disable at runtime with the <c>ManagedBass.DisableNativeLibraryResolver</c> <c>AppContext</c> switch.
/// </remarks>
public static class NativeLibraryResolver
{
    private const string DisableSwitchName = "ManagedBass.DisableNativeLibraryResolver";

#if NET7_0_OR_GREATER
    // Resolver runs once per P/Invoke entry point; Bass.cs alone declares hundreds.
    private static readonly ConcurrentDictionary<string, IntPtr> _handles = new();

    // SetDllImportResolver throws on a second registration for the same assembly.
    private static readonly ConcurrentDictionary<Assembly, bool> _registered = new();

    private static int _hooked;
#endif

    /// <summary>
    /// Registers the resolver for every loaded <c>ManagedBass*</c> assembly and any loaded later. Idempotent.
    /// </summary>
    public static void RegisterAll()
    {
#if NET7_0_OR_GREATER
        if (!Enabled)
            return;

        // Hook before sweeping so an add-on loading mid-sweep cannot slip through.
        if (Interlocked.Exchange(ref _hooked, 1) == 0)
            AppDomain.CurrentDomain.AssemblyLoad += (_, args) => Register(args.LoadedAssembly);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            Register(assembly);
#endif
    }

    /// <summary>
    /// Registers the resolver for a single assembly. Ignored unless its name starts with <c>ManagedBass</c>.
    /// </summary>
    /// <param name="assembly">The assembly whose <c>DllImport</c> lookups should be resolved.</param>
    public static void Register(Assembly assembly)
    {
#if NET7_0_OR_GREATER
        if (!Enabled || assembly?.GetName().Name is not { } name || !name.StartsWith("ManagedBass", StringComparison.Ordinal))
            return;

        if (!_registered.TryAdd(assembly, true))
            return;

        try
        {
            NativeLibrary.SetDllImportResolver(assembly, Resolve);
        }
        catch (InvalidOperationException)
        {
            // A host registered its own resolver first; respect it.
        }
#endif
    }

#if NET7_0_OR_GREATER
    // NOTE: desktop stays untouched so a missing native remains a plain DllNotFoundException rather than
    // the EntryPointNotFoundException the executable-itself fallback would produce.
    private static bool Enabled =>
        (CrossPlatformHelper.IsIOS || CrossPlatformHelper.IsTvOS || CrossPlatformHelper.IsMacCatalyst)
        && !(AppContext.TryGetSwitch(DisableSwitchName, out var disabled) && disabled);

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) =>
        _handles.GetOrAdd(libraryName, name =>
        {
            if (NativeLibrary.TryLoad(name, assembly, searchPath, out var handle))
                return handle;

            // Embedded framework layout; also covers embedded-but-not-linked.
            if (NativeLibrary.TryLoad($"{name}.framework/{name}", assembly, searchPath, out handle))
                return handle;

            // dlopen(NULL): sees anything linked into the process, dynamic frameworks included.
            return NativeLibrary.GetMainProgramHandle();
        });
#endif
}
