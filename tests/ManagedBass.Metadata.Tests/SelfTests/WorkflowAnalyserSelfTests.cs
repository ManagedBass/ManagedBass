using ManagedBass.Metadata.Tests.Analysis;

namespace ManagedBass.Metadata.Tests.SelfTests;

public class WorkflowAnalyserSelfTests
{
    private const string Ordered = """
        steps:
          - name: Build (internal DllImport, Unity/iOS variant)
            run: dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true
          # - name: Test (metadata and package shape)
          - name: "Test (metadata and package shape)"
            run: dotnet test tests/ManagedBass.Metadata.Tests
        """;

    [Fact]
    public void Steps_OnWorkflow_ReturnsNamesInOrder_WhenQuotedAndCommented()
    {
        var sut = WorkflowAnalyser.Steps(Ordered);

        sut.Select(s => s.Name).Should().Equal(WorkflowAnalyser.InternalDllImportStepName, WorkflowAnalyser.MetadataTestStepName);
        sut[0].Body.Should().Contain(BuildOutputs.InternalDllImportBuildCommand);
    }

    [Fact]
    public void IndexOfStep_OnReorderedWorkflow_PutsTestFirst_WhenStepsSwapped()
    {
        var swapped = string.Join('\n', Ordered.Split('\n').Skip(3).Concat(Ordered.Split('\n').Skip(1).Take(2)));
        var steps = WorkflowAnalyser.Steps("steps:\n" + swapped);

        WorkflowAnalyser.IndexOfStep(steps, WorkflowAnalyser.MetadataTestStepName)
            .Should().BeLessThan(WorkflowAnalyser.IndexOfStep(steps, WorkflowAnalyser.InternalDllImportStepName));
    }

    [Fact]
    public void IndexOfStep_OnMissingStep_ReturnsMinusOne_WhenNotNamed() =>
        WorkflowAnalyser.IndexOfStep(WorkflowAnalyser.Steps(Ordered), "Nope").Should().Be(-1);

    private const string Pipeline = """
        jobs:
          metadata:
            runs-on: ubuntu-latest
            steps:
              - name: Build (internal DllImport, Unity/iOS variant)
                run: dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true
              - name: Pack
                run: dotnet pack src/ManagedBass.sln -c Release --no-build -o artifacts/packages
              - name: Test (metadata and package shape)
                run: dotnet test tests/ManagedBass.Metadata.Tests
              - name: Upload
                if: always()
                uses: actions/upload-artifact@v4
                with:
                  if-no-files-found: warn
          functional:
            steps:
              - name: Test (native interop)
                run: dotnet test tests/ManagedBass.Tests
        """;

    [Fact]
    public void Steps_OnMultiJobWorkflow_AttributesJobAndCondition_WhenStepsSpanJobs()
    {
        var sut = WorkflowAnalyser.Steps(Pipeline);

        sut.Select(s => (s.Name, s.Job, s.IsConditional)).Should().Equal(
            (WorkflowAnalyser.InternalDllImportStepName, "metadata", false),
            ("Pack", "metadata", false),
            (WorkflowAnalyser.MetadataTestStepName, "metadata", false),
            ("Upload", "metadata", true),
            ("Test (native interop)", "functional", false));
        sut[3].Body.Should().NotContain("functional:", "a step body ends at the next job");
    }

    [Fact]
    public void DescribeMetadataPipeline_OnOrderedJob_ReportsSafe_WhenProducersPrecedeTest()
    {
        var sut = WorkflowAnalyser.DescribeMetadataPipeline(Pipeline);

        sut.Should().BeEquivalentTo(new { AllPresent = true, InSameJob = true, ProducersBeforeTest = true });
        sut.ConditionalSteps.Should().BeEmpty();
    }

    [Fact]
    public void DescribeMetadataPipeline_OnWorkflow_ReportsDifferentJob_WhenInternalDllImportMovedToEarlierJob()
    {
        var internalDllImport = "      - name: Build (internal DllImport, Unity/iOS variant)\n        run: dotnet build src/ManagedBass.Shipping.slnf -c Release -p:BassInternalDllImport=true\n";
        var moved = "jobs:\n  earlier:\n    steps:\n" + internalDllImport + Pipeline["jobs:\n".Length..].Replace(internalDllImport, "");

        var sut = WorkflowAnalyser.DescribeMetadataPipeline(moved);

        sut.AllPresent.Should().BeTrue();
        sut.InSameJob.Should().BeFalse();
    }

