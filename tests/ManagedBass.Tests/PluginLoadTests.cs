using ManagedBass.Flac;

namespace ManagedBass.Tests;

/// <summary>
/// Exercises <c>Bass.PluginLoad</c>'s per-OS probing (<c>name.dll</c>, <c>libname.so</c>, <c>libname.dylib</c>),
/// which nothing checks at compile time.
/// </summary>
[Collection(BassCollection.Name)]
public class PluginLoadTests
{
    const string PluginName = "bassflac";

    readonly ITestOutputHelper _output;

    public PluginLoadTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Bass_OnPluginLoad_ResolvesAddOn_WhenGivenBareName()
    {
        // Extension-less and directory-qualified: the branch that probes, not the short-circuit for a full file name.
        var bareName = Path.Combine(NativeLibraries.ProbeDirectory, PluginName);

        var handle = Bass.PluginLoad(bareName);
        handle.Should().NotBe(0, $"PluginLoad(\"{bareName}\") failed with {Bass.LastError} although {NativeLibraries.PathFor(PluginName)} exists");

        try
        {
            var info = Bass.PluginGetInfo(handle);
            var formats = info.Formats;
            _output.WriteLine($"BassFlac {info.Version}: " + string.Join(", ", formats.Select(f => f.FileExtensions)));

            // Empty would mean the IntPtr-to-struct-array walk marshalled nothing.
            formats.Should().NotBeEmpty();
        }
        finally
        {
            Bass.PluginFree(handle).Should().BeTrue($"PluginFree failed with {Bass.LastError}");
        }
    }

    [Fact]
    public void Bass_OnPluginLoad_ResolvesAddOn_WhenGivenExplicitPath()
    {
        var handle = Bass.PluginLoad(NativeLibraries.PathFor(PluginName));
        handle.Should().NotBe(0, $"PluginLoad failed with {Bass.LastError}");

        Bass.PluginFree(handle).Should().BeTrue();
    }

    [Fact]
    public void Bass_OnPluginLoad_ReportsError_WhenAddOnIsMissing()
    {
        Bass.PluginLoad(Path.Combine(NativeLibraries.ProbeDirectory, "bassdefinitelynotreal")).Should().Be(0);
        Bass.LastError.Should().NotBe(Errors.OK);
    }

    [Fact]
    public void BassFlac_OnCreateStream_ReportsFileFormat_WhenGivenGarbage()
    {
        // Goes through ManagedBass.Flac.dll's own DllImport, so the add-on assembly's binding is covered too.
        var garbage = new byte[512];

        BassFlac.CreateStream(garbage, 0, garbage.Length, BassFlags.Decode).Should().Be(0);
        Bass.LastError.Should().Be(Errors.FileFormat);
    }
}
