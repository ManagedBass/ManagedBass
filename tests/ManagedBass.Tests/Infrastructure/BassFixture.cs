namespace ManagedBass.Tests.Infrastructure;

/// <summary>
/// Initialises BASS once against the no-sound device (device 0) so real P/Invoke runs headless on CI.
/// </summary>
public class BassFixture : IDisposable
{
    public BassFixture()
    {
        // Fail with the fetch command, not a bare DllNotFoundException deep inside a test.
        NativeLibraries.Require("bass");
        NativeLibraries.Require("bassflac");
        NativeLibraries.Require("bassenc");

        try
        {
            Initialised = Bass.Init(Bass.NoSoundDevice) || Bass.LastError == Errors.Already;
            InitError = Bass.LastError;
        }
        catch (Exception ex) when (ex is DllNotFoundException or BadImageFormatException or EntryPointNotFoundException)
        {
            throw new InvalidOperationException(
                $"'{NativeLibraries.PathFor("bass")}' exists but could not be loaded ({ex.GetType().Name}); " +
                $"probably an architecture mismatch with the {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture} test host. " +
                $"Re-run {NativeLibraries.FetchCommand} --refresh.", ex);
        }

        if (!Initialised)
            throw new InvalidOperationException(FailureMessage);
    }

    public bool Initialised { get; }

    public Errors InitError { get; }

    public string FailureMessage =>
        $"Bass.Init(device {Bass.NoSoundDevice}) returned false, LastError = {InitError}, native '{NativeLibraries.PathFor("bass")}'.";

    public void Dispose() => Bass.Free();
}
