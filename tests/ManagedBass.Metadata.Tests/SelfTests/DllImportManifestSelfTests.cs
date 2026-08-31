using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class DllImportManifestSelfTests
{
    private static IReadOnlyDictionary<string, IReadOnlyList<string>> Manifest(params (string, string[])[] entries) =>
        entries.ToDictionary(e => e.Item1, IReadOnlyList<string> (e) => e.Item2, StringComparer.Ordinal);

    [Fact]
    public void Compare_OnIdenticalSets_ReturnsNoDifferences_WhenOrderDiffers() =>
        DllImportManifest.Compare(Manifest(("A", ["x", "y"])), Manifest(("A", ["x", "y"]))).Should().BeEmpty();

    [Fact]
    public void Compare_OnRenamedLibrary_ReportsAssembly_WhenNameChanged() =>
        DllImportManifest.Compare(Manifest(("A", ["bass"])), Manifest(("A", ["__Internal"])))
            .Should().ContainSingle().Which.Assembly.Should().Be("A");

    [Fact]
    public void Compare_OnMissingAssembly_ReportsBothDirections_WhenSetsDisagree()
    {
        var sut = DllImportManifest.Compare(Manifest(("A", ["bass"])), Manifest(("B", ["bass"])));

        sut.Select(d => d.Assembly).Should().Equal("A", "B");
    }

    [Fact]
    public void Serialise_OnRoundTrip_ParsesBackToSortedEntries_WhenUnsorted()
    {
        var json = DllImportManifest.Serialise(Manifest(("B", ["z", "a"]), ("A", ["bass"])));
        var sut = DllImportManifest.Parse(json);

        sut.Keys.Should().Equal("A", "B");
        sut["B"].Should().Equal("a", "z");
    }
}
