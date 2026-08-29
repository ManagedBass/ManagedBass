using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 6: the iOS registration path is still wired end to end, and the resolver is still public API.</summary>
public class NativeResolverWiringTests
{
    public static TheoryData<string> TargetFrameworks => new(RepoLayout.StandardTargetFrameworks);

    [Fact]
    public void Targets_OnDescribe_ContributesInitialiserInsideTarget_WhenChecked()
    {
        var facts = NativeResolverWiring.DescribeTargets(RepoLayout.ReadText(RepoLayout.TargetsRelativePath));

        facts.ContributesInitialiser.Should().BeTrue("naming the platforms is not enough; the <Compile Include> is the file's purpose");
        facts.ContributionInsideTarget.Should().BeTrue();
        facts.ContributionGatedOnApplePlatform.Should().BeTrue("the ItemGroup must key on the platform decision, or desktop builds get the initialiser too");
        facts.RunsBeforeCompile.Should().BeTrue();
    }

    [Fact]
    public void Targets_OnDescribe_DecidesPlatformInsideTargetOnly_WhenChecked()
    {
        var facts = NativeResolverWiring.DescribeTargets(RepoLayout.ReadText(RepoLayout.TargetsRelativePath));

        facts.DecidesOutsideTarget.Should().BeFalse("$(TargetPlatformIdentifier) is empty at import time");
        facts.MissingPlatforms.Should().BeEmpty();
    }

    [Fact]
    public void Initialiser_OnDescribe_RegistersResolverFromModuleInitializer_WhenChecked()
    {
        var facts = NativeResolverWiring.DescribeInitialiser(RepoLayout.ReadText(RepoLayout.InitialiserRelativePath));

        facts.Should().Be(new InitialiserFacts(true, true));
    }

    [Theory, MemberData(nameof(TargetFrameworks))]
    public void CoreAssembly_OnInspect_ExposesPublicResolver_WhenFrameworkBuilt(string tfm)
    {
        var core = RepoLayout.ShippingProjects().Single(p => p.AssemblyName == "ManagedBass").StandardOutput(tfm);
        BuildOutputs.Exists(core).Should().BeTrue($"{core} must be built");

        var facts = AssemblyFacts.PublicType(BuildOutputs.Read(core), NativeResolverWiring.ResolverTypeName);

        facts.Exists.Should().BeTrue();
        facts.IsPublic.Should().BeTrue();
        facts.MissingMembers(NativeResolverWiring.RequiredResolverMembers).Should().BeEmpty($"found [{string.Join(", ", facts.PublicStaticMethods)}]");
    }
}
