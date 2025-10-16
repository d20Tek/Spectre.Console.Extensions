//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using LightInject;
using Spectre.Console.Cli;

namespace D20Tek.Spectre.Console.Extensions.Injection;

/// <summary>
/// Type registry for Spectre.Console that uses the LightInject
/// framework to register types with the DI engine.
/// </summary>
public sealed class LightInjectTypeRegistrar : ITypeRegistrar
{
    private readonly ServiceContainer _container;

    /// <summary>
    /// Constructor that takes a service container instance.
    /// </summary>
    /// <param name="container">Service registry to use for registering types.</param>
    public LightInjectTypeRegistrar(ServiceContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        _container = container;
    }

    /// <summary>
    /// Builds the type resolver representing the registrations
    /// specified in the current instance.
    /// </summary>
    /// <returns>A type resolver.</returns>
    public ITypeResolver Build() => new LightInjectTypeResolver(_container);

    /// <summary>
    /// Registers the specified service.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="implementation">The implementation type.</param>
    public void Register(Type service, Type implementation)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

        _container.Register(service, implementation);
    }

    /// <summary>
    /// Registers the specified instance.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="implementation">The implementation.</param>
    public void RegisterInstance(Type service, object implementation)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));

        _container.RegisterInstance(service, implementation);
    }

    /// <summary>
    /// Registers the specified instance lazily.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="factoryMethod">The factory that creates the implementation.</param>
    public void RegisterLazy(Type service, Func<object> factoryMethod)
    {
        ArgumentNullException.ThrowIfNull(service, nameof(service));
        ArgumentNullException.ThrowIfNull(factoryMethod, nameof(factoryMethod));

        _container.Register(service, (factory) => factoryMethod());
    }
}
