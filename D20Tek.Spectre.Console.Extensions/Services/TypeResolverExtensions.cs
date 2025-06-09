using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Services;

/// <summary>
/// Extension methods for resolving services (similar to .NET DI extensions) from an <see cref="ITypeResolver"/>.
/// </summary>
public static class TypeResolverExtensions
{
    /// <summary>
    /// Gets a service of type <typeparamref name="T"/> from the resolver.
    /// </summary>
    /// <typeparam name="T">The type of service to get.</typeparam>
    /// <param name="resolver">The TypeResolver.</param>
    /// <returns>Service instance of type <typeparamref name="T"/>, or null if not found.</returns>
    public static T? GetService<T>(this ITypeResolver resolver)
        where T : class =>
        resolver.Resolve(typeof(T)) as T;

    /// <summary>
    /// Gets a required service of type <typeparamref name="T"/> from the resolver.
    /// </summary>
    /// <typeparam name="T">The type of service to get.</typeparam>
    /// <param name="resolver">The TypeResolver.</param>
    /// <returns>Service instance of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the service cannot be resolved.</exception>
    public static T GetRequiredService<T>(this ITypeResolver resolver)
        where T : class
        => resolver.Resolve(typeof(T)) as T ??
            throw new InvalidOperationException($"Cannot resolve service of type: {typeof(T)}.");
}
