using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class NativeResolverWiringSelfTests
{
    private const string Wired = """
        <Project>
          <Target Name="X" BeforeTargets="BeforeCompile;CoreCompile">
            <PropertyGroup>
              <_Apple Condition="'$(TargetPlatformIdentifier)' == 'ios' Or '$(TargetPlatformIdentifier)' == 'tvos' Or '$(TargetPlatformIdentifier)' == 'maccatalyst'">true</_Apple>
            </PropertyGroup>
            <ItemGroup Condition="'$(_Apple)' == 'true'">
              <Compile Include="$(MSBuildThisFileDirectory)ManagedBassNativeResolverInitialiser.cs" />
            </ItemGroup>
          </Target>
        </Project>
        """;

    // Mirrors the shipped file: the ItemGroup keys on an opt-out property whose default is the platform decision.
    private const string Chained = """
        <Project>
          <Target Name="X" BeforeTargets="BeforeCompile;CoreCompile">
            <PropertyGroup>
              <_Apple>false</_Apple>
              <_Apple Condition="'$(TargetPlatformIdentifier)' == 'ios' Or '$(TargetPlatformIdentifier)' == 'tvos' Or '$(TargetPlatformIdentifier)' == 'maccatalyst'">true</_Apple>
              <Register Condition="'$(Register)' == ''">$(_Apple)</Register>
            </PropertyGroup>
            <ItemGroup Condition="'$(Register)' == 'true' And '$(Language)' == 'C#'">
              <Compile Include="$(MSBuildThisFileDirectory)ManagedBassNativeResolverInitialiser.cs" />
            </ItemGroup>
          </Target>
        </Project>
        """;

    [Fact]
    public void DescribeTargets_OnWiredFile_ReportsFullyWired_WhenAllPiecesPresent()
    {
        var sut = NativeResolverWiring.DescribeTargets(Wired);

        sut.Should().BeEquivalentTo(new { ContributesInitialiser = true, ContributionInsideTarget = true, ContributionGatedOnApplePlatform = true, RunsBeforeCompile = true, DecidesOutsideTarget = false });
        sut.MissingPlatforms.Should().BeEmpty();
    }

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsGated_WhenItemGroupKeysOnDefaultedProperty() =>
        NativeResolverWiring.DescribeTargets(Chained).ContributionGatedOnApplePlatform.Should().BeTrue();

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsUngated_WhenPlatformDecisionYieldsFalse() =>
        NativeResolverWiring.DescribeTargets(Chained.Replace("'maccatalyst'\">true</_Apple>", "'maccatalyst'\">false</_Apple>"))
            .ContributionGatedOnApplePlatform.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsUngated_WhenItemGroupComparesToFalse() =>
        NativeResolverWiring.DescribeTargets(Chained.Replace("'$(Register)' == 'true'", "'$(Register)' == 'false'"))
            .ContributionGatedOnApplePlatform.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsUngated_WhenItemGroupConditionRemoved() =>
        NativeResolverWiring.DescribeTargets(Chained.Replace(" Condition=\"'$(Register)' == 'true' And '$(Language)' == 'C#'\"", ""))
            .ContributionGatedOnApplePlatform.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsUngated_WhenPropertyAlsoTrueUnconditionally() =>
        NativeResolverWiring.DescribeTargets(Chained.Replace("<_Apple>false</_Apple>", "<_Apple>true</_Apple>"))
            .ContributionGatedOnApplePlatform.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnChainedFile_ReportsUngated_WhenDefaultPointsAtUnrelatedProperty() =>
        NativeResolverWiring.DescribeTargets(Chained.Replace("$(_Apple)</Register>", "$(Other)</Register>"))
            .ContributionGatedOnApplePlatform.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnFile_ReportsNotBeforeCompile_WhenOrderedAfterCompile() =>
        NativeResolverWiring.DescribeTargets(Wired.Replace("BeforeTargets=\"BeforeCompile;CoreCompile\"", "BeforeTargets=\"AfterCompile\""))
            .RunsBeforeCompile.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnFile_ReportsNoContribution_WhenCompileIncludeDeleted() =>
        NativeResolverWiring.DescribeTargets(Wired.Replace("<Compile Include=\"$(MSBuildThisFileDirectory)ManagedBassNativeResolverInitialiser.cs\" />", ""))
            .ContributesInitialiser.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnFile_ReportsNoContribution_WhenIncludeNotRelativeToTargets() =>
        NativeResolverWiring.DescribeTargets(Wired.Replace("$(MSBuildThisFileDirectory)", "build/")).ContributesInitialiser.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnFile_ReportsDecisionOutsideTarget_WhenPropertyHoisted()
    {
        var hoisted = Wired.Replace("<Target Name=\"X\" BeforeTargets=\"BeforeCompile;CoreCompile\">", "").Replace("</Target>", "");

        var sut = NativeResolverWiring.DescribeTargets(hoisted);

        sut.DecidesOutsideTarget.Should().BeTrue();
        sut.ContributionInsideTarget.Should().BeFalse();
        sut.MissingPlatforms.Should().Equal("ios", "tvos", "maccatalyst");
    }

    [Fact]
    public void DescribeTargets_OnFile_ReportsMissingPlatform_WhenCatalystDropped() =>
        NativeResolverWiring.DescribeTargets(Wired.Replace(" Or '$(TargetPlatformIdentifier)' == 'maccatalyst'", "")).MissingPlatforms.Should().Equal("maccatalyst");

    [Fact]
    public void DescribeTargets_OnFile_ReportsNotBeforeCompile_WhenOrderingRemoved() =>
        NativeResolverWiring.DescribeTargets(Wired.Replace(" BeforeTargets=\"BeforeCompile;CoreCompile\"", "")).RunsBeforeCompile.Should().BeFalse();

    [Fact]
    public void DescribeTargets_OnFile_ReportsNothing_WhenNotXml() =>
        NativeResolverWiring.DescribeTargets("<Project><Target").ContributesInitialiser.Should().BeFalse();

    [Fact]
    public void DescribeInitialiser_OnSource_ReportsWired_WhenAttributeAndCallPresent() =>
        NativeResolverWiring.DescribeInitialiser("[System.Runtime.CompilerServices.ModuleInitializer]\nstatic void I() => global::ManagedBass.NativeLibraryResolver.RegisterAll();")
            .Should().Be(new InitialiserFacts(true, true));

    [Fact]
    public void DescribeInitialiser_OnSource_ReportsUnwired_WhenOnlyInComments() =>
        NativeResolverWiring.DescribeInitialiser("// [ModuleInitializer]\n/* NativeLibraryResolver.RegisterAll(); */\nstatic void I() { }")
            .Should().Be(new InitialiserFacts(false, false));

    [Fact]
    public void DescribeInitialiser_OnSource_ReportsNoCall_WhenBodyEmptied() =>
        NativeResolverWiring.DescribeInitialiser("[ModuleInitializer]\nstatic void I() { }").CallsRegisterAll.Should().BeFalse();
}
