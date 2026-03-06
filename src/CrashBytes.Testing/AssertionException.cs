namespace CrashBytes.Testing;

/// <summary>
/// Exception thrown when a test assertion fails.
/// </summary>
public sealed class AssertionException : Exception
{
    /// <summary>
    /// Creates a new <see cref="AssertionException"/> with the specified message.
    /// </summary>
    /// <param name="message">The failure message.</param>
    public AssertionException(string message) : base(message) { }

    /// <summary>
    /// Creates a new <see cref="AssertionException"/> with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The failure message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AssertionException(string message, Exception innerException) : base(message, innerException) { }
}
