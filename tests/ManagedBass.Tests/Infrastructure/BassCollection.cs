namespace ManagedBass.Tests.Infrastructure;

/// <summary>
/// Every test that touches BASS's process-global device state shares one <see cref="BassFixture"/>.
/// </summary>
[CollectionDefinition(Name)]
public class BassCollection : ICollectionFixture<BassFixture>
{
    public const string Name = "BASS native";
}
