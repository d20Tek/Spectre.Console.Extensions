//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.Spectre.Console.Extensions.Hosting;

/// <summary>
/// Represents a single run-time registration captured by <see cref="HostTypeRegistrar"/>.
/// A registration is one of three kinds: an implementation type constructed on demand, a
/// pre-built instance, or a factory delegate. Implementation types are created with
/// <see cref="ActivatorUtilities"/> so their constructor dependencies are injected from the
/// host provider.
/// </summary>
/// <remarks>
/// Design note: this type exists instead of a secondary <see cref="IServiceCollection"/> /
/// <see cref="ServiceProvider"/> because Spectre.Console.Cli performs its registrations at
/// run time, after the host has already been built. By then the host's provider is immutable,
/// so there is no collection left to add to and rebuild. More importantly, command types that
/// Spectre registers (for example a command that injects IOptions&lt;T&gt;, ILogger&lt;T&gt;,
/// IConfiguration, or user services) must be constructed from the host's provider so those
/// dependencies resolve. A separate <see cref="ServiceProvider"/> would be an isolated
/// container that cannot see the host's registrations, would create duplicate singletons with
/// independent lifetimes, and would own its own disposal. This lightweight capture keeps the
/// host provider as the single source of truth for lifetimes: <see cref="ForType"/> defers
/// construction to <see cref="ActivatorUtilities.CreateInstance(IServiceProvider, Type, object[])"/>
/// against the host provider, while <see cref="ForInstance"/> and <see cref="ForFactory"/>
/// return the caller-supplied object directly.
/// </remarks>
public sealed class HostRegistration
{
    private readonly Type? _implementationType;
    private readonly object? _instance;
    private readonly Func<object>? _factory;

    private HostRegistration(Type? implementationType, object? instance, Func<object>? factory)
    {
        _implementationType = implementationType;
        _instance = instance;
        _factory = factory;
    }

    /// <summary>
    /// Creates a registration for an implementation type that is constructed on demand.
    /// </summary>
    /// <param name="implementationType">The concrete implementation type to construct.</param>
    /// <returns>A new <see cref="HostRegistration"/>.</returns>
    public static HostRegistration ForType(Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType, nameof(implementationType));
        return new HostRegistration(implementationType, instance: null, factory: null);
    }

    /// <summary>
    /// Creates a registration for a pre-built instance.
    /// </summary>
    /// <param name="instance">The instance to return on resolution.</param>
    /// <returns>A new <see cref="HostRegistration"/>.</returns>
    public static HostRegistration ForInstance(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        return new HostRegistration(implementationType: null, instance, factory: null);
    }

    /// <summary>
    /// Creates a registration for a factory delegate that produces the instance lazily.
    /// </summary>
    /// <param name="factory">The factory that creates the instance.</param>
    /// <returns>A new <see cref="HostRegistration"/>.</returns>
    public static HostRegistration ForFactory(Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory, nameof(factory));
        return new HostRegistration(implementationType: null, instance: null, factory);
    }

    /// <summary>
    /// Resolves the registration against the supplied provider. When constructing an
    /// implementation type, the provider is expected to be a composite that fuses the host
    /// provider with the captured registrations, so constructor dependencies on other run-time
    /// registrations are also satisfied.
    /// </summary>
    /// <param name="provider">The provider used to construct implementation types.</param>
    /// <returns>The resolved instance.</returns>
    public object Resolve(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        if (_instance is not null)
        {
            return _instance;
        }

        if (_factory is not null)
        {
            return _factory();
        }

        return ActivatorUtilities.CreateInstance(provider, _implementationType!);
    }
}
