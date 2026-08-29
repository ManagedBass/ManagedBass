using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class DllNameGuardScannerSelfTests
{
    private const string Guarded = "class X\n{\n#if MANAGEDBASS_INTERNAL_DLLIMPORT\n        const string DllName = \"__Internal\";\n#else\n        const string DllName = \"bass\";\n#endif\n}";

    [Fact]
    public void Scan_OnGuardedFile_ReportsGuarded_WhenShapeExact() =>
        DllNameGuardScanner.Scan("x.cs", Guarded).IsGuarded.Should().BeTrue();

    [Fact]
    public void Scan_OnGuardedFile_ReportsGuarded_WhenTabIndentedAndCrLf() =>
        DllNameGuardScanner.Scan("x.cs", Guarded.Replace("        ", "\t\t").Replace("\n", "\r\n")).IsGuarded.Should().BeTrue();

    [Fact]
    public void Scan_OnUnguardedFile_ReportsUnguarded_WhenPlainDeclaration()
    {
        var sut = DllNameGuardScanner.Scan("x.cs", "class X\n{\n    const string DllName = \"bass\";\n}");

        sut.Should().Be(new DllNameGuardFacts("x.cs", 1, 0));
        sut.IsGuarded.Should().BeFalse();
    }

    [Fact]
    public void Scan_OnFile_ReportsUnguarded_WhenSecondDeclarationAddedOutsideGuard() =>
        DllNameGuardScanner.Scan("x.cs", Guarded + "\nclass Y\n{\n    const string DllName = \"other\";\n}").IsGuarded.Should().BeFalse();

    [Fact]
    public void Scan_OnFile_ReportsUnguarded_WhenArmsSwapped() =>
        DllNameGuardScanner.Scan("x.cs", Guarded.Replace("\"__Internal\"", "\"tmp\"").Replace("\"bass\"", "\"__Internal\"").Replace("\"tmp\"", "\"bass\"")).IsGuarded.Should().BeFalse();

    [Fact]
    public void Scan_OnUnguardedFile_ReportsUnguarded_WhenDeclarationHasAccessModifier()
    {
        var sut = DllNameGuardScanner.Scan("x.cs", "class X\n{\n    internal   const  string  DllName = \"bass\";\n}");

        sut.Should().Be(new DllNameGuardFacts("x.cs", 1, 0));
        sut.IsGuarded.Should().BeFalse();
    }

    [Fact]
    public void Scan_OnGuardedFile_ReportsGuarded_WhenBothArmsHaveAccessModifier() =>
        DllNameGuardScanner.Scan("x.cs", Guarded.Replace("const string", "private const string")).IsGuarded.Should().BeTrue();

    [Fact]
    public void DeclaresDllName_OnFile_IsFalse_WhenNoDeclaration() =>
        DllNameGuardScanner.DeclaresDllName("class X { string DllName => \"bass\"; }").Should().BeFalse();
}
