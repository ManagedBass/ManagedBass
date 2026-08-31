namespace ManagedBass.Tests;

/// <summary>
/// The resolver exists for Apple mobile targets; on desktop it must be a harmless no-op.
/// </summary>
[Collection(BassCollection.Name)]
public class NativeLibraryResolverTests
{
    [Fact]
    public void NativeLibraryResolver_OnRegisterAll_LeavesBassUsable_WhenOnDesktop()
    {
        NativeLibraryResolver.RegisterAll();
        NativeLibraryResolver.RegisterAll();
        NativeLibraryResolver.Register(typeof(Bass).Assembly);

        // Re-initialise after registration: proves the P/Invoke path still binds, not just that a cached call survives.
        Bass.Free().Should().BeTrue($"Free failed with {Bass.LastError}");
        Bass.Init(Bass.NoSoundDevice).Should().BeTrue($"Init after RegisterAll failed with {Bass.LastError}");
        Bass.Version.Should().BeGreaterThanOrEqualTo(new Version(2, 4, 17, 0));
    }
}
