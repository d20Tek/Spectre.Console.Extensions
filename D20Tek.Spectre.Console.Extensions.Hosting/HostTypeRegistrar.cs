//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Type registrar for Spectre.Console that bridges an already-built Generic Host
/// <see cref="IServiceProvider"/> to Spectre.Console.Cli. Because the host provider is
/// immutable once the host is built, this registrar captures the run-time registrations that
/// Spectre.Console.Cli performs (for example command types) into an internal map instead of
/// mutating the provider. The paired <see cref="HostTypeResolver"/> fuses the map with the
/// host provider so command constructor dependencies are injected from the host.
/// </summary>
/// <remarks>
/// Design note: a secondary <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>
/// was intentionally avoided. Spectre registers its types after <c>host.Build()</c>, when the
/// host provider is already immutable, and its command types depend on host-owned services
/// (options, logging, configuration, user services). Building a separate provider would create
/// an isolated container that cannot resolve those host dependencies and would duplicate
/// singleton lifetimes and disposal. Instead each registration is captured as a
/// <see cref="HostRegistration"/> and resolved lazily against the host provider, keeping the
/// host as the single container.
/// </remarks>
public sealed class HostTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceProvider _provider;
    private readonly Dictionary<Type, HostRegistration> _registrations = [];

    /// <summary>
    /// Constructor that takes the host's already-built service provider.
    /// </summary>
    /// <param name="provider">The host service provider used for resolution.</param>
    public HostTypeRegistrar(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        _provider = provider;
    }

    /// <summary>
    /// Builds the type resolver representing the host provider fused with the captured
    /// run-time registrations.
    /// </summary>
    /// <returns>A type resolver.</returns>
    public ITypeResolver Build() => new HostTypeResolver(_provider, _registrations);

    /// <summary>
    /// Registers the specified service and implementation type. The implementation is
    /// constructed on demand from the host provider when resolved.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="implementation">The implementation type.</param>
    public void Register(Type service, Type implementation)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

        _registrations[service] = HostRegistration.ForType(implementation);
    }

    /// <summary>
    /// Registers the specified pre-built instance for a service type.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="implementation">The instance.</param>
    public void RegisterInstance(Type service, object implementation)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

        _registrations[service] = HostRegistration.ForInstance(implementation);
    }

    /// <summary>
    /// Registers the specified service using a factory delegate evaluated lazily.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="factoryMethod">The factory that creates the implementation.</param>
    public void RegisterLazy(Type service, Func<object> factoryMethod)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(factoryMethod, nameof(factoryMethod));

        _registrations[service] = HostRegistration.ForFactory(factoryMethod);
    }
}
