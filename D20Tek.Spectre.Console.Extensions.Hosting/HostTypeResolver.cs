//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Type resolver for Spectre.Console that fuses an already-built Generic Host
/// <see cref="IServiceProvider"/> with the run-time registrations captured by a
/// <see cref="HostTypeRegistrar"/>. Host-owned services are resolved directly from the
/// provider, while types registered by Spectre.Console.Cli at run time (for example command
/// types) are constructed with <see cref="ActivatorUtilities"/> so their constructor
/// dependencies are injected from the host provider.
/// </summary>
public sealed class HostTypeResolver : ITypeResolver
{
    private readonly CompositeServiceProvider _composite;

    /// <summary>
    /// Constructor that takes the host service provider and the captured registrations.
    /// </summary>
    /// <param name="provider">The host's already-built service provider.</param>
    /// <param name="registrations">The registrations captured by the registrar.</param>
    public HostTypeResolver(IServiceProvider provider, IReadOnlyDictionary<Type, HostRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        ArgumentNullException.ThrowIfNull(registrations, nameof(registrations));

        _composite = new CompositeServiceProvider(provider, registrations);
    }

    /// <summary>
    /// Resolves an instance of the specified type. Resolution is delegated to a composite
    /// provider that resolves host-owned services first and otherwise constructs captured
    /// run-time registrations with <see cref="ActivatorUtilities"/>, so nested dependencies on
    /// other run-time registrations are also satisfied.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>An instance of the specified type, or null if it cannot be resolved.</returns>
    public object? Resolve(Type? type) => type is null ? null : _composite.GetService(type);
}
