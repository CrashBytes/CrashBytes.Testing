using System.Linq.Expressions;
using System.Reflection;

namespace CrashBytes.Testing;

/// <summary>
/// Fluent test data builder for any class with a parameterless constructor.
/// </summary>
/// <typeparam name="T">The type to build.</typeparam>
public sealed class ObjectBuilder<T> where T : class, new()
{
    private readonly List<Action<T>> _configurations = new();

    private ObjectBuilder() { }

    /// <summary>
    /// Creates a new builder instance.
    /// </summary>
    public static ObjectBuilder<T> Create() => new();

    /// <summary>
    /// Sets a property value via a member expression.
    /// </summary>
    /// <typeparam name="TProp">The property type.</typeparam>
    /// <param name="expression">An expression selecting the property to set.</param>
    /// <param name="value">The value to assign.</param>
    public ObjectBuilder<T> With<TProp>(Expression<Func<T, TProp>> expression, TProp value)
    {
        var memberExpression = expression.Body as MemberExpression
            ?? throw new ArgumentException("Expression must be a member access expression.", nameof(expression));

        var property = memberExpression.Member as PropertyInfo
            ?? throw new ArgumentException("Expression must reference a property.", nameof(expression));

        _configurations.Add(obj => property.SetValue(obj, value));
        return this;
    }

    /// <summary>
    /// Applies a bulk configuration action to the object.
    /// </summary>
    /// <param name="configure">An action that configures the object.</param>
    public ObjectBuilder<T> With(Action<T> configure)
    {
        _configurations.Add(configure ?? throw new ArgumentNullException(nameof(configure)));
        return this;
    }

    /// <summary>
    /// Builds and returns the configured object.
    /// </summary>
    public T Build()
    {
        var obj = new T();
        foreach (var config in _configurations)
        {
            config(obj);
        }
        return obj;
    }

    /// <summary>
    /// Builds multiple objects with sequential index passed to an optional customizer.
    /// Each object gets the base configurations applied, then receives its zero-based index
    /// for further customization.
    /// </summary>
    /// <param name="count">Number of objects to create.</param>
    /// <param name="indexCustomizer">Optional action receiving the object and its zero-based index.</param>
    public List<T> BuildMany(int count, Action<T, int>? indexCustomizer = null)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");

        var results = new List<T>(count);
        for (var i = 0; i < count; i++)
        {
            var obj = Build();
            indexCustomizer?.Invoke(obj, i);
            results.Add(obj);
        }
        return results;
    }
}
