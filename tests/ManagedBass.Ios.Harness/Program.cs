using UIKit;

namespace ManagedBass.Ios.Harness;

/// <summary>Entry point for the Tier 2 iOS simulator harness.</summary>
public static class Program
{
    public static void Main(string[] args) => UIApplication.Main(args, null, typeof(AppDelegate));
}
