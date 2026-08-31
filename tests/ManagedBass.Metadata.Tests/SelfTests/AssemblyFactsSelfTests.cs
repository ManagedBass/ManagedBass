using System.Reflection;
using System.Runtime.InteropServices;
using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

/// <summary>Reads this very assembly, which carries deliberate specimens, so the reader is proven against real metadata.</summary>
public class AssemblyFactsSelfTests
{
    private static byte[] Self => File.ReadAllBytes(typeof(AssemblyFactsSelfTests).Assembly.Location);

    [Fact]
    public void DllImports_OnRead_RecoversModuleName_WhenInternalDllImportImportExists()
    {
        var sut = AssemblyFacts.DllImports(Self);

        sut.Should().Contain(s => s.Module == "__Internal" && s.Method == nameof(Specimen.InternalDllImportImport));
        sut.Should().Contain(s => s.Module == "bass" && s.Method == nameof(Specimen.DynamicImport));
    }

    [Fact]
    public void DistinctModules_OnRead_ReturnsSortedUniqueNames_WhenMultipleImports() =>
        AssemblyFacts.DistinctModules(Self).Should().Contain(["__Internal", "bass"]).And.BeInAscendingOrder();

    [Fact]
    public void PublicType_OnRead_ReportsPublicStaticSignatures_WhenTypeIsPublic()
    {
        var sut = AssemblyFacts.PublicType(Self, typeof(PublicSpecimen).FullName!);

        sut.Exists.Should().BeTrue();
        sut.IsPublic.Should().BeTrue();
        sut.PublicStaticMethods.Should().Contain(["RegisterAll()", "Register(System.Reflection.Assembly)"]);
        sut.PublicStaticMethods.Should().NotContain(m => m.StartsWith("Hidden"), "internal and instance members are not public static surface");
        sut.MissingMembers(["RegisterAll()", "Missing()"]).Should().Equal("Missing()");
    }

    [Fact]
    public void PublicType_OnRead_ReportsNotPublic_WhenTypeIsInternal()
    {
        var sut = AssemblyFacts.PublicType(Self, typeof(Specimen).FullName!);

        sut.Exists.Should().BeTrue();
        sut.IsPublic.Should().BeFalse();
        sut.MissingMembers(["RegisterAll()"]).Should().Equal("RegisterAll()");
    }

    [Fact]
    public void PublicType_OnRead_ReportsAbsent_WhenTypeDoesNotExist() =>
        AssemblyFacts.PublicType(Self, "ManagedBass.Metadata.Tests.DoesNotExist").Exists.Should().BeFalse();

    private static class Specimen
    {
        [DllImport("__Internal")]
        public static extern void InternalDllImportImport();

        [DllImport("bass")]
        public static extern void DynamicImport();
    }
}

public class PublicSpecimen
{
    public static void RegisterAll() { }
    public static void Register(Assembly assembly) => _ = assembly;
    internal static void HiddenInternal() { }
    public void HiddenInstance(int _) { }
}
