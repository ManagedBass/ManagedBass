namespace FetchNatives;

/// <summary>Console output, prefixed so it stands out in MSBuild's log.</summary>
public static class Report
{
    public static void Info(string message) => Console.WriteLine($"fetch-natives: {message}");

    public static void Error(string message) => Console.Error.WriteLine($"fetch-natives: error: {message}");
}
