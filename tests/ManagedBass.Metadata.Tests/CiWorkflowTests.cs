using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests;

/// <summary>Group 9: the skips in <see cref="InternalDllImportTests"/> and <see cref="PackageShapeTests"/> are only safe because CI produces their inputs first.</summary>
public class CiWorkflowTests
{
    [Fact]
    public void BuildTestWorkflow_OnRead_BuildsInternalDllImportBeforeMetadataTests_WhenStepsNamed() =>
        AssertPipeline(WorkflowAnalyser.BuildTestWorkflow);

    [Fact]
    public void PackageAndReleaseWorkflow_OnRead_PacksAndBuildsInternalDllImportBeforeMetadataTests_WhenStepsNamed() =>
        AssertPipeline(WorkflowAnalyser.PackageAndReleaseWorkflow);

    [Fact]
    public void IosSimulatorWorkflow_OnRead_TreatsMissingPassAsFailure_WhenEvaluated()
    {
        var handling = WorkflowAnalyser.DescribeOutcomeHandling(ReadWorkflow(WorkflowAnalyser.IosSimulatorWorkflow));

        handling.ChecksForPass.Should().BeTrue($"a step must look for '{WorkflowAnalyser.PassMarker}'");
        handling.FailsExplicitly.Should().BeTrue("a crashed app produces no marker; silence must exit non-zero");
    }

    [Fact]
    public void IosSimulatorWorkflow_OnRead_RunsBothDllImportVariants_WhenMatrixed()
    {
        var content = WorkflowAnalyser.StripCommentLines(ReadWorkflow(WorkflowAnalyser.IosSimulatorWorkflow));

        content.Should().Contain(WorkflowAnalyser.InternalDllImportFlag,
            "Tier 2b is the only run of the assemblies the Unity/iOS zip ships, and the only proof that __Internal symbol lookup works on a device");
        WorkflowAnalyser.DllImportModesAsserted(content).Should().Equal(new[] { "internal", "standard" },
            "both variants emit the same markers, so each leg has to pin the mode it expects or a wrong bundle passes twice");
    }

    private static void AssertPipeline(string fileName)
    {
        var pipeline = WorkflowAnalyser.DescribeMetadataPipeline(ReadWorkflow(fileName));

        pipeline.InternalDllImportBuild.Should().NotBeNull($"a step named '{WorkflowAnalyser.InternalDllImportStepName}' running '{BuildOutputs.InternalDllImportBuildCommand}' is required");
        pipeline.Pack.Should().NotBeNull($"a step running '{WorkflowAnalyser.PackCommand}' with '{WorkflowAnalyser.PackOutputOption}' is required, or the package-shape tests skip");
        pipeline.MetadataTest.Should().NotBeNull($"a step named '{WorkflowAnalyser.MetadataTestStepName}' is required");
        pipeline.InSameJob.Should().BeTrue("a producer in another job leaves nothing on this job's runner");
        pipeline.ProducersBeforeTest.Should().BeTrue("the internal-DllImport and package assertions skip when their inputs are absent, so the producers must come first");
        pipeline.ConditionalSteps.Should().BeEmpty("an 'if:' can turn a producer off without anything going red");
    }

    private static string ReadWorkflow(string fileName)
    {
        var path = $"{WorkflowAnalyser.WorkflowDirectory}/{fileName}";
        File.Exists(RepoLayout.Absolute(path)).Should().BeTrue($"{path} is part of the Tier 0 contract");
        return RepoLayout.ReadText(path);
    }
}
