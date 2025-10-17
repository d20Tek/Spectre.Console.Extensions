//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Type resolver for Spectre.Console that uses the Microsoft.Extensions.DependencyInjection
/// framework.
/// </summary>
public sealed class DependencyInjectionTypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;

    /// <summary>
    /// Constructor that takes a service provider instance.
    /// </summary>
    /// <param name="provider">Provider to use in type resolution.</param>
    public DependencyInjectionTypeResolver(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        _provider = provider;
    }

    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>An instance of the specified type, or null if no registration for that
    /// type exists.</returns>
    public object? Resolve(Type? type)
    {
        if (type == null) return null;
        return _provider.GetService(type);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
