namespace CrashBytes.Testing;

/// <summary>
/// Deterministic random data generator for creating test fixtures.
/// Uses a seeded <see cref="Random"/> for reproducible output.
/// </summary>
public static class RandomData
{
    private static Random _random = new(42);
    private static readonly object _lock = new();

    private const string AlphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string LowerAlphaChars = "abcdefghijklmnopqrstuvwxyz";

    private static readonly string[] EmailDomains = { "example.com", "test.org", "fake.net", "sample.io", "demo.dev" };

    /// <summary>
    /// Resets the random generator with the specified seed for reproducible results.
    /// </summary>
    /// <param name="seed">The seed value.</param>
    public static void WithSeed(int seed)
    {
        lock (_lock)
        {
            _random = new Random(seed);
        }
    }

    /// <summary>
    /// Generates a random alphanumeric string of the specified length.
    /// </summary>
    /// <param name="length">The desired string length (default 10).</param>
    public static string String(int length = 10)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

        lock (_lock)
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                chars[i] = AlphanumericChars[_random.Next(AlphanumericChars.Length)];
            }
            return new string(chars);
        }
    }

    /// <summary>
    /// Generates a random email address.
    /// </summary>
    public static string Email()
    {
        lock (_lock)
        {
            var name = GenerateLowerString(8);
            var domain = EmailDomains[_random.Next(EmailDomains.Length)];
            return $"{name}@{domain}";
        }
    }

    /// <summary>
    /// Generates a random URL.
    /// </summary>
    public static string Url()
    {
        lock (_lock)
        {
            var path = GenerateLowerString(10);
            return $"https://example.com/{path}";
        }
    }

    /// <summary>
    /// Generates a random integer within the specified range (inclusive).
    /// </summary>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    public static int Int(int min = 0, int max = int.MaxValue - 1)
    {
        lock (_lock)
        {
            return _random.Next(min, max + 1);
        }
    }

    /// <summary>
    /// Generates a random GUID.
    /// </summary>
    public static Guid Guid()
    {
        lock (_lock)
        {
            var bytes = new byte[16];
            _random.NextBytes(bytes);
            return new Guid(bytes);
        }
    }

    /// <summary>
    /// Generates a random boolean value.
    /// </summary>
    public static bool Bool()
    {
        lock (_lock)
        {
            return _random.Next(2) == 1;
        }
    }

    /// <summary>
    /// Picks a random item from the given list.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="items">The list to pick from.</param>
    /// <exception cref="ArgumentException">Thrown when the list is null or empty.</exception>
    public static T Pick<T>(IReadOnlyList<T> items)
    {
        if (items is null || items.Count == 0)
            throw new ArgumentException("Items list must not be null or empty.", nameof(items));

        lock (_lock)
        {
            return items[_random.Next(items.Count)];
        }
    }

    private static string GenerateLowerString(int length)
    {
        var chars = new char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = LowerAlphaChars[_random.Next(LowerAlphaChars.Length)];
        }
        return new string(chars);
    }
}