    [Fact]
    public void DescribeMetadataPipeline_OnWorkflow_ReportsMissingPack_WhenPackStepDeleted()
    {
        var withoutPack = Pipeline.Replace("      - name: Pack\n        run: dotnet pack src/ManagedBass.sln -c Release --no-build -o artifacts/packages\n", "");

        var sut = WorkflowAnalyser.DescribeMetadataPipeline(withoutPack);

        sut.Pack.Should().BeNull();
        sut.AllPresent.Should().BeFalse();
    }

    [Fact]
    public void DescribeMetadataPipeline_OnWorkflow_ReportsMissingPack_WhenOutputDirectoryDropped() =>
        WorkflowAnalyser.DescribeMetadataPipeline(Pipeline.Replace(" -o artifacts/packages", "")).Pack.Should().BeNull();

    [Fact]
    public void DescribeMetadataPipeline_OnWorkflow_ReportsConditional_WhenInternalDllImportStepHasIf()
    {
        var guarded = Pipeline.Replace("      - name: Build (internal DllImport, Unity/iOS variant)\n", "      - name: Build (internal DllImport, Unity/iOS variant)\n        if: false\n");

        WorkflowAnalyser.DescribeMetadataPipeline(guarded).ConditionalSteps.Should().Equal(WorkflowAnalyser.InternalDllImportStepName);
    }

    [Fact]
    public void DescribeMetadataPipeline_OnWorkflow_ReportsOutOfOrder_WhenPackFollowsTest()
    {
        var pack = "      - name: Pack\n        run: dotnet pack src/ManagedBass.sln -c Release --no-build -o artifacts/packages\n";
        var late = Pipeline.Replace(pack, "").Replace("      - name: Upload\n", pack + "      - name: Upload\n");

        WorkflowAnalyser.DescribeMetadataPipeline(late).ProducersBeforeTest.Should().BeFalse();
    }

    [Fact]
    public void DescribeOutcomeHandling_OnEvaluateStep_ReportsNoFailure_WhenOnlyExitPrecedesCheck()
    {
        const string yaml = "  - name: Evaluate\n    run: |\n      [ -f out.txt ] || exit 1\n      grep -q '^outcome=PASS$' out.txt && echo ok\n";

        WorkflowAnalyser.DescribeOutcomeHandling(yaml).Should().Be(new OutcomeHandling(true, false));
    }

    [Fact]
    public void DescribeOutcomeHandling_OnEvaluateStep_ReportsExplicitFailure_WhenExitOneAfterCheck()
    {
        const string yaml = "  - name: Evaluate\n    run: |\n      if grep -q '^outcome=PASS$' out.txt; then exit 0; fi\n      echo missing; exit 1\n";

        WorkflowAnalyser.DescribeOutcomeHandling(yaml).Should().Be(new OutcomeHandling(true, true));
    }

    [Fact]
    public void DescribeOutcomeHandling_OnEvaluateStep_ReportsNoFailure_WhenSilenceFallsThrough() =>
        WorkflowAnalyser.DescribeOutcomeHandling("  - name: Evaluate\n    run: grep 'outcome=PASS' out.txt || echo 'no result'\n")
            .Should().Be(new OutcomeHandling(true, false));

    [Fact]
    public void DescribeOutcomeHandling_OnWorkflow_ReportsNoCheck_WhenMarkerOnlyInComment() =>
        WorkflowAnalyser.DescribeOutcomeHandling("  - name: Evaluate\n    # looks for outcome=PASS\n    run: exit 1\n").ChecksForPass.Should().BeFalse();

    [Fact]
    public void DllImportModesAsserted_OnMatrix_ReturnsBothModes_WhenEachLegPinsOne() =>
        WorkflowAnalyser.DllImportModesAsserted("          - expected_mode: 'dllimportmode=standard'\n          - expected_mode: 'dllimportmode=internal'\n")
            .Should().Equal("internal", "standard");

    [Fact]
    public void DllImportModesAsserted_OnMatrix_IgnoresComments_WhenModeOnlyMentionedInProse() =>
        WorkflowAnalyser.DllImportModesAsserted("# dllimportmode=internal is the point of Tier 2b\n          - expected_mode: 'dllimportmode=standard'\n")
            .Should().Equal("standard");

    [Fact]
    public void StripCommentLines_OnYaml_KeepsHashInsideValues_WhenNotLineStart() =>
        WorkflowAnalyser.StripCommentLines("  # gone\n  run: echo 'a # b'").Should().Be("  run: echo 'a # b'");
}
