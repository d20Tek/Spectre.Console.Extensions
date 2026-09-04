//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// An <see cref="IServiceProvider"/> that fuses the host's already-built provider with the
/// run-time registrations captured by a <see cref="HostTypeRegistrar"/>. Host-owned services
/// are resolved first; otherwise a captured <see cref="HostRegistration"/> is used.
/// </summary>
/// <remarks>
/// Design note: this composite is what <see cref="HostRegistration.Resolve"/> hands to
/// <see cref="Microsoft.Extensions.DependencyInjection.ActivatorUtilities"/> when constructing
/// an implementation type. Passing the raw host provider would only resolve constructor
/// dependencies that the host knows about, so a Spectre-registered type that depends on another
/// Spectre-registered type would fail. Resolving through this composite lets those chained
/// run-time registrations satisfy each other while host services still take precedence.
/// </remarks>
internal sealed class CompositeServiceProvider(
    IServiceProvider hostProvider,
    IReadOnlyDictionary<Type, HostRegistration> registrations) : IServiceProvider
{
    private readonly IServiceProvider _hostProvider = hostProvider;
    private readonly IReadOnlyDictionary<Type, HostRegistration> _registrations = registrations;

    public object? GetService(Type serviceType)
    {
        var fromHost = _hostProvider.GetService(serviceType);
        if (fromHost is not null)
        {
            return fromHost;
        }

        if (_registrations.TryGetValue(serviceType, out var registration))
        {
            return registration.Resolve(this);
        }

        return null;
    }
}
