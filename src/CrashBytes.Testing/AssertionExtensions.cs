using System.Collections;

namespace CrashBytes.Testing;

/// <summary>
/// Extension methods providing fluent assertion syntax for tests.
/// All methods throw <see cref="AssertionException"/> on failure.
/// </summary>
public static class AssertionExtensions
{
    /// <summary>
    /// Asserts that the actual value equals the expected value.
    /// </summary>
    public static void ShouldBe<T>(this T actual, T expected)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            throw new AssertionException($"Expected <{Format(expected)}> but was <{Format(actual)}>.");
    }

    /// <summary>
    /// Asserts that the actual value does not equal the expected value.
    /// </summary>
    public static void ShouldNotBe<T>(this T actual, T expected)
    {
        if (EqualityComparer<T>.Default.Equals(actual, expected))
            throw new AssertionException($"Expected value to differ from <{Format(expected)}>, but they are equal.");
    }

    /// <summary>
    /// Asserts that the value is null.
    /// </summary>
    public static void ShouldBeNull<T>(this T? actual) where T : class
    {
        if (actual is not null)
            throw new AssertionException($"Expected null but was <{Format(actual)}>.");
    }

    /// <summary>
    /// Asserts that the value is not null.
    /// </summary>
    public static void ShouldNotBeNull<T>(this T? actual) where T : class
    {
        if (actual is null)
            throw new AssertionException("Expected a non-null value but was null.");
    }

    /// <summary>
    /// Asserts that a collection contains the specified item.
    /// </summary>
    public static void ShouldContain<T>(this IEnumerable<T> collection, T item)
    {
        if (collection is null)
            throw new AssertionException("Collection is null.");

        if (!collection.Contains(item))
            throw new AssertionException($"Expected collection to contain <{Format(item)}>, but it was not found.");
    }

    /// <summary>
    /// Asserts that a collection is empty.
    /// </summary>
    public static void ShouldBeEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection is null)
            throw new AssertionException("Collection is null.");

        if (collection.Any())
            throw new AssertionException("Expected collection to be empty, but it contains elements.");
    }

    /// <summary>
    /// Asserts that a collection is not empty.
    /// </summary>
    public static void ShouldNotBeEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection is null)
            throw new AssertionException("Collection is null.");

        if (!collection.Any())
            throw new AssertionException("Expected collection to contain elements, but it is empty.");
    }

    /// <summary>
    /// Asserts that the specified action throws an exception of type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">The expected exception type.</typeparam>
    /// <param name="action">The action that should throw.</param>
    /// <returns>The thrown exception for further inspection.</returns>
    public static TException ShouldThrow<TException>(this Action action) where TException : Exception
    {
        try
        {
            action();
        }
        catch (TException ex)
        {
            return ex;
        }
        catch (Exception ex)
        {
            throw new AssertionException(
                $"Expected exception of type <{typeof(TException).Name}> but caught <{ex.GetType().Name}>: {ex.Message}");
        }

        throw new AssertionException(
            $"Expected exception of type <{typeof(TException).Name}> but no exception was thrown.");
    }

    /// <summary>
    /// Asserts that the value is greater than the specified threshold.
    /// </summary>
    public static void ShouldBeGreaterThan<T>(this T actual, T threshold) where T : IComparable<T>
    {
        if (actual.CompareTo(threshold) <= 0)
            throw new AssertionException($"Expected <{Format(actual)}> to be greater than <{Format(threshold)}>.");
    }

    /// <summary>
    /// Asserts that the value is less than the specified threshold.
    /// </summary>
    public static void ShouldBeLessThan<T>(this T actual, T threshold) where T : IComparable<T>
    {
        if (actual.CompareTo(threshold) >= 0)
            throw new AssertionException($"Expected <{Format(actual)}> to be less than <{Format(threshold)}>.");
    }

    private static string Format<T>(T value) => value?.ToString() ?? "null";
}
