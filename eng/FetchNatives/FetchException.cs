namespace FetchNatives;

/// <summary>A failure the user can act on; reported as a one-line message rather than a stack trace.</summary>
public sealed class FetchException : Exception
{
    public FetchException(string message) : base(message) { }
}
