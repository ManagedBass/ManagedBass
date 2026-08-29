using System.Runtime.InteropServices;

namespace ManagedBass.Ios.Harness;

/// <summary>
/// The Tier 2 checks, decoupled from UIKit. Every line is prefixed with <see cref="ResultPrefix"/> so a
/// log-scraping CI step can find it.
/// </summary>
/// <remarks>
/// NOTE: contains no resolver registration of its own. A pass proves the module initialiser injected by
/// <c>ManagedBass.targets</c> did the work; <c>reason=dllnotfound</c> means that path has regressed.
/// </remarks>
public static class HarnessProbe
{
    public const string ResultPrefix = "MANAGEDBASS_RESULT: ";

    public static readonly Version MinimumBassVersion = new(2, 4, 17);

    /// <summary>Runs every check, writing one prefixed line per result, and returns a process exit code.</summary>
    public static int Run(Action<string> write)
    {
        void Emit(string line) => write(ResultPrefix + line);

        Emit("harness=start");
        Emit($"framework={(FrameworkIsInBundle() ? "present" : "absent")}");

        // NOTE: the P/Invoke goes first, before anything touches CrossPlatformHelper, so the ordering matches
        // a real consumer and the injected initialiser has to have run already.
        var nativeFailure = ProbeNative(Emit);

        Emit($"runtime={Environment.Version}");
        Emit($"osdescription={RuntimeInformation.OSDescription}");
        Emit($"rid={RuntimeInformation.RuntimeIdentifier}");

        var predicates = new (string Name, bool Actual, bool Expected)[]
        {
            ("IsIOS", CrossPlatformHelper.IsIOS, true),
            ("IsMacCatalyst", CrossPlatformHelper.IsMacCatalyst, false),
            ("IsApple", CrossPlatformHelper.IsApple, true),
            ("IsTvOS", CrossPlatformHelper.IsTvOS, false),
            ("IsMacOS", CrossPlatformHelper.IsMacOS, false),
            ("IsLinux", CrossPlatformHelper.IsLinux, false),
            ("IsWindows", CrossPlatformHelper.IsWindows, false),
            ("IsAndroid", CrossPlatformHelper.IsAndroid, false)
        };

        foreach (var (name, actual, _) in predicates)
            Emit($"{name}={Bool(actual)}");

        var failedPredicates = predicates.Where(p => p.Actual != p.Expected).Select(p => p.Name).ToArray();

        if (failedPredicates.Length > 0)
            Emit($"failedpredicates={string.Join(",", failedPredicates)}");

        ProbeResolution(Emit);

        if (failedPredicates.Length > 0)
            return Fail(Emit, "predicates");

        if (nativeFailure is not null)
            return Fail(Emit, nativeFailure);

        Emit("outcome=PASS");
        return 0;
    }

    private static bool FrameworkIsInBundle() =>
        Directory.Exists(Path.Combine(AppContext.BaseDirectory, "Frameworks", "bass.framework"));

    // Diagnostic only. resolverslotfree=true after the P/Invoke ran means the injected initialiser never
    // registered ManagedBass's resolver.
    private static void ProbeResolution(Action<string> emit)
    {
        var assembly = typeof(Bass).Assembly;

        foreach (var candidate in new[] { "bass", "libbass.dylib", "bass.framework/bass" })
        {
            try
            {
                emit($"probe.tryload[{candidate}]={Bool(NativeLibrary.TryLoad(candidate, assembly, null, out _))}");
            }
            catch (Exception ex)
            {
                emit($"probe.tryload[{candidate}]=threw:{ex.GetType().Name}");
            }
        }

        try
        {
            var main = NativeLibrary.GetMainProgramHandle();
            emit($"probe.mainprogram.BASS_GetVersion={Bool(NativeLibrary.TryGetExport(main, "BASS_GetVersion", out _))}");
        }
        catch (Exception ex)
        {
            emit($"probe.mainprogram=threw:{ex.GetType().Name}");
        }

        try
        {
            NativeLibrary.SetDllImportResolver(assembly, (_, _, _) => IntPtr.Zero);
            emit("probe.resolverslotfree=true");
        }
        catch (InvalidOperationException)
        {
            emit("probe.resolverslotfree=false");
        }
        catch (Exception ex)
        {
            emit($"probe.resolverslot=threw:{ex.GetType().Name}");
        }
    }

    private static string? ProbeNative(Action<string> emit)
    {
        var initialised = false;

        try
        {
            initialised = Bass.Init(Bass.NoSoundDevice, 44100, DeviceInitFlags.Default, IntPtr.Zero);
            emit($"init={Bool(initialised)}");

            if (!initialised)
            {
                emit($"initerror={Bass.LastError}");
                return "initfailed";
            }

            var version = Bass.Version;
            emit($"version={version}");

            if (version < MinimumBassVersion)
            {
                emit($"minimumversion={MinimumBassVersion}");
                return "versiontooold";
            }

            return ProbeSineStream(emit);
        }
        catch (DllNotFoundException ex)
        {
            return Failure(emit, ex, "dllnotfound");
        }
        catch (EntryPointNotFoundException ex)
        {
            return Failure(emit, ex, "entrypointnotfound");
        }
        catch (Exception ex)
        {
            return Failure(emit, ex, "unexpectedexception");
        }
        finally
        {
            if (initialised)
                Bass.Free();
        }
    }

    // A push decode stream on the no-sound device: sine goes in, the same bytes must come back out.
    private static string? ProbeSineStream(Action<string> emit)
    {
        const int sampleRate = 44100;
        const int sampleCount = sampleRate / 10;

        var sine = Enumerable.Range(0, sampleCount)
            .Select(i => (short)(Math.Sin(2 * Math.PI * 440 * i / sampleRate) * short.MaxValue / 2))
            .ToArray();

        var stream = Bass.CreateStream(sampleRate, 1, BassFlags.Decode, StreamProcedureType.Push);
        emit($"stream={(stream != 0 ? "created" : "failed")}");

        if (stream == 0)
        {
            emit($"streamerror={Bass.LastError}");
            return "streamfailed";
        }

        try
        {
            var queued = Bass.StreamPutData(stream, sine, sine.Length * sizeof(short));
            var read = Bass.ChannelGetData(stream, new short[sampleCount], sampleCount * sizeof(short));
            emit($"streamqueued={queued}");
            emit($"streamread={read}");

            return read == sampleCount * sizeof(short) ? null : "streamdata";
        }
        finally
        {
            Bass.StreamFree(stream);
        }
    }

    private static string Failure(Action<string> emit, Exception ex, string reason)
    {
        emit($"exception={ex.GetType().Name}");
        emit($"message={ex.Message.Replace("\r", " ").Replace("\n", " ")}");
        return reason;
    }

    private static int Fail(Action<string> emit, string reason)
    {
        emit($"outcome=FAIL reason={reason}");
        return 1;
    }

    private static string Bool(bool value) => value ? "true" : "false";
}
