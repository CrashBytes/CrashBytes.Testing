namespace CrashBytes.Testing;

/// <summary>
/// Interface for abstracting time in application code, enabling testable time-dependent logic.
/// </summary>
public interface ITimeProvider
{
    /// <summary>
    /// Gets the current time.
    /// </summary>
    DateTimeOffset Now { get; }
}

/// <summary>
/// A controllable time provider for deterministic testing of time-dependent code.
/// </summary>
public sealed class FakeTimeProvider : ITimeProvider
{
    private DateTimeOffset _current;

    /// <summary>
    /// Creates a new <see cref="FakeTimeProvider"/> initialized to the current UTC time.
    /// </summary>
    public FakeTimeProvider()
    {
        _current = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new <see cref="FakeTimeProvider"/> initialized to the specified time.
    /// </summary>
    /// <param name="startTime">The initial time.</param>
    public FakeTimeProvider(DateTimeOffset startTime)
    {
        _current = startTime;
    }

    /// <summary>
    /// Gets the current fake time.
    /// </summary>
    public DateTimeOffset Now => _current;

    /// <summary>
    /// Advances the current time by the specified duration.
    /// </summary>
    /// <param name="duration">The amount of time to advance.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when duration is negative.</exception>
    public FakeTimeProvider Advance(TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must not be negative.");

        _current = _current.Add(duration);
        return this;
    }

    /// <summary>
    /// Sets the current time to a specific value.
    /// </summary>
    /// <param name="time">The time to set.</param>
    public FakeTimeProvider Set(DateTimeOffset time)
    {
        _current = time;
        return this;
    }
}
